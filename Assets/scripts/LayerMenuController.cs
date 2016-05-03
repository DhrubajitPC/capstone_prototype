using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LayerMenuController : MonoBehaviour {

    public UnityEngine.UI.Toggle toggle;
    public UnityEngine.UI.Toggle toggle2;

    private bool renderMat;
    private bool showHuman;

    // Use this for initialization
    void Start () {
        renderMat = false;
        showHuman = false;
        if (PlayerPrefs.GetInt("RenderMat") == 1) {
            toggle.isOn = PlayerPrefs.GetInt("RenderMat") == 1; //this toggles the trigger
        }
        if (PlayerPrefs.GetInt("ShowHuman") == 1)
        {
            toggle2.isOn = PlayerPrefs.GetInt("ShowHuman") == 1;
        }
    }
	
    public void ToggleMat(){
        renderMat = !renderMat;
    }
    public void ToggleHuman()
    {
        showHuman = !showHuman;
    }

    public void BackToRoom()
    {
        PlayerPrefs.SetInt("RenderMat", renderMat ? 1 : 0);
        PlayerPrefs.SetInt("ShowHuman", showHuman ? 1 : 0);
        SceneManager.LoadScene("Scene1",LoadSceneMode.Single);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
