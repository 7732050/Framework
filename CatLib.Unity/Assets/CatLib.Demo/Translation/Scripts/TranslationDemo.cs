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

using CatLib.API.Translation;
using System.Collections.Generic;

namespace CatLib.Demo.Translation
{
    /// <summary>
    /// 国际化demo
    /// </summary>
    public class TranslationDemo : IServiceProvider
    {
        /// <summary>
        /// 翻译字典
        /// </summary>
        private class TranslationDict : ITranslateResources
        {
            /// <summary>
            /// 测试用的字典
            /// </summary>
            private Dictionary<string, string> dict = new Dictionary<string, string>
            {
                { "hello" , "[1,10]hello|[11,*]world" },
                { "npc_message_1" , "this is one apple|there are :count apples" },
                { "npc_message_2" , "help!help!" },
                { "npc_message_3" , "My name is :name!" }
            };

            /// <summary>获取映射</summary>
            /// <param name="locale">语言</param>
            /// <param name="key">键</param>
            /// <param name="str">返回的值</param>
            /// <returns>是否成功获取</returns>
            public bool TryGetValue(string locale, string key, out string str)
            {
                return dict.TryGetValue(key, out str);
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            App.On(ApplicationEvents.OnStartCompleted, (payload) =>
            {
                var translator = App.Make<ITranslator>();
                translator.SetResources(new TranslationDict());

                translator.SetFallback(Languages.Chinese);
                translator.SetLocale(Languages.English);

                UnityEngine.Debug.Log(translator.Get("hello", 15));
                UnityEngine.Debug.Log(translator.Get("npc_message_1", 5));
                UnityEngine.Debug.Log(translator.Get("npc_message_2"));
                UnityEngine.Debug.Log(translator.Get("npc_message_3", "name", "catlib"));
            });
        }

        /// <summary>
        /// 注册服务提供者
        /// </summary>
        public void Register()
        {
        }
    }
}
