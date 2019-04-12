using System.Collections.Generic;
using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using Main.Looting.Components;
using UnityEngine;

namespace Main.Inventory.Data
{
    public class InventoryData : SubscriberBehaviour
    {
       
        private List<GameObject> NearObjects;

        protected override void Awake()
        {
            NearObjects = new List<GameObject>();
        }

        [Subscribe(SubscribeType.Channel, Looting.API.Messages.ADD_NEAR_OBJECT)]
        private void AddNearObject(Message msg)
        {
            Debug.Log("ADD NEAR!");

            var data = (ObjectData)msg.Data;
            NearObjects.Add(data.Value as GameObject);
        }

        [Subscribe(SubscribeType.Channel, Looting.API.Messages.REMOVE_NEAR_OBJECT)]
        private void RemoveNearObject(Message msg)
        {
            var data = (ObjectData)msg.Data;
            NearObjects.Remove(data.Value as GameObject);
        }

        [Subscribe(SubscribeType.Network, Network.API.Messages.TRY_PICK_UP_NEAR)]
        private void PickUp(Message msg)
        {
            Debug.Log("PICK UP!");

            NearObjects[0].GetComponent<PickUpComponent>().PickUp(Channel.ChannelIds[SubscribeType.Network]);
            NearObjects.RemoveAt(0);
        }
    }
}