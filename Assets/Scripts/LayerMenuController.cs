﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LayerMenuController : MonoBehaviour {

    public GameObject canvas;
    public UnityEngine.UI.Toggle toggleMat;
    public UnityEngine.UI.Toggle toggleFurn;
    public UnityEngine.UI.Toggle toggleHuman;
    public UnityEngine.UI.Toggle toggleFreeRoam;
    public UnityEngine.UI.Toggle togglePathedTele;

    private bool renderMat;
    private bool showFurn;
    private bool showHuman;
    private bool enableFreeRoam;
    private bool pathedTeleport;

    // Use this for initialization
    void Start () {
        //Ensure canvas is always in front of you (and facing you)
        canvas.transform.position = this.transform.position + this.transform.Find("Head").forward.normalized * 200;
        canvas.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        renderMat = false;
        showFurn = false;
        showHuman = false;
        enableFreeRoam = false;
        pathedTeleport = false;
        if (PlayerPrefs.GetInt("RenderMat") == 1) {
            toggleMat.isOn = PlayerPrefs.GetInt("RenderMat") == 1; //this toggles the trigger
        }
        if (PlayerPrefs.GetInt("ShowFurn") == 1)
        {
            toggleFurn.isOn = PlayerPrefs.GetInt("ShowFurn") == 1;
        }
        if (PlayerPrefs.GetInt("ShowHuman") == 1)
        {
            toggleHuman.isOn = PlayerPrefs.GetInt("ShowHuman") == 1;
        }
        if (PlayerPrefs.GetInt("EnableFreeRoam") == 1)
        {
            toggleFreeRoam.isOn = PlayerPrefs.GetInt("EnableFreeRoam") == 1;
        }
        if (PlayerPrefs.GetInt("PathedTeleport") == 1)
        {
            togglePathedTele.isOn = PlayerPrefs.GetInt("PathedTeleport") == 1;
        }
    }
	
    public void ToggleMat(){
        renderMat = !renderMat;
    }
    public void ToggleFurn()
    {
        showFurn = !showFurn;
    }
    public void ToggleHuman()
    {
        showHuman = !showHuman;
    }
    public void ToggleFreeRoam()
    {
        enableFreeRoam = !enableFreeRoam;
    }
    public void TogglePathedTele()
    {
        pathedTeleport = !pathedTeleport;
    }

    public void BackToRoom()
    {
        PlayerPrefs.SetInt("RenderMat", renderMat ? 1 : 0);
        PlayerPrefs.SetInt("ShowFurn", showFurn ? 1 : 0);
        PlayerPrefs.SetInt("ShowHuman", showHuman ? 1 : 0);
        PlayerPrefs.SetInt("EnableFreeRoam", enableFreeRoam ? 1 : 0);
        PlayerPrefs.SetInt("PathedTeleport", pathedTeleport ? 1 : 0);
        SceneManager.LoadScene("Dynamic Scene",LoadSceneMode.Single);
    }

	// Update is called once per frame
	void LateUpdate () {
		//		relocating the camera angle to default
//		GameObject.Find ("CardboardMain").transform.eulerAngles = new Vector3 (0, 0, 0);
//		GameObject.Find ("CardboardMain").transform.position = new Vector3 (0, 0, 0);
	}
}
