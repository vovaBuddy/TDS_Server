using UnityEngine;
using System.Collections;
using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using UnityEngine.Networking;
using Main.Network;

namespace Main.AimTarget
{
    public class NetReplicateComponent : SubscriberBehaviour
    {
        [Subscribe(Network.API.Messages.NEW_CLIENT)]
        private void NewClient(Message msg)
        {
            var clientNetId = ((IntData)msg.Data).Value;
            var characterNetId = GetComponent<Data.AimTargetData>().CharacterNetId;
            var replicantNetId = Channel.ChannelIds[SubscribeType.Network];

            if (characterNetId == clientNetId) return;

            MessageBus.SendMessage(NetAddressedMessage.Get(Network.API.Messages.CREATE_AIM_TARGET_REPLICANT, QosType.Reliable, clientNetId,
                API.InstanceData.GetData("Replicant", replicantNetId, GetComponent<Data.AimTargetData>().CharacterNetId)));
        }

        private void Start()
        {
            var netId = Channel.ChannelIds[SubscribeType.Network];

            MessageBus.SendMessage(NetBroadcastMessage.Get(Network.API.Messages.CREATE_AIM_TARGET_REPLICANT, QosType.Reliable, netId,
                API.InstanceData.GetData("Replicant", netId, GetComponent<Data.AimTargetData>().CharacterNetId)));
        }
    }
}