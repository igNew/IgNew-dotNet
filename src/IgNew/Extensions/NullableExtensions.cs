// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullableExtensions.cs" company="IgNew LLC">
//   Copywright (c) 2012 IgNew LLC. All rights reserved.
// </copyright>
// <summary>
//   extension methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace IgNew.Extensions
{
    #region Using Directives

    using System;

    #endregion

    /// <summary>
    ///   <see cref="Nullable" /> extension methods.
    /// </summary>
    public static class NullableExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets the value of the current <see cref="Nullable{T}"/> if one is set, otherwise the defaut for the type is returned.
        /// </summary>
        /// <typeparam name="T">
        /// The underlying value type of the <see cref="Nullable{T}"/> generic type. 
        /// </typeparam>
        /// <param name="value">
        /// The object being extended. 
        /// </param>
        /// <returns>
        /// Value if set, default otherwise. 
        /// </returns>
        public static T ValueOrDefault<T>(this T? value) where T : struct
        {
            return value.ValueOrDefault(default(T));
        }

        /// <summary>
        /// Gets the value of the current <see cref="Nullable{T}"/> if one is set, otherwise the provided default value is returned.
        /// </summary>
        /// <typeparam name="T">
        /// The underlying value type of the <see cref="Nullable{T}"/> generic type. 
        /// </typeparam>
        /// <param name="value">
        /// The object being extended. 
        /// </param>
        /// <param name="defaultValue">
        /// The default value to return if no value has been set. 
        /// </param>
        /// <returns>
        /// Value if set, default otherwise. 
        /// </returns>
        public static T ValueOrDefault<T>(this T? value, T defaultValue) where T : struct
        {
            if (!value.HasValue)
            {
                return defaultValue;
            }

            return value.Value;
        }

        #endregion
    }
}