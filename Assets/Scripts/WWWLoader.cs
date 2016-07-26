using UnityEngine;
using System.Collections;
using System;
using System.IO;

public static class WWWLoader{
    static bool is_downloading = false;
    public const string resources_path = "downloads/";
    public const string full_resources_path = "Assets/resources/downloads/";
    public const string download_url = "https://luccan.github.io/capstone_prototype_assetbundle/";

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
