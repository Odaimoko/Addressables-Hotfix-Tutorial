using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Imk.AddrTutorial
{
    public static class AddrHelper
    {
        public static UnityAction OnRetriveLatest;

        public static UnityAction OnPreBundleChanged;

        // https://forum.unity.com/threads/can-i-ask-addressables-if-a-specified-key-addressable-name-is-valid.574033/#post-3826660
        public static bool AddressableResourceExists<T>(object key)
        {
            foreach (var l in Addressables.ResourceLocators)
            {
                if (l.Locate(key, typeof(T), out _))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateKeys"></param>
        /// <param name="download"></param>
        /// <returns>True if Catalog needs to be updated</returns>
        public static async Task<bool> UpdateCatalog(List<object> updateKeys = null, bool download = false)
        {
            var init = Addressables.InitializeAsync();
            await init.Task;

            // The operation containing the list of catalog ids that have an available update.  This can be used to filter which catalogs to update with the UpdateContent.
            var checkUpdateHandle = Addressables.CheckForCatalogUpdates(false);
            await checkUpdateHandle.Task;
            Debug.Log("check catalog status " + checkUpdateHandle.Status);
            var success = checkUpdateHandle.Status == AsyncOperationStatus.Succeeded;
            if (success)
            {
                List<string> catalogs = checkUpdateHandle.Result;

                if (catalogs != null && catalogs.Count > 0)
                {
                    success = true;
                    foreach (var catalog in catalogs)
                    {
                        Debug.Log($"catalog needed to be updated: {catalog}");
                    }

                    if (download)
                    {
                        Debug.Log("download catalog start ");
                        var updateHandle = Addressables.UpdateCatalogs(catalogs, false);
                        await updateHandle.Task;
                        foreach (var item in updateHandle.Result)
                        {
                            Debug.Log("catalog result " + item.LocatorId);
                            foreach (var key in item.Keys)
                            {
                                Debug.Log("catalog key " + key);
                            }

                            if (updateKeys == null)
                                continue;
                            updateKeys.Clear();
                            updateKeys.AddRange(item.Keys);
                        }

                        Debug.Log("download catalog finish " + updateHandle.Status);
                        Addressables.Release(updateHandle);
                    }
                }
                else
                {
                    success = false;
                    Debug.Log("dont need update catalogs");
                }
            }

            Addressables.Release(checkUpdateHandle);
            return success;
        }

        /// <summary>
        /// Returns size in Bytes.
        /// </summary>
        /// <param name="updateKeys"></param>
        /// <returns></returns>
        public static async Task<long> GetDownloadSize(IEnumerable<object> updateKeys)
        {
            var handle = Addressables.GetDownloadSizeAsync(updateKeys);
            var downloadsize = await handle.Task;
            Addressables.Release(handle);
            return downloadsize;
        }

        public static async Task DownAssetImpl(IEnumerable<object> updateKeys)
        {
            var handle = Addressables.DownloadDependenciesAsync(updateKeys, Addressables.MergeMode.Union);
            var downloadResult = await handle.Task;
            LogDownloadedBundles(downloadResult as List<IAssetBundleResource>);

            Addressables.Release(handle);
        }


        public static async Task ClearAllCache()
        {
            foreach (var locator in Addressables.ResourceLocators)
            {
                Debug.Log($"Clearing cache for {locator.LocatorId}");
                // Will throw exception if the bundle is still in use
                var handle = Addressables.ClearDependencyCacheAsync(locator.Keys, false); // Catalog
                await handle.Task;
                Addressables.Release(handle);
            }

            Caching.ClearCache(); // Bundle
        }

        // public static void ClearUnusedBundles()
        // {
        //     Addressables.CleanBundleCache().Completed += handle =>
        //     {
        //         Debug.Log("CleanBundleCache " + handle.Result);
        //         Addressables.Release(handle);
        //     };
        // }

        public static async Task LoadLatestCatalogFromCache()
        {
            if (File.Exists(AddressablesInfo.CatalogCacheDir))
            {
                var files = Directory.GetFiles(AddressablesInfo.CatalogCacheDir);
                // What if multiple catalogs? Use the latest
                // sort by last write time, descending
                Array.Sort(files, (a, b) =>
                    new FileInfo(b).LastWriteTime.CompareTo(new FileInfo(a).LastWriteTime)
                );
                foreach (var file in files)
                {
                    if (file.EndsWith(".json"))
                    {
                        await Addressables.LoadContentCatalogAsync(file, true).Task;
                        Debug.Log($"Loaded Catalog: {file}");
                        break;
                    }
                }
            }
        }

        #region Debug

        private static void LogDownloadedBundles(List<IAssetBundleResource> downloadResult)
        {
            if (downloadResult == null)
            {
                Debug.Log("download result is null");
                return;
            }

            Debug.Log("download result type " + downloadResult.GetType());
            foreach (var item in downloadResult)
            {
                var ab = item.GetAssetBundle();
                Debug.Log("ab name " + ab.name);
                foreach (var name in ab.GetAllAssetNames())
                {
                    Debug.Log("asset name " + name);
                }
            }
        }

        public static void LogRemoteBundlesInCurrentCatalog()
        {
            // Get all the keys of the Addressables
            var locators = Addressables.ResourceLocators;

            // List to store bundles in use
            List<string> bundlesInUse = new List<string>();
            // Loop through all the keys
            foreach (var locator in locators)
            {
                var keys = locator.Keys;
                foreach (var key in keys)
                {
                    // Get the locations for each key
                    if (locator.Locate(key, typeof(object),
                            out IList<IResourceLocation> locationsList))
                    {
                        // Loop through each location
                        foreach (var location in locationsList)
                        {
                            // Check if the location is an asset bundle
                            if (location.InternalId.StartsWith("http://") || location.InternalId.StartsWith("https://"))
                            {
                                // Add the asset bundle name to the list of bundles in use
                                bundlesInUse.Add(location.PrimaryKey);
                            }
                        }
                    }
                }
            }

            // Print or process the list of bundles in use
            foreach (var bundle in bundlesInUse)
            {
                Debug.Log("Bundle in use: " + bundle);
            }
        }

        #endregion
    }
}