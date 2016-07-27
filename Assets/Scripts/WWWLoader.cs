using UnityEngine;
using System.Collections;
using System;
using System.IO;

public static class WWWLoader{
    static bool is_downloading = false;
    public const string download_path = "downloads/";
#if UNITY_EDITOR
    public const string full_download_path = "Assets/resources/downloads/";
#elif UNITY_ANDROID
    public static string full_download_path = Application.persistentDataPath + "/";
#endif
    public static string active_download_path
    {
        get
        {
            return full_download_path + PlayerPrefs.GetString("ActiveBundleName") + ".sky/";
        }
    }

    public static IEnumerator downloadFiles(string download_url, string bundlename)
    {
        if (!bundlename.EndsWith(".sky"))
            bundlename += ".sky";
        bundlename += "/"; //this should be a directory
        if (!download_url.EndsWith("/"))
            download_url += "/";
        download_url += bundlename;
        string savepath = full_download_path + bundlename;

        if (System.IO.Directory.Exists(savepath))
        {
            System.IO.Directory.Delete(savepath, true);
        }
        System.IO.Directory.CreateDirectory(savepath);

        string lol = String.Join(",", System.IO.Directory.GetDirectories(full_download_path));
        PlayerPrefs.SetString("ERROR", savepath + "!" + lol);

        yield return downloadFile(download_url, "renderbundle", savepath);
        yield return downloadFile(download_url, "cfd.csv", savepath);
        yield return downloadFile(download_url, "cfdorigin.csv", savepath);
        yield return downloadFile(download_url, "jumplocations.csv", savepath);
        yield return downloadFile(download_url, "humancoords.csv", savepath);
        yield return downloadFile(download_url, "layersettings.csv", savepath);
        yield return downloadFile(download_url, "noisecoords.csv", savepath);
        yield return 0;
    }

    public static IEnumerator downloadFile(string download_url, string filename, string savepath)
    {
        is_downloading = true;

        try {
            string path = savepath + filename;
            System.Net.WebClient client = new System.Net.WebClient();
            client.DownloadFile(download_url + filename, path);
        } catch (Exception ex)
        {
            PlayerPrefs.SetString("ERROR", "!" + ex.Message);
        }

        is_downloading = false;
        yield return null;
    }

}
