using System.Collections;
using Core.MessageBus;
using Main.Bullets;
using Main.Bullets.API;
using Main.Weapons.Data;
using UnityEngine;

namespace Main.Weapons.Components
{
    public class ControlComponent : SubscriberBehaviour
    {
        private WeaponData _weaponData;
        
        private Message _endReloadMsg;

        private float _kdTimer;

        protected override void Awake()
        {
            base.Awake();
            _weaponData = gameObject.GetComponent<WeaponData>();
        }

        [Subscribe(SubscribeType.Channel,API.Messages.SHOOT)]
        private void Shoot(Message msg)
        {
            if (_kdTimer <= 0)
            {
                if (_weaponData.MagazineBulletAmount > 0)
                {
                    --_weaponData.MagazineBulletAmount;

                    _kdTimer = _weaponData.WeaponConfig.shootKd;

                    //ToDo: replace to server
                    var id = 1000 * Channel.ChannelIds[SubscribeType.Channel] + BulletsFactory.GetNextBulletId();

                    //MessageBus.SendMessage(Message.GetMessage(Messages.CREATE_PROJECTILE, 
                    //    ProjectileMessageData.GetProjectileMessageData(
                    //        id,
                    //        0,
                    //        _weaponData.ShootPosition.position,
                    //        _weaponData.ShootPosition.rotation,
                    //        _weaponData.WeaponConfig.bulletType,
                    //        _weaponData.WeaponConfig.bulletStartSpeed,
                    //        _weaponData.WeaponConfig.damageFactor)));
                    
                    //MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel], 
                    //    Message.GetMessage(API.Messages.FIRING));
                    
                }
                else
                {
                    _weaponData.NoBulletsInMagazine();
                }
            }
        }
        
        [Subscribe(SubscribeType.Channel,API.Messages.START_RELOAD)]
        private void Reload(Message msg)
        {
            //var key = LootType.BULLETS.ToString("d") + _weaponData.WeaponConfig.bulletType.ToString("d");
            
            //_weaponData.MagazineBulletAmount += _weaponData.Data.GetNeededAmountByType(
            //    _weaponData.WeaponConfig.magazineBulletsAmount - _weaponData.MagazineBulletAmount, 
            //    key);
            
            //StartCoroutine(Reload(_weaponData.WeaponConfig.reloadKd));
        }

        private IEnumerator Reload(float time)
        {
            yield return new WaitForSeconds(time);
            
            MessageBus.SendMessage(SubscribeType.Channel, Channel.ChannelIds[SubscribeType.Channel], CommonMessage.Get(API.Messages.END_RELOAD));
        }

        private void Update()
        {
            _kdTimer -= Time.deltaTime;
        }
    }
}