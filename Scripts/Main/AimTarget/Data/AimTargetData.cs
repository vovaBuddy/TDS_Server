using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using UnityEngine;

namespace Main.AimTarget.Data
{
    public class AimTargetData : SubscriberBehaviour
    {
        public Transform AimTargetObject;
        public Transform ReduceSpreadTarget;
        public Transform ResultTargetObject;
        public Transform StickyAimObject;

        public LayerMask ExcludeAimLayers;
        public LayerMask TargetsAimLayers;

        public Transform ShootPosition;

        [HideInInspector]
        public int CharacterNetId;

        [Subscribe(SubscribeType.Channel, API.Messages.UPDATE_SHOOT_POSITION_TRANSFORM)]
        private void UpdateShootPosition(Message msg)
        {
            ShootPosition = ObjectData.ParseObjectData<Transform>(msg.Data);
        }
    }
}