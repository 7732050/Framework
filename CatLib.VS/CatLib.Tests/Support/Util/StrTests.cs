﻿/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using System;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.API.Stl
{
    [TestClass]
    public class StrTests
    {
        [TestMethod]
        public void TestRegexQuote()
        {
            var input = "(.*?) from table";
            input = Str.RegexQuote(input);

            Assert.AreEqual(@"\(\.\*\?\) from table", input);
        }

        [TestMethod]
        public void TestAsteriskWildcard()
        {
            var input = "path.?+/hello/wor*d";
            input = Str.AsteriskWildcard(input);

            Assert.AreEqual(@"path\.\?\+/hello/wor.*?d", input);
        }

        [TestMethod]
        public void TestStrIs()
        {
            Console.WriteLine(Str.AsteriskWildcard("path.?+/hello/w*d"));
            Assert.AreEqual(true, Str.Is("path.?+/hello/w*d", @"path.?+/hello/world"));
            Assert.AreEqual(false, Str.Is("path.?+/hello/w*d", @"hellopath.?+/hello/world"));
            Assert.AreEqual(true, Str.Is("path.?+/hello/w*d", @"path.?+/hello/worlddddddddd"));
            Assert.AreEqual(false, Str.Is("path.?+/hello/w*d", @"path.?+/hello/worldddddddddppppp"));
        }

        [TestMethod]
        public void TestSplit()
        {
            var result = Str.Split("helloworlds", 2);

            Assert.AreEqual("he" , result[0]);
            Assert.AreEqual("ll", result[1]);
            Assert.AreEqual("ow", result[2]);
            Assert.AreEqual("or", result[3]);
            Assert.AreEqual("ld", result[4]);
            Assert.AreEqual("s", result[5]);
            Assert.AreEqual(6, result.Length);
        }

        [TestMethod]
        public void TestSplit2()
        {
            var result = Str.Split("helloworld", 2);

            Assert.AreEqual("he", result[0]);
            Assert.AreEqual("ll", result[1]);
            Assert.AreEqual("ow", result[2]);
            Assert.AreEqual("or", result[3]);
            Assert.AreEqual("ld", result[4]);
            Assert.AreEqual(5, result.Length);
        }

        [TestMethod]
        public void TestSplitEmpty()
        {
            var result = Str.Split(string.Empty, 2);

            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void TestStrRepeat()
        {
            var result = Str.Repeat("abc", 2);
            Assert.AreEqual("abcabc", result);
        }

        [TestMethod]
        public void TestStrShuffle()
        {
            var str = "helloworld";
            var i = 0;
            while (Str.Shuffle(str) == "helloworld")
            {
                if (i++ > 1000)
                {
                    Assert.Fail();
                }
            }

            Assert.AreEqual("helloworld", str);
        }

        [TestMethod]
        public void TestStrShuffleEmpty()
        {
            var str = string.Empty;
            Assert.AreEqual(string.Empty, Str.Shuffle(str));
        }

        [TestMethod]
        public void TestSubstringCount()
        {
            var count = Str.SubstringCount("helloworldworld", "wor");
            Assert.AreEqual(2 , count);

            count = Str.SubstringCount("helloworldworld", "l");
            Assert.AreEqual(4, count);

            count = Str.SubstringCount("helloworldworld", "l", 5);
            Assert.AreEqual(2, count);

            count = Str.SubstringCount("helloworldworld", "l", 5, 5);
            Assert.AreEqual(1, count);

            count = Str.SubstringCount("heLLoworldworld", "l", 0, null, StringComparison.CurrentCulture);
            Assert.AreEqual(2, count);

            count = Str.SubstringCount("abcabcab", "abcab");
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void TestSubstringEmpty()
        {
            var count = Str.SubstringCount(string.Empty, "wor");
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void TestSubstringStartLargeThanLength()
        {
            var count = Str.SubstringCount("helloworld", "wor", 999, 9);
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void TestReverse()
        {
            var result = Str.Reverse("hello");
            Assert.AreEqual("olleh", result);
        }
    }
}
