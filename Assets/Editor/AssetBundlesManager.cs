﻿using UnityEngine;
using System.Collections;
using UnityEditor;


public class AssetBundlesManager
{
    [MenuItem("AssetBundle/单独打包")]
    public static void CreatAssetBundleSeparate()
    {
        //获取在Project视图中选择的所有游戏对象
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        //遍历所有的游戏对象
        foreach (Object obj in SelectedAsset)
        {
            string sourcePath = AssetDatabase.GetAssetPath(obj);
            //本地测试：建议最后将Assetbundle放在StreamingAssets文件夹下，如果没有就创建一个，因为移动平台下只能读取这个路径
            //服务器下载：就不需要放在这里，服务器上客户端用www类进行下载。
            string targetPath = Application.streamingAssetsPath + "/" + obj.name + ".assetbundle";
            if (BuildPipeline.BuildAssetBundle(obj, null, targetPath, BuildAssetBundleOptions.CollectDependencies))
            {
                Debug.Log(obj.name + "资源打包成功");
            }
            else
            {
                Debug.Log(obj.name + "资源打包失败");
            }
        }
        //刷新编辑器
        AssetDatabase.Refresh();    
    }

    [MenuItem("AssetBundle/整体打包")]
    public static void CreatAssetBundleAll()
    {
        Caching.CleanCache();

        string Path = Application.streamingAssetsPath + "/ALL.assetbundle";

        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        foreach (Object obj in SelectedAsset)
        {
            Debug.Log("Create AssetBunldes name :" + obj);
        }

        //这里注意第二个参数就行
        if (BuildPipeline.BuildAssetBundle(null, SelectedAsset, Path, BuildAssetBundleOptions.CollectDependencies))
        {
            AssetDatabase.Refresh();
        }
        else
        {

        }
    }
}
