using UnityEngine;
using System.Collections;
using System;

public static class WWWLoader{

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

}
