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

using CatLib.API.Debugger;
using CatLib.Debugger.WebMonitor.Handler;
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
            monitor.Monitor(new SizeMonitorHandler("monitor.memory.total", new[] { "tags.common" },
                () => Profiler.GetTotalAllocatedMemoryLong()));
        }
    }
}
