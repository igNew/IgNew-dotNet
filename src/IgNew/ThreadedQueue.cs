using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IgNew
{
    /// <summary>
    /// Standard EventArgs object for ItemReceived events.
    /// </summary>
    /// <typeparam name="T" />
    public class ItemReceivedEventArgs<T> : EventArgs
    {
        private readonly T _item;

        /// <summary>
        /// Item dequeued from threaded queue.
        /// </summary>
        public T Item { get { return _item; } }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="item">Item dequeued from threaded queue.</param>
        public ItemReceivedEventArgs(T item)
        {
            _item = item;
        }
    }

    /// <summary>
    /// Runs in a separate thread and listens for messages of a certain type to be dropped in.
    /// Processes entries in order of lowest priority, then by the order they were received.
    /// 
    /// Useful for martialling operations from multiple worker threads without impacting overall
    /// app responsiveness.
    /// </summary>
    /// <typeparam name="T">Type of item for queue.</typeparam>
    public class ThreadedQueue<T> : IDisposable
    {
        const int DefaultPriority = 5;

        /// <summary>
        /// Delegate definition for ItemReceivedEventHandler.
        /// </summary>
        /// <param name="sender">This threaded queue</param>
        /// <param name="e">EventArgs object containing the item received</param>
        public delegate void ItemReceivedEventHandler(object sender, ItemReceivedEventArgs<T> e);

        /// <summary>
        /// Fires on unhandled exception in the thread. Will rethrow the error if not defined.
        /// Ignores ThreadAbortException, obviously. (This event fires in the Threaded Queue's
        /// threadspace.)
        /// </summary>
        public event UnhandledExceptionEventHandler UnhandledException;
        
        /// <summary>
        /// Fires when an item is processed off the queue. (This event fires in the Threaded Queue's
        /// threadspace.)
        /// </summary>
        public event ItemReceivedEventHandler ItemReceived;

        private bool _disposed;
        private readonly object _syncLock = new object();
        private readonly ManualResetEvent _queueAvailable = new ManualResetEvent(false);
        private readonly SortedList<int, Queue<T>> _queues = new SortedList<int, Queue<T>>();
        private Thread _thread;

        ~ThreadedQueue()
        {
            PrivateDispose();
        }

        public void Dispose()
        {
            PrivateDispose();
            GC.SuppressFinalize(this);
        }

        private void PrivateDispose()
        {
            if (_disposed) return;

            Stop();
            _disposed = true;
        }

        /// <summary>
        /// Place an item on the queue. Threadsafe.
        /// </summary>
        /// <param name="item">Item to add to the queue.</param>
        /// <param name="priority">Item priority. Lowest value goes first.</param>
        public void Enqueue(T item, int priority = DefaultPriority)
        {
            lock (_syncLock)
            {
                Queue<T> queue;

                if (_queues.ContainsKey(priority))
                    queue = _queues[priority];
                else
                {
                    queue = new Queue<T>();
                    _queues.Add(priority, queue);
                }

                queue.Enqueue(item);
            }

            _queueAvailable.Set();
        }

        private bool Dequeue(out T item)
        {
            lock (_syncLock)
            {
                while (_queues.Any())
                {
                    var queue = _queues.Values.First();

                    if (queue.Any())
                    {
                        item = queue.Dequeue();
                        return true;
                    }

                    _queues.RemoveAt(0);
                }
            }

            item = default(T);
            return false;
        }

        protected virtual void OnUnhandledException(Exception ex)
        {
            if (UnhandledException != null)
                UnhandledException(this, new UnhandledExceptionEventArgs(ex, false));
            else
                throw ex;
        }

        protected virtual void OnItemReceived(T item)
        {
            if (ItemReceived != null) ItemReceived(this, new ItemReceivedEventArgs<T>(item));
        }

        private void Execute()
        {
            try
            {
                for(;;)
                {
                    _queueAvailable.WaitOne();

                    T item;
                    
                    while (Dequeue(out item))
                    {
                        OnItemReceived(item);
                    }
                }
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception ex)
            {
                OnUnhandledException(ex);
            }
        }

        /// <summary>
        /// Starts the threaded queue.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the queue has already been started.</exception>
        public void Start()
        {
            if (_thread != null) throw new InvalidOperationException("Threaded queue has already been started.");

            _thread = new Thread(Execute);
            _thread.Start();
        }

        /// <summary>
        /// Stops the threaded queue. Does nothing if the queue isn't running.
        /// </summary>
        public void Stop()
        {
            if (_thread != null)
            {
                _thread.Abort();
                _thread.Join();
                _thread = null;
            }
        }
    }
}
