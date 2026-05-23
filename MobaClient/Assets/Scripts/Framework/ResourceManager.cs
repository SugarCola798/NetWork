using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class ResourceManager : GameSingleton<ResourceManager>
    {
        public async UniTask<T> LoadAsync<T>(string path) where T : Object
        {
            return null;
        }
    }   
}
