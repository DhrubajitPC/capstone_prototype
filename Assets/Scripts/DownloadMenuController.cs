using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
//using UnityEditor;


public class DownloadMenuController : MonoBehaviour {

    bool _isDownloading = false;
    bool isDownloading
    {
        get
        {
            return _isDownloading;
        }
        set
        {
            _isDownloading = value;
            GameObject.Find("Download Button").GetComponent<UnityEngine.UI.Button>().interactable = !value;
            GameObject.Find("Back Button").GetComponent<UnityEngine.UI.Button>().interactable = !value;
        }
    }

    // Use this for initialization
    void Start()
    {
        LoadBundles();
//        GameObject.Find("Url Field").GetComponent<UnityEngine.UI.InputField>().text = "http://luccan.github.io/capstone_prototype_assetbundle/";
//        GameObject.Find("BundleName Field").GetComponent<UnityEngine.UI.InputField>().text = "HouseO.sky";
        UnityEngine.UI.Dropdown dropdown = GameObject.Find("ActiveBundle Dropdown").GetComponent<UnityEngine.UI.Dropdown>();
        dropdown.value = 0;
        for (int i=0; i< dropdown.options.Count; i++)
        {
            if (dropdown.options[i].text == PlayerPrefs.GetString("ActiveBundleName"))
            {
                dropdown.value = i;
                break;
            }
        }
    }

    public void downloadBundleClick()
    {
        if (!isDownloading)
        {
            isDownloading = true;
            StartCoroutine(downloadBundle());
        }
    }

    public IEnumerator downloadBundle()
    {
        string url = GameObject.Find("Url Field").GetComponent<UnityEngine.UI.InputField>().text;
        string bundlename = GameObject.Find("BundleName Field").GetComponent<UnityEngine.UI.InputField>().text;

        PlayerPrefs.SetString("ERROR", "Done Downloading Bundle" + bundlename); //clear error
        showStatusText("Downloading Bundle " + bundlename + "...");
        yield return WWWLoader.downloadFiles(url, bundlename);
        LoadBundles();
        isDownloading = false;
        showStatusText(PlayerPrefs.GetString("ERROR"));
    }

    public void loadLayerScene(){
        PlayerPrefs.DeleteAll();
        string selected = GameObject.Find("ActiveBundle Dropdown/Label").GetComponent<UnityEngine.UI.Text>().text;
        if (selected.Length <= 0)
            selected = "";
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
