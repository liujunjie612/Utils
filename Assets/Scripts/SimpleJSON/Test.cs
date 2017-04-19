using UnityEngine;
using System.Collections;

namespace SimpleJSON
{
    public class Test : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(load());
        }

        IEnumerator load()
        {
            WWW www = new WWW("file:///" + Application.streamingAssetsPath + "/JsonTxt.txt");

            yield return www;
            if (System.String.IsNullOrEmpty(www.error))
            {
                string s = www.text;
                JSONNode node = JSONNode.Parse(s);
                Debug.Log(node["sn"].AsInt);
                Debug.Log(node["ws"]);
                Debug.Log(node["ws"][0]["cw"]);
                string t = node["ws"][0]["cw"][0]["w"];
                Debug.Log(t);
            }
        }
    }
}
