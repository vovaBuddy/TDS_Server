using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using UnityEngine;
using Main.AimTarget.Data;

namespace Main.AimTarget.Components
{
    public class CursorFollowComponent : SubscriberBehaviour
    {
        private AimTargetData _aimTargetData;
                
        protected override void Awake()
        {
            base.Awake();

            _aimTargetData = gameObject.GetComponent<AimTargetData>();
        }

        public void Update()
        {
            MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel],
                CommonMessage.Get(API.Messages.UPDATE_AIM_TARGET_POSITION,
                    Vector3Data.GetVector3Data(_aimTargetData.AimTargetObject.position)));
        }
    }
}