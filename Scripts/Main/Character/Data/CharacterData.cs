using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using Main.Characters.API;
using Main.Characters.Configs;
using UnityEngine;
using UnityEngine.Networking;

namespace Main.Characters.Data
{
    public class CharacterData : SubscriberBehaviour, ITransformData
    {
        [HideInInspector] public SpeedMode CurrentSpeedMode { get; set; }

        [SerializeField] private MovementConfig _movementConfig;
        public MovementConfig MovementConfig { get { return _movementConfig; } }

        public Transform RightArm;
        
        public int _hitPoints;

        public int HitPoints
        {
            set
            {
                _hitPoints =  value;
                _hitPoints = Mathf.Min(100, _hitPoints);
                _hitPoints = Mathf.Max(0, _hitPoints);
                                                
                MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel],
                    CommonMessage.Get(Messages.UPDATE_HIT_POINTS, IntData.GetIntData(_hitPoints)));

                if (_hitPoints <= 0)
                {
                    //ZeroHpMsg
                }
            }
            get { return _hitPoints; }
        }

        [Subscribe(SubscribeType.Network, Network.API.Messages.CREATE_WEAPON)]
        private void NeedArmPosition(Message msg)
        {
            var data = (Weapons.API.WeaponInstantiateData)(msg.Data).GetCopy();
            data.Parent = RightArm;
            data.ChannelId = Channel.ChannelIds[SubscribeType.Channel];
            MessageBus.SendMessage(CommonMessage.Get(Weapons.API.Messages.INSTANTIATE, data));

            int connectionId = ((ClientMessage)msg).ClientConnectionId;
            int netId = Network.Server.GetNetId(connectionId);

            if (!data.CreateAimTarget) return;

            MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel],
                CommonMessage.Get(AimTarget.API.Messages.DESTROY));

            MessageBus.SendMessage(NetAddressedMessage.Get(Network.API.Messages.CREATE_AIM_TARGET, QosType.Reliable, Channel.ChannelIds[SubscribeType.Network],
                AimTarget.API.InstanceData.GetData("ARAimTarget", netId, Channel.ChannelIds[SubscribeType.Network])));
        }

        override protected void Awake()
        {
            _hitPoints = 100;
            CurrentSpeedMode = SpeedMode.RUN;
        }
    }
}