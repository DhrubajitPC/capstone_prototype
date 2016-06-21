using UnityEngine;
using System.Collections;
using System;
using System.IO;

public static class WWWLoader{
    static bool is_downloading = false;
    public const string resources_path = "downloads/";
    public const string download_url = "https://luccan.github.io/capstone_prototype_assetbundle/";

    public static WWW loadWWWAssetBundle(string url, int version)
    {
        WWW www;
        try
        {
            // load AssetBundle file from Cache if it exists with the same version or download and store it in the cache
            www = WWW.LoadFromCacheOrDownload(url, version);
            //yield return www;

            Debug.Log("Loaded ");

            if (www.error != null)
            {
                GameObject.Find("ERROR").GetComponent<UnityEngine.UI.Text>().text = www.error;
            }
        }
        catch (Exception e)
        {
            throw new Exception("WWW download had an error: " + e.Message);
        }
        return www;
    }

    public static WWW loadWWWText(string url)
    {
        WWW www;
        try
        {
            // load AssetBundle file from Cache if it exists with the same version or download and store it in the cache
            www = new WWW(url);
            //www = WWW.???????
            //yield return www;

            Debug.Log("Loaded ");

            if (www.error != null)
            {
                GameObject.Find("ERROR").GetComponent<UnityEngine.UI.Text>().text = www.error;
            }
        }
        catch (Exception e)
        {
            throw new Exception("WWW download had an error: " + e.Message);
        }
        return www;
    }

    public static IEnumerator downloadFile(string filename)
    {
        is_downloading = true;
        WWW www = new WWW(download_url + filename);
        yield return www;
        string content = www.text;
        string path = null;
        path = resources_path + filename;
        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(content);
            }
        }
        #if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
        #endif
        is_downloading = false;
    }

}
