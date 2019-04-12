using Core.MessageBus;
using Core.MessageBus.MessageDataTemplates;
using Core.Services;

namespace Main.Character.API
{
    public class CharacterInstantiateData : MessageData
    {
        private static ObjectPool<CharacterInstantiateData> _pool = new ObjectPool<CharacterInstantiateData>();

        public int NetId;
        public int ConnectionId;
        
        public override void FreeObjectInPool()
        {
            throw new System.NotImplementedException();
        }

        public override MessageData GetCopy()
        {
            throw new System.NotImplementedException();
        }
    }
}