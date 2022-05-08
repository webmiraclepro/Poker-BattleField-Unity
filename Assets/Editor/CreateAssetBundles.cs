using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build WebGL AssetBundles")]
    static void BuildWebGLAssetBundles()
    {
        string assetBundleDirectory = "Assets/StreamingAssets/" + BuildTarget.WebGL.ToString();
        if(!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, 
                                        BuildAssetBundleOptions.ChunkBasedCompression, 
                                        BuildTarget.WebGL);
    }

    [MenuItem("Assets/Build StandaloneWindows AssetBundles")]
    static void BuildPCAssetBundles()
    {
        string assetBundleDirectory = "Assets/StreamingAssets/" + BuildTarget.StandaloneWindows.ToString();
        if(!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, 
                                        BuildAssetBundleOptions.None, 
                                        BuildTarget.StandaloneWindows);
    }
}
