﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
//using UnityEditor;


public class DownloadMenuController : MonoBehaviour {

    bool isDownloading = false;

    // Use this for initialization
    void Start()
    {
        LoadBundles();
        GameObject.Find("Url Field").GetComponent<UnityEngine.UI.InputField>().text = "http://luccan.github.io/capstone_prototype_assetbundle/";
        GameObject.Find("BundleName Field").GetComponent<UnityEngine.UI.InputField>().text = "HouseO.sky";
    }

    public void downloadBundleClick()
    {
        StartCoroutine(downloadBundle());
    }

    public IEnumerator downloadBundle()
    {
        string url = GameObject.Find("Url Field").GetComponent<UnityEngine.UI.InputField>().text;
        string bundlename = GameObject.Find("BundleName Field").GetComponent<UnityEngine.UI.InputField>().text;

        isDownloading = true;
        showStatusText("Downloading Bundle " + bundlename + "...");
        yield return WWWLoader.downloadFiles(url, bundlename);
        showStatusText("Done Downloading Bundle " + bundlename);
        isDownloading = false;
    }

    public void loadLayerScene(){
        string selected = GameObject.Find("ActiveBundle Dropdown/Label").GetComponent<UnityEngine.UI.Text>().text;
        PlayerPrefs.SetString("ActiveBundleName", selected);
        SceneManager.LoadScene("LayerScene");
	}

    private void showStatusText(string text)
    {
        GameObject.Find("Status Text").GetComponent<UnityEngine.UI.Text>().text = text;
    }

    private void LoadBundles()
    {
        string[] bundles = System.IO.Directory.GetDirectories(WWWLoader.full_download_path);
        UnityEngine.UI.Dropdown dropdown = GameObject.Find("ActiveBundle Dropdown").GetComponent<UnityEngine.UI.Dropdown>();
        dropdown.ClearOptions();
        foreach (string bundle in bundles)
        {
            if (bundle.EndsWith(".sky"))
            {
                string bundlename = System.IO.Path.GetFileNameWithoutExtension(bundle);
                dropdown.options.Add(new UnityEngine.UI.Dropdown.OptionData(bundlename));
            }
        }
        dropdown.RefreshShownValue();
    }
}
