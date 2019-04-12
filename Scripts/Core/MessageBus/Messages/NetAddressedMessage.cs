using UnityEngine;
using System.Collections;
using Main.Network;
using UnityEngine.Networking;
using Core.Services;

namespace Core.MessageBus
{
    public class NetAddressedMessage : Message, ISendMessage
    {
        public int NetId;
        public byte NetType;
        public QosType QoS;

        public void SendMessage()
        {
            NetMessage netMsg;
            netMsg.Type = NetType;
            netMsg.Data = Data;
            netMsg.NetId = -1;
            netMsg.Broadcast = false;

            Server.Instance.SendMessage(netMsg, QoS, NetId);
        }

        private static ObjectPool<NetAddressedMessage> _pool
            = new ObjectPool<NetAddressedMessage>(10);

        public NetAddressedMessage() : base()
        {
            NetId = -1;
            NetType = 0;
            QoS = QosType.AllCostDelivery;
        }

        public static NetAddressedMessage Get(byte msgType, QosType qos, int netId, MessageData data = null)
        {
            var msg = _pool.Get();
            msg.Type = msgType.ToString();
            msg.NetType = msgType;
            msg.NetId = netId;
            msg.QoS = qos;
            msg.Data = data;
            return msg;
        }

        public override void FreePooledObject()
        {
            NetId = -1;
            NetType = 0;
            Data = null;
            Type = null;
            QoS = QosType.AllCostDelivery;

            _pool.Release(this);
        }
    }
}