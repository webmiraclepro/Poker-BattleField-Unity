using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class BundleSingleton : Singleton<BundleSingleton>
{
	private readonly List<AssetBundle> AssetBundleList = new List<AssetBundle>();
	
	private void Awake ()
	{
		if (_currentLevelAssetBundle != null)
		{
			_currentLevelAssetBundle.Unload(false);
			_currentLevelAssetBundle = null;
		}	
	}
	
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

	public async Task<AssetBundle> LoadBundle(string path)
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
		        await www.SendWebRequest();
		 
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

		return assetBundle;
	}

	private void UnloadAllBundles()
	{
		for (int i = 0; i < AssetBundleList.Count; ++i)
		{
			AssetBundleList[i].Unload(false);
		}
		AssetBundleList.Clear();
	}
	
	public void LoadLevelAssetBundle(string level)
	{
		string path = DirectoryUtility.ExternalAssets() + level + ".assetBundle";
		Debug.Log("LoadLevelAssetBundle: " + path);
		_currentLevelAssetBundle = AssetBundle.LoadFromFile(path);
		if (_currentLevelAssetBundle != null && Application.CanStreamedLevelBeLoaded(level))
		{
			BundleSingleton.Instance.UnloadAllBundles();
			SceneManager.LoadScene(level);	
		}
		else
		{
			Debug.Log("AssetBundle Not Found: " + path);
		}
	}
	static private AssetBundle _currentLevelAssetBundle;
}

