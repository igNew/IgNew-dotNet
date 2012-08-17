﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanExtensionsTests.cs" company="IgNew LLC">
//   Copywright (c) 2012 IgNew LLC. All rights reserved.
// </copyright>
// <summary>
//   Unit tests for .
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace IgNew.Extensions.Tests
{
    #region Using Directives

    using NUnit.Framework;

    #endregion

    /// <summary>
    ///   Unit tests for <see cref="BooleanExtensions" /> .
    /// </summary>
    [TestFixture]
    public class BooleanExtensionsTests
    {
        #region Public Methods and Operators

        /// <summary>
        /// TODO The test_ to string_false.
        /// </summary>
        [Test]
        public void Test_ToString_false()
        {
            var trueString = "true";
            var falseString = "false";

            Assert.That(false.ToString(trueString, falseString), Is.EqualTo(falseString));
        }

        /// <summary>
        /// TODO The test_ to string_true.
        /// </summary>
        [Test]
        public void Test_ToString_true()
        {
            var trueString = "true";
            var falseString = "false";

            Assert.That(true.ToString(trueString, falseString), Is.EqualTo(trueString));
        }

        #endregion
    }
}