using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using Core.Services;
using Main.Network;
using UnityEngine;
using UnityEngine.Networking;

namespace Main.Character
{
    public class CharacterFactory : SubscriberBehaviour
    {
        [Subscribe(SubscribeType.Broadcast, Network.API.Messages.CREATE_CHARACTER)]
        private void CreateCharacter(Message msg)
        {
            var netId = ((IntData) msg.Data).Value;
            var channelId = Channel.GetChannelId();
            
            ServiceLocator.GetService<ResourceLoaderService>()
                .InstantiatePrefabByPathName("Character/CharacterReplicant",
                    go => {
                        var sub = go.GetComponent<SubscriberBehaviour>();
                        sub.Channel.ChannelIds.Add(SubscribeType.Network, netId);
                        sub.Channel.ChannelIds.Add(SubscribeType.Channel, channelId);
                        Channel.ChannelIdByNetId.Add(netId, channelId);
                        sub.ReSubscribe();
                    });
        }
    }
}