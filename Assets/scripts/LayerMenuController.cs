using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LayerMenuController : MonoBehaviour {

    public UnityEngine.UI.Toggle toggle;

    private bool renderMat;

	// Use this for initialization
	void Start () {
        renderMat = false;
        if (PlayerPrefs.GetInt("RenderMat") == 1) {
            toggle.isOn = PlayerPrefs.GetInt("RenderMat") == 1; //this toggles the trigger
        }
    }
	
    public void ToggleMat(){
        renderMat = !renderMat;
    }

    public void BackToRoom()
    {
        PlayerPrefs.SetInt("RenderMat", renderMat ? 1 : 0);
        SceneManager.LoadScene("Scene1",LoadSceneMode.Single);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
