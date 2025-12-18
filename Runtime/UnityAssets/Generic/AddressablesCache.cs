using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace LucasWarwick02.UnityAssets
{
    /// <summary>
    /// Caches Addressables assets for quick access.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AddressablesCache<T>
    {
        /// <summary>
        /// The key used to load the asset.
        /// </summary>
        public string Key { private set; get;}

        /// <summary>
        /// The cached asset.
        /// </summary>
        public T Cache { private set; get; }

        /// <summary>
        /// Indicates whether the asset has been loaded.
        /// </summary>
        public bool Loaded { private set; get; }

        /// <summary>
        /// The in-flight load task, if any.
        /// </summary>
        private Task<T> _loadTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressablesCache{T}"/> class.
        /// </summary>
        public AddressablesCache(string key)
        {
            Key = key;
            Loaded = false;
        }

        /// <summary>
        /// Preloads the asset into the cache without blocking.
        /// </summary>
        public void Warm()
        {
            _ = LoadAsync();
        }

        /// <summary>
        /// Loads the asset asynchronously into the cache.
        /// </summary>
        public async Task<T> LoadAsync()
        {
            // If already loaded, return from cache
            if (Loaded)
            {
                return Cache;
            }

            // If load is already in progress, wait for that task
            if (_loadTask != null)
            {
                return await _loadTask;
            }

            // Start a new load operation
            _loadTask = PerformLoadAsync();
            return await _loadTask;
        }

        /// <summary>
        /// Performs the actual load operation.
        /// </summary>
        private async Task<T> PerformLoadAsync()
        {
            var handle = Addressables.LoadAssetAsync<T>(Key);
            await handle.Task;
            Cache = handle.Result;
            Loaded = true;

            return Cache;
        }

        /// <summary>
        /// Loads the asset from the cache.
        /// </summary>
        public T Load()
        {
            if (!Loaded) {
                throw new System.InvalidOperationException("Asset not loaded yet. Do not call Load() immediately after Warm() - you must wait for the asset to warm up (typically <1s). Use LoadAsync() for proper async handling.");
            }

            return Cache;
        }
    }
}