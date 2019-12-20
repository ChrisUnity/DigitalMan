using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFramework;
using Holoview;
using Holoview.Chat;
namespace Holoview.Chat
{
    public class ChatEventArg : GlobalEventArgs
    {
        public ChatEvent ChatEvent;
        public object Param;
        public object Param1;

        public override int Id
        {
            get
            {
                return 1;
            }
        }

        public override void Clear()
        {
            ChatEvent = ChatEvent.InValid;
        }


        /// <summary>
        /// 事件填充
        /// </summary>
        public ChatEventArg ChangeState(ChatEvent chatEvent, object param = null, object param1 = null)
        {
            ChatEvent = chatEvent;
            Param = param;
            Param1 = param1;
            return this;
        }
    }
}

