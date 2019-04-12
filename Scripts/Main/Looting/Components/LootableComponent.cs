using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using Main.Inventory.Data;
using Main.Looting.Data;
using UnityEngine;

namespace Main.Looting.Components
{
    public class LootableComponent : SubscriberBehaviour
    {
        private LootData _lootData;

        protected override void Awake()
        {
            base.Awake();
            
            _lootData = GetComponent<LootData>();
        }

        public void OnTriggerEnter(Collider other)
        {
            MessageBus.SendMessage(SubscribeType.Channel, other.GetComponent<Channel>().ChannelIds[SubscribeType.Channel],
                CommonMessage.Get(API.Messages.ADD_NEAR_OBJECT, ObjectData.GetObjectData(gameObject)));

        }

        public void OnTriggerExit(Collider other)
        {
            MessageBus.SendMessage(SubscribeType.Channel, other.GetComponent<Channel>().ChannelIds[SubscribeType.Channel],
                CommonMessage.Get(API.Messages.REMOVE_NEAR_OBJECT, ObjectData.GetObjectData(gameObject)));
        }       
        
        [Subscribe(SubscribeType.Network, Network.API.Messages.DELETE_OBJECT)]
        public void DeleteObject(Message msg)
        {
            Destroy(gameObject);
        }
    }
}