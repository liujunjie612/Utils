using UnityEngine;
using System.Collections;

namespace AssetBundle
{
    public class Test : MonoBehaviour
    {

        void Start()
        {
            //StartCoroutine(loadAssetBundle());

            StartCoroutine(loadAssetBundleAll());
        }

        IEnumerator loadAssetBundle()
        {
            Debug.Log("Begain");
            WWW bundle = WWW.LoadFromCacheOrDownload("file://" + Application.streamingAssetsPath + "/Button.assetbundle", 0);
            yield return bundle;

            GameObject go = Instantiate(bundle.assetBundle.mainAsset) as GameObject;
            go.transform.parent = this.transform;
            go.transform.localPosition = Vector3.zero;

            bundle.assetBundle.Unload(false);
            Debug.Log("Done");
        }

        IEnumerator loadAssetBundleAll()
        {
            Debug.Log("Begain");
            WWW bundle = WWW.LoadFromCacheOrDownload("file://" + Application.streamingAssetsPath + "/ALL.assetbundle", 0);
            yield return bundle;

            GameObject go1 = Instantiate(bundle.assetBundle.LoadAsset("Button")) as GameObject;
            go1.transform.parent = this.transform;
            go1.transform.localPosition = Vector3.zero;

            GameObject go2 = Instantiate(bundle.assetBundle.LoadAsset("RawImage")) as GameObject;
            go2.transform.parent = this.transform;
            go2.transform.localPosition = Vector3.zero;

            bundle.assetBundle.Unload(false);
            Debug.Log("Done");
        }
    }
}
