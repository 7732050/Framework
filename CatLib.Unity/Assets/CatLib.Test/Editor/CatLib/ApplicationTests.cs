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
using CatLib.Config;
using CatLib.Converters;
using CatLib.Events;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif


namespace CatLib.Tests
{
    [TestClass]
    public class ApplicationTests
    {
        [TestMethod]
        public void RepeatInitTest()
        {
            var app = MakeApplication();

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                app.Init();
            });
        }

        /// <summary>
        /// 未经引导的初始化
        /// </summary>
        [TestMethod]
        public void NoBootstrapInit()
        {
            var app = new Application();

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                app.Init();
            });
        }

        /// <summary>
        /// 获取版本号测试
        /// </summary>
        [TestMethod]
        public void GetVersionTest()
        {
            var app = MakeApplication();
            Assert.AreNotEqual(string.Empty, app.Version);
        }

        /// <summary>
        /// 获取当前启动流程
        /// </summary>
        [TestMethod]
        public void GetCurrentProcess()
        {
            var app = MakeApplication();
            Assert.AreEqual(Application.StartProcess.Inited, app.Process);
        }

        /// <summary>
        /// 重复的引导测试
        /// </summary>
        [TestMethod]
        public void RepeatBootstrap()
        {
            var app = new Application();
            app.Bootstrap();
            app.Init();
            app.Bootstrap();
            Assert.AreEqual(Application.StartProcess.Inited, app.Process);
        }

        /// <summary>
        /// 注册非法类型测试
        /// </summary>
        [TestMethod]
        public void RegisteredIllegalType()
        {
            var app = new Application();
            app.Bootstrap();
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                app.Register(null);
            });
        }

        /// <summary>
        /// 重复的注册
        /// </summary>
        [TestMethod]
        public void RepeatRegister()
        {
            var app = MakeApplication();

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                app.Register(new ConfigProvider());
            });
        }

        /// <summary>
        /// 获取唯一标识符
        /// </summary>
        [TestMethod]
        public void GetGuid()
        {
            var app = MakeApplication();
            Assert.AreNotEqual(app.GetGuid(), app.GetGuid());
        }

        private static bool prioritiesTest;

        private class ProviderTest1 : ServiceProvider
        {
            [Priority(10)]
            public override void Init()
            {
                prioritiesTest = true;
            }

            public override void Register()
            {

            }
        }

        [Priority(5)]
        private class ProviderTest2 : ServiceProvider
        {
            public override void Init()
            {
                prioritiesTest = false;
            }

            public override void Register()
            {

            }
        }

        /// <summary>
        /// 优先级测试
        /// </summary>
        [TestMethod]
        public void ProvidersPrioritiesTest()
        {
            var app = new Application();
            app.OnFindType((t) =>
            {
                return Type.GetType(t);
            });
            app.Bootstrap();
            App.Instance.Register(new ProviderTest1());
            App.Instance.Register(new ProviderTest2());
            app.Init();
            Assert.AreEqual(true, prioritiesTest);
        }

        /// <summary>
        /// 无效的引导
        /// </summary>
        [TestMethod]
        public void IllegalBootstrap()
        {
            var app = new Application();
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                app.Bootstrap(null).Init();
            });
        }

        /// <summary>
        /// 初始化后再注册
        /// </summary>
        [TestMethod]
        public void InitedAfterRegister()
        {
            prioritiesTest = true;
            var app = new Application();
            app.OnFindType((t) =>
            {
                return Type.GetType(t);
            });
            app.Bootstrap();
            App.Instance.Register(new ProviderTest1());
            app.Init();

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                App.Instance.Register(new ProviderTest2());
            });
        }

        [TestMethod]
        public void TestRepeatRegister()
        {
            var app = new Application();
            app.OnFindType((t) =>
            {
                return Type.GetType(t);
            });

            app.Bootstrap();
            app.Register(new ProviderTest1());

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                app.Register(new ProviderTest1());
            });
        }

        [TestMethod]
        public void TestOnDispatcher()
        {
            var app = MakeApplication();

            app.On("testevent", (payload) =>
            {
                Assert.AreEqual("abc", payload);
                return 123;
            });

            var result = app.Trigger("testevent", "abc", true);
            Assert.AreEqual(123, result);
        }

        [TestMethod]
        public void TestNoDispatcher()
        {
            var app = new Application();
            app.OnFindType((t) =>
            {
                return Type.GetType(t);
            });
            app.Bootstrap().Init();

            var onHandler = app.On("testevent", (payload) =>
            {
                Assert.AreEqual("abc", payload);
                return 123;
            });

            Assert.AreEqual(null, onHandler);

            var result = app.Trigger("testevent", "abc", true);
            Assert.AreEqual(null, result);
            var result2 = app.Trigger("testevent", "abc") as object[];
            Assert.AreEqual(0 , result2.Length);
        }

        [TestMethod]
        public void TestIsMainThread()
        {
            var app = MakeApplication();
            Assert.AreEqual(true, app.IsMainThread);
        }

        private Application MakeApplication()
        {
            var app = new Application();
            app.OnFindType((t) =>
            {
                return Type.GetType(t);
            });
            app.Bootstrap(new BootstrapClass()).Init();
            return app;
        }

        private class BootstrapClass : IBootstrap
        {
            public void Bootstrap()
            {
                App.Instance.Register(new ConfigProvider());
                App.Instance.Register(new ConvertersProvider());
                App.Instance.Register(new EventsProvider());
            }
        }
    }
}
