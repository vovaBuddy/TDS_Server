using System;
using Core.MessageBus;
using Main.Bullets.API;
using Main.Bullets.Data;
using Main.Characters.Data;
using UnityEngine;

namespace Main.Characters.Components
{
    public class DamagedCollider : MonoBehaviour, IOnCollisionComponent
    {
        private DamagedComponent _damagedComponent;
        private CharacterData _ownerCharacterData;
        
        public float DamageFactor;

        private void Awake()
        {
            _damagedComponent = GetComponentInParent<DamagedComponent>();
            _ownerCharacterData = _damagedComponent.GetComponent<CharacterData>();
        }

        public bool IsProjectileOwner(int id)
        {
            return _damagedComponent.BulletIds.IndexOf(id) != -1;
        }

        public void OnCollisionAction(ProjectileData projectile, Collision collision)
        {
            var damage = 2 * DamageFactor;
            _ownerCharacterData.HitPoints -= (int)damage;
            
            //MessageBus.SendMessage(Message.GetMessage(Messages.PROJECTILE_COLLISION, 
            //    ProjectileCollisionData.GetProjectileCollisionData(projectile.BulletId, 
            //        collision.contacts[0].point, collision.contacts[0].normal)));

            Destroy(projectile.gameObject);
        }
    }
}