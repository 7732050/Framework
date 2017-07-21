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

using CatLib.Debugger.WebMonitor;
using UnityEngine.Profiling;

namespace CatLib.Debugger.WebMonitorContent
{
    /// <summary>
    /// 总分配的内存大小
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class TotalAllocatedMemoryMonitor
    {
        /// <summary>
        /// 构建一个总内存监控
        /// </summary>
        /// <param name="monitor">监控</param>
        public TotalAllocatedMemoryMonitor([Inject(Required = true)]IMonitor monitor)
        {
            monitor.DefinedMoitor("memory.total",
                MonitorHelper.CallbackSize("monitor.memory.total", () => Profiler.GetTotalAllocatedMemoryLong())
                , 30);
        }
    }
}
