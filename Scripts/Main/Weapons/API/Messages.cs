using Core.MessageBus;
using Core.Services;
using System;
using UnityEngine;

namespace Main.Weapons.API
{
    public static class Messages
    {
        public const string INSTANTIATE = "Main.Weapons.INSTANTIATE";
        public const string SHOOT = "Main.Weapons.SHOOT";
        public const string FIRING = "Main.Weapons.FIRING";
        public const string HIT = "Main.Weapons.HIT";
        
        public const string START_RELOAD = "Main.Weapons.START_RELOAD";
        public const string END_RELOAD = "Main.Weapons.END_RELOAD";
        public const string UPDATE_MAGAZINE_BULLETS_AMOUNT = "Main.Weapons.UPDATE_MAGAZINE_BULLETS_AMOUNT";
        public const string NO_BULLETS_IN_MAGAZINE = "Main.Weapons.NO_BULLETS_IN_MAGAZINE";
    }

    public enum WeaponType
    {
        HEMLOK = 0,
        WINGMAN = 1,
        PEACEKEEPER = 2,
        PISTOL = 3,
    }

    [Serializable]
    public class WeaponInstantiateData : MessageData
    {
        private static ObjectPool<WeaponInstantiateData> _pool = new ObjectPool<WeaponInstantiateData>(2);

        public static WeaponInstantiateData GetWeaponInstantiateData
            (WeaponType type, Transform parent, int channelId, bool createAimTarget)
        {
            var data = _pool.Get();
            data.Type = type;
            data.Parent = parent;
            data.ChannelId = channelId;
            data.CreateAimTarget = createAimTarget;
            return data;
        }
        
        public WeaponType Type;
        [NonSerialized]
        public Transform Parent;
        [NonSerialized]
        public int ChannelId;
        public bool CreateAimTarget;

        public WeaponInstantiateData() {}
        
        public override void FreeObjectInPool()
        {
            _pool.Release(this);
        }
        
        public override MessageData GetCopy()
        {
            var data = _pool.Get();
            data.Type = Type;
            data.Parent = Parent;
            data.ChannelId = ChannelId;
            data.CreateAimTarget = CreateAimTarget;
            return data;
        }
    }
}