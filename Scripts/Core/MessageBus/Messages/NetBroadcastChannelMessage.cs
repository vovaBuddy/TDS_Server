using UnityEngine;
using System.Collections;
using Main.Network;
using UnityEngine.Networking;
using Core.Services;

namespace Core.MessageBus
{
    public class NetBroadcastChannelMessage : Message, ISendMessage
    {
        public int NetId;
        public byte NetType;
        public QosType QoS;
        public int ExceptId;

        public void SendMessage()
        {
            NetMessage netMsg;
            netMsg.Type = NetType;
            netMsg.Data = Data;
            netMsg.NetId = NetId;
            netMsg.Broadcast = false;

            Server.Instance.SendMessage(netMsg, QoS, -1, ExceptId);
        }

        private static ObjectPool<NetBroadcastChannelMessage> _pool
            = new ObjectPool<NetBroadcastChannelMessage>(10);

        public NetBroadcastChannelMessage() : base()
        {
            NetId = -1;
            NetType = 0;
            ExceptId = -1;
            QoS = QosType.AllCostDelivery;
        }

        public static NetBroadcastChannelMessage Get(byte msgType, QosType qos, int netId, int exceptId = -1, MessageData data = null)
        {
            var msg = _pool.Get();
            msg.Type = Channel.GetFullSubscribeType(SubscribeType.Network, netId, msgType.ToString());
            msg.NetType = msgType;
            msg.NetId = netId;
            msg.QoS = qos;
            msg.Data = data;
            msg.ExceptId = exceptId;
            return msg;
        }

        public override void FreePooledObject()
        {
            NetId = -1;
            NetType = 0;
            Data = null;
            Type = null;
            ExceptId = -1;
            QoS = QosType.AllCostDelivery;

            _pool.Release(this);
        }
    }
}