using UnityEngine;
using System.Collections;
using System.IO;

public class LoadMapNavData : MonoBehaviour
{

    // Use this for initialization
    IEnumerator Start() 
    {

        yield return StartCoroutine(LoadData());
        Debug.Log("Suc");

    }


    private IEnumerator LoadData()
    {
        //string AssetBundlesOutputPath = Application.dataPath;
        //AssetBundlesOutputPath = AssetBundlesOutputPath + "\\Resources\\Map\\MapNavData";
        string AssetBundlesOutputPath = "Map\\MapNavData";
        string strTargetPlatform = "";
#if UNITY_WEBPLAYER
         strTargetPlatform = "WebPlayer";
#elif UNITY_ANDROID
         strTargetPlatform = "Android";
#elif UNITY_IPHONE
         strTargetPlatform = "IOS";
#else
        strTargetPlatform = "StandaloneWindows";
#endif
        AssetBundlesOutputPath = Path.Combine(AssetBundlesOutputPath, strTargetPlatform) + "\\MapNavData";

        TextAsset bindata = Resources.Load(AssetBundlesOutputPath) as TextAsset;

        AssetBundleCreateRequest tAbcRequest = AssetBundle.LoadFromMemoryAsync(bindata.bytes);

        yield return tAbcRequest;

        // Get the reference to the loaded object
        MapNavData tMapNavData = tAbcRequest.assetBundle.mainAsset as MapNavData;

        // Unload the AssetBundles compressed contents to conserve memory
        tAbcRequest.assetBundle.Unload(false);
        tAbcRequest = null;

        Debug.Log("......");


    }

    // Update is called once per frame
    void Update()
    {

    }


}
