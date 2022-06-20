using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DataLoading
{
    public class AssetLoader
    {
        private GameObject cachedGameObject;

        protected async Task<T> LoadInternal<T>(string assetId)
        {
            var handle = Addressables.InstantiateAsync(assetId);
            cachedGameObject = await handle.Task;

            if (!cachedGameObject.TryGetComponent(out T obj))
            {
                throw new NullReferenceException($"Object of type {obj.GetType()} is null");
            }

            return obj;
        }

        protected void UnloadInternal()
        {
            if (cachedGameObject == null)
            {
                return;
            }
            
            cachedGameObject.SetActive(false);
            Addressables.ReleaseInstance(cachedGameObject);
            cachedGameObject = null;
        }
    }
}