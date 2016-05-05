using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class RoomControl : MonoBehaviour {

    public GameObject PlayerObj;
    public GameObject BaseGeometryObj;
    public Material BaseGeometryMat; //???? CANNOT ASSIGN MATERIALS TO OBJECTS????
    public GameObject[] HumanObjs; //should be replaced with coordinates?

    private bool renderMat;
    private bool showHuman;
    // Use this for initialization
    void Start () {
        Cardboard.SDK.OnTrigger += TriggerPulled;
        renderMat = PlayerPrefs.GetInt("RenderMat") == 1;
        showHuman = PlayerPrefs.GetInt("ShowHuman") == 1;
        if (PlayerPrefs.GetInt("LoadLocation") == 1){
            PlayerObj.transform.position = new Vector3(PlayerPrefs.GetFloat("LocationX"),
                PlayerPrefs.GetFloat("LocationY"), PlayerPrefs.GetFloat("LocationZ"));
            PlayerObj.transform.rotation = new Quaternion(
                PlayerPrefs.GetFloat("QuarternionX"), PlayerPrefs.GetFloat("QuarternionY"),
                PlayerPrefs.GetFloat("QuarternionZ"), PlayerPrefs.GetFloat("QuarternionW"));
            PlayerPrefs.SetInt("LoadLocation", 0);
        }
        ApplyMaterialLayer();
        ApplyHumanLayer();
    }
	
    void TriggerPulled()
    {
        PlayerPrefs.SetInt("RenderMat", renderMat ? 1 : 0);
        PlayerPrefs.SetInt("ShowHuman", showHuman ? 1 : 0);
        SceneManager.LoadScene("LayerScene", LoadSceneMode.Single);
        PlayerPrefs.SetInt("LoadLocation", 1);
        PlayerPrefs.SetFloat("LocationX", PlayerObj.transform.position.x);
        PlayerPrefs.SetFloat("LocationY", PlayerObj.transform.position.y);
        PlayerPrefs.SetFloat("LocationZ", PlayerObj.transform.position.z);
        PlayerPrefs.SetFloat("QuarternionX", PlayerObj.transform.Find("CardboardMain/Head").rotation.x);
        PlayerPrefs.SetFloat("QuarternionY", PlayerObj.transform.Find("CardboardMain/Head").rotation.y);
        PlayerPrefs.SetFloat("QuarternionZ", PlayerObj.transform.Find("CardboardMain/Head").rotation.z);
        PlayerPrefs.SetFloat("QuarternionW", PlayerObj.transform.Find("CardboardMain/Head").rotation.w);
    }

	// Update is called once per frame
	void Update () {

	}

    void ApplyMaterialLayer()
    {
        MeshRenderer mr = BaseGeometryObj.GetComponentInChildren<MeshRenderer>() as MeshRenderer;
        if (renderMat)
        {
            Texture t = Resources.Load("images/WDF_2069399") as Texture;
            BaseGeometryMat.mainTexture = t;
            //mr.sharedMaterial = BaseGeometryMat;
        }
        else {
            Texture t = new Texture();
            BaseGeometryMat.mainTexture = t;
            //mr.sharedMaterial = BaseGeometryMat;
        }
    }
    void ApplyHumanLayer()
    {
        if (!showHuman)
        {
            foreach (GameObject human in HumanObjs)
            {
                human.SetActive(false);
            }
        }
    }
}
