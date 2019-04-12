using Core.MessageBus;
using Core.Services;
using Main.AimTarget.API;
using Main.Weapons.API;
using Main.Weapons.Data;
using UnityEngine;

namespace Main.Weapons
{
    public class WeaponsFactory : SubscriberBehaviour
    {
        public static void InstantiateFactory()
        {
            var container = new GameObject("__Weapons_FACTORY");
            container.AddComponent<WeaponsFactory>();
        }
        
        [Subscribe(API.Messages.INSTANTIATE)]
        private void InstantiateWeapon(Message msg)
        {
            var weaponData = msg.Data as WeaponInstantiateData;

            ServiceLocator.GetService<ResourceLoaderService>()
                .InstantiatePrefabByPathName("Weapons/" + weaponData.Type.ToString(),
                    weapon =>
                    {
                        weapon.transform.parent = weaponData.Parent;
                        weapon.transform.localPosition = new Vector3(0.001f, 0.079f, 0.025f);
                        weapon.transform.localEulerAngles = new Vector3(94.46698f, 51.211f, 44.17799f);

                        //weapon.GetComponent<MessageChannelID>().Init(weaponData.ChannelId);

                        MessageBus.SendMessage(SubscribeType.Channel, weaponData.ChannelId,
                            CommonMessage.Get(Characters.API.Messages.SET_ARMED));

                        //MessageBus.SendMessageToChannel(weapon.GetComponent<MessageChannelID>(),
                        //    Message.GetMessage(Characters.API.Messages.SET_ARMED));
                    });
        }
        

    }
}