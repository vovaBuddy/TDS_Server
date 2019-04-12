using Main.Looting.API;
using UnityEngine;

namespace Main.Looting.Data
{
    public class LootPlace : MonoBehaviour
    {
        public LootType LootType;
        public float SpawnProbability;
        public int[] Params;
        public int AmountFrom;
        public int AmountTo;
    }
}