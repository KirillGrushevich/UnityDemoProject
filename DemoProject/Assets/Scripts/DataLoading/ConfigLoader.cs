using System.ComponentModel;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DataLoading
{
    public class ConfigLoader
    {
        private ScriptableObject cachedConfig;

        protected async Task<T> LoadInternal<T>(string assetId) where T : ScriptableObject
        {
            if (cachedConfig != null)
            {
                return (T)cachedConfig;
            }
            
            var handle = Addressables.LoadAssetAsync<T>(assetId);
            var so = await handle.Task;
            
            if (so == null)
            {
                throw new WarningException($"Object of type {typeof(T)} is not loaded");
            }

            cachedConfig = so;
            return (T)cachedConfig;
        }

        protected void UnloadInternal()
        {
            if (cachedConfig == null)
            {
                return;
            }
            
            Addressables.Release(cachedConfig);
            cachedConfig = null;
        }
    }
}