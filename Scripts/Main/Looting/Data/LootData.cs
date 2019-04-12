using Main.Looting.API;
using System;
using UnityEngine;

namespace Main.Looting.Data
{   
    [Serializable]
    public class LootData : MonoBehaviour
    {
        public int OwnerId;
        public LootType LootType;
        public int Amount;
        public int[] Params;

        public LootData(LootType lootType, int amount, int[] param)
        {
            LootType = lootType;
            Amount = amount;
            Params = param;
        }
    }
}