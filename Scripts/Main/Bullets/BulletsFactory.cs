using System.Collections.Generic;
using Core.MessageBus;
using Core.Services;
using Main.Bullets.API;
using Main.Bullets.Data;
using UnityEngine;

namespace Main.Bullets
{
    public class BulletsFactory : SubscriberBehaviour
    {
        public static BulletsFactory Instance;
        public Dictionary<int, GameObject> Bullets;

        private static int _bulletId = -1;
        public static int GetNextBulletId()
        {
            return ++_bulletId;
        }

        protected override void Awake()
        {
            base.Awake();

            Instance = this;
            Bullets = new Dictionary<int, GameObject>();
        }

        [Subscribe(API.Messages.PROJECTILE_COLLISION)]
        private void Collision(Message msg)
        {
            var data = (ProjectileCollisionData)msg.Data;
            var projectile = Bullets[data.BulletId];

            ServiceLocator.GetService<ResourceLoaderService>().InstantiatePrefabByPathName("VFX/Bullets/SmallRedImpact",
                (go =>
                {
                    go.transform.position = data.Position;
                    go.transform.rotation = Quaternion.LookRotation(data.Normal);
                }));

            Bullets.Remove(data.BulletId);
            Destroy(projectile);
        }

        [Subscribe(SubscribeType.Broadcast, Network.API.Messages.CREATE_PROJECTILE,
            Network.API.Messages.CREATE_PROJECTILE_REPLICANT)]
        private void CreateProjectile(Message msg)
        {
            var projectileData = (msg.Data as API.ProjectileMessageData);

            var postfix = msg.Type == Network.API.Messages.CREATE_PROJECTILE_REPLICANT.ToString() ? "_Replicant" : string.Empty;

            Debug.Log("CreateProjectile " + projectileData.type.ToString() + postfix);

            ServiceLocator.GetService<ResourceLoaderService>()
                .InstantiatePrefabByPathName("Projectiles/Bullets/" + projectileData.type.ToString() + postfix,
                    (go =>
                    {
                        Bullets.Add(projectileData.BulletNetId, go);
                        go.transform.position = projectileData.GetPostition();
                        go.transform.rotation = projectileData.GetQuaternion();
                        go.GetComponent<ProjectileData>().DamageFactor = projectileData.damageFactor;
                        go.GetComponent<Rigidbody>().velocity = go.transform.forward * projectileData.startSpeed;
                        go.GetComponent<ProjectileData>().BulletId = projectileData.BulletNetId;
                    }));
        }

    }
}