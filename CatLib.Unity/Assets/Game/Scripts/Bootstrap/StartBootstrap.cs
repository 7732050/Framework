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

using CatLib.Facade;

namespace CatLib.Bootstrap
{
    /// <summary>
    /// 启动到用户代码
    /// </summary>
    public class StartBootstrap : IBootstrap
    {
        /// <summary>
        /// 引导程序
        /// </summary>
        public void Bootstrap()
        {
            App.On(ApplicationEvents.OnStartCompleted, (payload) =>
            {
                if (Router.Instance != null)
                {
                    Router.Instance.Dispatch("bootstrap://config");
                    Router.Instance.Dispatch("bootstrap://start");
                }
            });
        }
    }
}