using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LayerMenuController : MonoBehaviour {

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

//
//	[MenuItem("VR Build Settings/Build GearVR")]
//	static void BuildGearVRSettings(){
//		PlayerSettings.virtualRealitySupported = true;
//		Cardboard cardboard = (Cardboard)GameObject.FindGameObjectWithTag("Cam").GetComponent<Cardboard>();
//		CardboardHead cardboardHead = cardboard.GetComponentInChildren<CardboardHead>();
//		cardboard.VRModeEnabled = false;
//		cardboardHead.trackPosition = false;
//		cardboardHead.trackRotation = false;
//		EditorUtility.SetDirty (cardboard);
//		EditorUtility.SetDirty (cardboardHead);
//	}
//
    // Use this for initialization
    void Start () {
        renderMat = false;
        showFurn = false;
        showHuman = false;
        enableFreeRoam = false;
        pathedTeleport = false;

//		Cardboard cardboard = (Cardboard)GameObject.FindGameObjectWithTag("Cam").GetComponent<Cardboard>();
//		CardboardHead cardboardHead = cardboard.GetComponentInChildren<CardboardHead>();
//		cardboardHead.trackPosition = false;
//		cardboardHead.trackRotation = false;
//		cardboard.VRModeEnabled = false;
//		GameObject.Find("Head").transform.position = new Vector3(0,0,0);
//		GameObject.Find("Head").transform.rotation = new Vector3(0,0,0);
//		cardboardHead.transform.position = new Vector3(0,0,0);
//		cardboardHead.transform.rotation = new Vector3(0,0,0);
//		GameObject.Find('Canvas').transform.position = new Vector3(0,0,0);

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
        SceneManager.LoadScene("Duxton Render",LoadSceneMode.Single);
    }

	// Update is called once per frame
	void LateUpdate () {
		//		relocating the camera angle to default
		float curX = GameObject.Find ("Head").transform.eulerAngles.x;
		float curY = GameObject.Find ("Head").transform.eulerAngles.y;
		float curZ = GameObject.Find ("Head").transform.eulerAngles.z;
//		print ("x:" + curX + "y:" + curY + "z:" + curZ);
//		print ("y:" + curY);
//		print ("z:" + curZ);
		if (curZ > 0.01f) {
//			GameObject.Find ("Head").transform.eulerAngles = new Vector3 (curX, curY, 0.01f);
//		} else if (curZ < -0.03f) {
//			GameObject.Find ("Head").transform.eulerAngles = new Vector3 (curX, curY, -0.03f);
		} else if (curY > 0.02f) {
			GameObject.Find ("Head").transform.eulerAngles = new Vector3 (curX, 0.02f, curZ);
		} else if (curY < -0.02f) {
			GameObject.Find ("Head").transform.eulerAngles = new Vector3 (curX, -0.02f, curZ);
		}
//		GameObject.Find ("Head").transform.eulerAngles = new Vector3 (0, 0, 0);
//		GameObject.Find ("Head").transform.position = new Vector3 (0, 0, 0);
	}
}
//y -0.04 0.02 x -0.03 0.01