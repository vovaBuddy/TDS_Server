using Core.MessageBus;
using Core.Services;
using Main.Bullets.API;
using Main.Bullets.Data;
using Main.Characters.Components;
using UnityEngine;

namespace Main.Bullets.Components
{
    public class CollisionComponent : MonoBehaviour
    {
        private ProjectileData _projectileData;

        private void Awake()
        {
            _projectileData = GetComponent<ProjectileData>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            var collisioned = collision.gameObject.GetComponent<IOnCollisionComponent>();

            if (collisioned != null)
            {
                collisioned.OnCollisionAction(_projectileData, collision);
            }
        }
    }
}