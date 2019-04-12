using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using Main.Inventory.Data;
using Main.Weapons.API;
using UnityEngine;

namespace Main.Inventory.Components
{
    public class ControlComponent : SubscriberBehaviour
    {
        private InventoryData _inventoryData;

        protected override void Awake()
        {
            base.Awake();
            _inventoryData = gameObject.GetComponent<InventoryData>();
        }

        [Subscribe(SubscribeType.Channel, API.Messages.CHECK_UPDATE)]
        private void Ping(Message msg)
        {
            MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel],
                CommonMessage.Get(API.Messages.UPDATE,
                    ObjectData.GetObjectData(_inventoryData)));
        }

        [Subscribe(SubscribeType.Network, Network.API.Messages.TRY_PICK_UP_NEAR)]
        private void PickUpNear(Message msg)
        {

        }
        
        [Subscribe(SubscribeType.Channel, API.Messages.PICK_UP_Weapons)]
        private void PickUpWeapon(Message msg)
        {
            //var data = ObjectData.ParseObjectData<PickUpLootData>(msg.Data);
            
            //MessageBus.SendMessage( Message.GetMessage(
            //    Weapons.API.Messages.INSTANTIATE, 
            //    WeaponInstantiateData.GetWeaponInstantiateData((Weapons)data.Params[0], 
            //        //ToDo: remove transform nesting dependence
            //        Characters.CharacterFactory.PlayableCharacters[data.MessageChannelId].
            //            GetComponent<Characters.Data.CharacterData>().RightArm, 
            //        data.MessageChannelId, 
            //        Characters.CharacterFactory.PlayableCharacters[data.MessageChannelId].
            //            GetComponent<Characters.Components.ReplicantComponent>() == null)));
        }
        
        [Subscribe(SubscribeType.Channel, API.Messages.PICK_UP_BULLETS, 
            API.Messages.PICK_UP_EQUIPMENTS)]
        private void PickUpBullets(Message msg)
        {
            //var data = ObjectData.ParseObjectData<PickUpLootData>(msg.Data);
            
            ////ToDo: Support all param values
            //var key = data.LootType.ToString("d") + data.Params[0].ToString("d");
            //if (_inventoryData.InventoryItemsAmount.ContainsKey(key))
            //{
            //    _inventoryData.InventoryItemsAmount[key] += data.Amount;
            //}
            //else
            //{
            //    _inventoryData.InventoryItemsAmount.Add(key, data.Amount);
            //}

            //var msgType = Inventory.API.Messages.UPDATE_PREFFIX +
            //              data.LootType.ToString() + "_" + data.Params[0].ToString("d");
            
            //MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel], Message.GetMessage(
            //    msgType, IntData.GetIntData(data.Amount)));
        }
    }
}