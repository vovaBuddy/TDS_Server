using Main.Looting.Data;
using UnityEngine;

namespace Main.Looting.Components
{
    public class CreateReplicantComponent : MonoBehaviour
    {
        private LootData _lootData;

        private void Awake()
        {
            _lootData = GetComponent<LootData>();
        }

        private void Start()
        {
            //var netMsg = new ReplicateLoot(_lootData.LootId, transform.position);
            //Server.Instance.SendMessage(netMsg);
        }
    }
}