using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class BundleSingleton : Singleton<BundleSingleton>
{
	private readonly List<AssetBundle> AssetBundleList = new List<AssetBundle>();
	
	public void OnDestroy()
	{
		UnloadAllBundles();
	}	

	public AssetBundle GetBundle(string name)
	{
		for (int i = 0; i < AssetBundleList.Count; ++i)
		{
			if (name == AssetBundleList[i].name)
			{
				return AssetBundleList[i];
			}
		}
		return null;
	}

	public IEnumerator LoadBundle(string path, Action<AssetBundle> onSuccess)
	{
		string name = System.IO.Path.GetFileNameWithoutExtension(path);
		AssetBundle assetBundle = GetBundle(name);
		string assetPath = System.IO.Path.Combine(
			Application.streamingAssetsPath, 
			Application.platform == RuntimePlatform.WebGLPlayer ? "WebGL" : "StandaloneWindows",
			path
		);

		if (assetBundle == null)
		{
			if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(assetPath);
		        yield return www.SendWebRequest();
		 
		        if (www.result != UnityWebRequest.Result.Success) {
		            Debug.Log(www.error);
		        }
		        else 
		        {
		            assetBundle = DownloadHandlerAssetBundle.GetContent(www);
		        }
			}
			else
			{
				assetBundle = AssetBundle.LoadFromFile(assetPath);
			}
			
			assetBundle.name = name;
			AssetBundleList.Add(assetBundle);
		}

		onSuccess(assetBundle);
	}

	private void UnloadAllBundles()
	{
		for (int i = 0; i < AssetBundleList.Count; ++i)
		{
			AssetBundleList[i].Unload(false);
		}
		AssetBundleList.Clear();
	}
}
