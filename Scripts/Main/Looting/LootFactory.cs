using System.Collections.Generic;
using Core.MessageBus;
using Core.Services;
using Main.Looting.Data;
using Main.Network;
using Services;
using UnityEngine;

namespace Main.Looting
{
    public class LootFactory : SubscriberBehaviour
    {
        [SerializeField] private GameObject _lootPlacesContainer;

        private float _spawnTimer = 0;
        [SerializeField] private float SpawnTime = 30;

        public static LootFactory Instance;
        
        protected override void Awake()
        {
            base.Awake();

            Instance = this;
        }

        public void Update()
        {
            _spawnTimer += Time.deltaTime;

            if (_spawnTimer >= SpawnTime)
            {
                _spawnTimer = 0.0f;

                Spawn();
            }
        }

        [Subscribe(SubscribeType.Broadcast, Network.API.Messages.DROP_LOOT_ITEM)]
        private void Drop(Message msg)
        {
            InstanceLootItemData data = msg.Data as InstanceLootItemData;

            SpawnItem(_lootPlacesContainer.transform, data.GetVector3(), data.LootType, data.Amout, data.Params);
        }

        private void SpawnItem(Transform parent, Vector3 pos, Looting.API.LootType type, int amount, int[] prms)
        {
            ServiceLocator.GetService<ResourceLoader>().InstantiatePrefabByPathName("Loot/LootItem", go =>
            {
                go.transform.SetParent(parent);
                go.transform.position = pos;

                var curLootData = go.GetComponent<LootData>();
                curLootData.LootType = type;

                curLootData.Amount = amount;

                curLootData.Params = prms;

                var netId = Server.GetNetId();
                var sub = go.GetComponent<SubscriberBehaviour>();
                sub.Channel.ChannelIds.Add(SubscribeType.Network, netId);
                sub.ReSubscribe();

                MessageBus.SendMessage(NetBroadcastMessage.Get(Network.API.Messages.CREATE_LOOT_ITEM, UnityEngine.Networking.QosType.Reliable, -1,
                    InstanceLootItemData.GetData(pos, curLootData.LootType,
                        curLootData.Amount, curLootData.Params, netId)));
            });
        }

        private void Spawn()
        {
            foreach (var lootPlace in _lootPlacesContainer.GetComponentsInChildren<LootPlace>())
            {
                if (Random.Range(0.0f, 100.0f) > lootPlace.SpawnProbability) continue;

                if (lootPlace.GetComponentInChildren<LootData>() != null) continue;

                SpawnItem(lootPlace.transform, lootPlace.transform.position, lootPlace.LootType, 
                    Random.Range(lootPlace.AmountFrom, lootPlace.AmountTo + 1), lootPlace.Params);
            }
        }
    }
}