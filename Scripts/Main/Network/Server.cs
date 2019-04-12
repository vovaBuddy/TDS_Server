using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Transactions;
using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using Main.Network.API;
using UnityEngine;
using UnityEngine.Networking;

namespace Main.Network
{
    public class Server : SubscriberBehaviour
    {
        private Dictionary<int, int> ConnectionIdByNetId;
        private Dictionary<int, int> NetIdByConnectionId;

        private List<int> connections;
        
        private static int _curNetId = 0;
        public static int GetNetId(int connectionId)
        {
            _curNetId++;
            Instance.ConnectionIdByNetId.Add(_curNetId, connectionId);
            return _curNetId;
        }

        public static int GetNetId()
        {
            return ++_curNetId;
        }

        private const int MAX_USER = 100;
        private const int PORT = 26000;
        //private const string SERVER_IP = "159.69.45.209";
        private const string SERVER_IP = "192.168.0.3";
        //private const string SERVER_IP = "127.0.0.1";
        
        private Dictionary<QosType, byte> _QoSChannels = new Dictionary<QosType, byte>();
		
        private int _hostId;
        private byte _error;

        private int BUFFER_LENGTH = 1024;

        private bool _isStarted;

        public static Server Instance;
        
        private void ReadyForCharacter(int connectionId)
        {
            var netId = GetNetId(connectionId);
            MessageBus.SendMessage(NetAddressedMessage.Get(Messages.CREATE_CHARACTER, QosType.Reliable, netId,
                IntData.GetIntData(netId)));

            var aimTargetnetId = GetNetId(connectionId);            
            MessageBus.SendMessage(NetAddressedMessage.Get(Messages.CREATE_AIM_TARGET, QosType.Reliable, aimTargetnetId, 
                AimTarget.API.InstanceData.GetData("UnarmedAimTarget", aimTargetnetId, netId)));

            MessageBus.SendMessage(CommonMessage.Get(Messages.NEW_CLIENT, IntData.GetIntData(netId)));
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            Instance = this;
            
            ConnectionIdByNetId = new Dictionary<int, int>();
            NetIdByConnectionId = new Dictionary<int, int>();
            connections = new List<int>();


            NetworkTransport.Init();
			
            var config = new ConnectionConfig();
            _QoSChannels.Add(QosType.Unreliable, config.AddChannel(QosType.Unreliable));
            _QoSChannels.Add(QosType.Reliable, config.AddChannel(QosType.Reliable));
            _QoSChannels.Add(QosType.ReliableSequenced, config.AddChannel(QosType.ReliableSequenced));

            var topology = new HostTopology(config, MAX_USER);

            _hostId = NetworkTransport.AddHost(topology, PORT, SERVER_IP);

            _isStarted = true;
        }

        private void SendMessage(NetMessage netMsg, int connection)
        {
            var buffer = new byte[BUFFER_LENGTH];
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream(buffer);

            formatter.Serialize(stream, netMsg);

            NetworkTransport.Send(_hostId, connection, _QoSChannels[QosType.Reliable],
                buffer, BUFFER_LENGTH, out _error);
        }
        
        public void SendMessage(NetMessage netMsg, QosType qos, int netIdAddress, int exceptIdAddress = -1)
        {
            Debug.Log("Send Message: " + netMsg.Type);
            
            var buffer = new byte[BUFFER_LENGTH];
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream(buffer);

            formatter.Serialize(stream, netMsg);

            if (netIdAddress == -1)
            {
                foreach (var connectionId in connections)
                {
                    if ((exceptIdAddress != -1 && connectionId != ConnectionIdByNetId[exceptIdAddress]) ||
                        (exceptIdAddress == -1))
                    {
                        NetworkTransport.Send(_hostId, connectionId, _QoSChannels[qos],
                            buffer, BUFFER_LENGTH, out _error);
                    } 
                }
            }
            else
            {
                NetworkTransport.Send(_hostId, ConnectionIdByNetId[netIdAddress], _QoSChannels[qos],
                    buffer, BUFFER_LENGTH, out _error);
            }
        }

        private void Update()
        {
            ReceiveMessage();
        }

        private void ReceiveMessage()
        {
            int recHostId; 
            int connectionId; 
            int channelId; 
            byte[] recBuffer = new byte[BUFFER_LENGTH]; 
            int dataSize;
            byte error;
			
            while(true)
            {
                NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId,
                    recBuffer, BUFFER_LENGTH, out dataSize, out error);
                switch (recData)
                {
                    case NetworkEventType.ConnectEvent:
                        connections.Add(connectionId);
                        Debug.Log(string.Format("Client connected hostId:{0}, connectionId:{1}", recHostId,
                            connectionId));
                        break;

                    case NetworkEventType.DataEvent:
                        var formatter = new BinaryFormatter();
                        var stream = new MemoryStream(recBuffer);

                        var netMsg = (NetMessage)formatter.Deserialize(stream);

                        if(netMsg.Type == Messages.READY_FOR_CHARACTER)
                        {
                            ReadyForCharacter(connectionId);
                            return;
                        }

                        if(netMsg.Broadcast)
                        {
                            foreach(var connection in connections)
                            {
                                if (connection != connectionId)
                                {
                                    Debug.Log("Send Message broadcast");

                                    SendMessage(netMsg, connection);
                                }
                            }
                        }

                        MessageBus.SendMessage(ClientMessage.Get(
                            (netMsg.NetId == -1 ? netMsg.Type.ToString() :
                                Channel.GetFullSubscribeType(
                                    SubscribeType.Network, netMsg.NetId, netMsg.Type.ToString())), 
                            connectionId,
                            (MessageData)netMsg.Data));

                        stream.Close();
                        
                        break;
                    case NetworkEventType.DisconnectEvent: break;

                    case NetworkEventType.BroadcastEvent: break;
                    default:
                    case NetworkEventType.Nothing: 
                        return;
                }
            }
        }
    }
}