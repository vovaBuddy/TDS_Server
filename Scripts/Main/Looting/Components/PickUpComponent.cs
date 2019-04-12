using Core.MessageBus;
using Main.Looting.Data;
using UnityEngine;
using Core.MessageBus.MessageDataTemplates;

namespace Main.Looting.Components
{
    public class PickUpComponent : SubscriberBehaviour
    {
        private LootData _lootData;

        protected override void Awake()
        {
            _lootData = GetComponent<LootData>();
        }

        public void PickUp(int networkId)
        {
            MessageBus.SendMessage(NetAddressedChannelMessage.Get(
                Network.API.Messages.PICK_UP_LOOT_ITEM, UnityEngine.Networking.QosType.ReliableSequenced, networkId,
                LootItemData.GetData(_lootData.LootType, _lootData.Amount, _lootData.Params)));

            MessageBus.SendMessage(NetBroadcastChannelMessage.Get(
                Network.API.Messages.DELETE_OBJECT, UnityEngine.Networking.QosType.ReliableSequenced, Channel.ChannelIds[SubscribeType.Network]));

            Destroy(gameObject);
        }
    }
}