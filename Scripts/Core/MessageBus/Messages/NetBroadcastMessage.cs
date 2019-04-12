using UnityEngine;
using System.Collections;
using Main.Network;
using UnityEngine.Networking;
using Core.Services;

namespace Core.MessageBus
{
    public class NetBroadcastMessage : Message, ISendMessage
    {
        public byte NetType;
        public QosType QoS;
        public int ExceptId;

        public void SendMessage()
        {
            NetMessage netMsg;
            netMsg.Type = NetType;
            netMsg.Data = Data;
            netMsg.NetId = -1;
            netMsg.Broadcast = false;

            Server.Instance.SendMessage(netMsg, QoS, -1, ExceptId);
        }

        private static ObjectPool<NetBroadcastMessage> _pool
            = new ObjectPool<NetBroadcastMessage>(10);

        public NetBroadcastMessage() : base()
        {
            NetType = 0;
            ExceptId = -1;
            QoS = QosType.AllCostDelivery;
        }

        public static NetBroadcastMessage Get(byte msgType, QosType qos, int exceptId = -1, MessageData data = null)
        {
            var msg = _pool.Get();
            msg.Type = msgType.ToString();
            msg.NetType = msgType;
            msg.QoS = qos;
            msg.Data = data;
            msg.ExceptId = exceptId;
            return msg;
        }

        public override void FreePooledObject()
        {
            NetType = 0;
            Data = null;
            Type = null;
            QoS = QosType.AllCostDelivery;
            ExceptId = -1;

            _pool.Release(this);
        }
    }
}