using UnityEngine;
using System.Collections;
using Core.MessageBus;
using Core.Services;

namespace Main.AimTarget
{
    public class AimTargetFactory : SubscriberBehaviour
    {
        [Subscribe(SubscribeType.Broadcast, Network.API.Messages.CREATE_AIM_TARGET)]
        private void InstantiateAimTarget(Message msg)
        {
            var aimTargetData = (msg.Data as API.InstanceData);

            ServiceLocator.GetService<ResourceLoaderService>()
                .InstantiatePrefabByPathName("AimTarget/" + aimTargetData.Type,
                    go => {
                        go.GetComponent<Data.AimTargetData>().CharacterNetId = aimTargetData.CoupledObjectNetId;

                        var sub = go.GetComponent<SubscriberBehaviour>();
                        sub.Channel.ChannelIds.Add(SubscribeType.Network, aimTargetData.NetIds[0]);
                        if(aimTargetData.CoupledObjectNetId != -1)
                            sub.Channel.ChannelIds.Add(SubscribeType.Channel, 
                                Channel.ChannelIdByNetId[aimTargetData.CoupledObjectNetId]);
                        sub.ReSubscribe();
                    });
        }
    }
}