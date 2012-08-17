// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullableExtensionsTests.cs" company="IgNew LLC">
//   Copywright (c) 2012 IgNew LLC. All rights reserved.
// </copyright>
// <summary>
//   Unit tests for
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace IgNew.Tests.Extensions
{
    #region Using Directives

    using IgNew.Extensions;

    using NUnit.Framework;

    #endregion

    /// <summary>
    ///   Unit tests for <see cref="NullableExtensions" />
    /// </summary>
    [TestFixture]
    public class NullableExtensionsTests
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The test_ value or default_default.
        /// </summary>
        [Test]
        public void Test_ValueOrDefault_default()
        {
            int? nullable = null;

            Assert.That(nullable.ValueOrDefault(), Is.EqualTo(default(int)));
        }

        /// <summary>
        /// TODO The test_ value or default_value.
        /// </summary>
        [Test]
        public void Test_ValueOrDefault_value()
        {
            var value = 1234;

            int? nullable = value;

            Assert.That(nullable.ValueOrDefault(), Is.EqualTo(value));
        }

        #endregion
    }
}