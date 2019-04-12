using UnityEngine;
using System.Collections;
using Core.Services;

namespace Core.MessageBus
{
    public class ClientMessage : Message
    {
        private static ObjectPool<ClientMessage> _pool = new ObjectPool<ClientMessage>(10);

        public int ClientConnectionId;

        public ClientMessage() : base()
        {
            ClientConnectionId = -1;
        }

        public static ClientMessage Get(string msgType, int connectionId, MessageData data = null)
        {
            var msg = _pool.Get();
            msg.ClientConnectionId = connectionId;
            msg.Type = msgType;
            msg.Data = data;
            return msg;
        }

        public override void FreePooledObject()
        {
            Data = null;
            Type = null;
            WaitingForSubscriber = false;
            GeneratedChannelMsgType = false;
            ClientConnectionId = - 1;

            _pool.Release(this);
        }
    }
}