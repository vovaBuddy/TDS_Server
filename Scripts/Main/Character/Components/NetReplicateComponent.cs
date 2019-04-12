using UnityEngine;
using System.Collections;
using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using UnityEngine.Networking;

namespace Main.Character
{
    public class NetReplicateComponent : SubscriberBehaviour
    {
        [Subscribe(Network.API.Messages.NEW_CLIENT)]
        private void NewClient(Message msg)
        {
            var clientNetId = ((IntData)msg.Data).Value;
            var replicantNetId = Channel.ChannelIds[SubscribeType.Network];

            if (clientNetId == replicantNetId) return;

            MessageBus.SendMessage(NetAddressedMessage.Get(Network.API.Messages.CREATE_CHARACTER_REPLICANT, QosType.Reliable, clientNetId,
                IntData.GetIntData(replicantNetId)));
        }

        private void Start()
        {
            var netId = Channel.ChannelIds[SubscribeType.Network];

            MessageBus.SendMessage(NetBroadcastMessage.Get(Network.API.Messages.CREATE_CHARACTER_REPLICANT, QosType.Reliable, netId,
                IntData.GetIntData(netId)));
        }
    }
}