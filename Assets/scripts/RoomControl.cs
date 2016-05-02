using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class RoomControl : MonoBehaviour {

    public GameObject BaseGeometryObj;
    public Material BaseGeometryMat; //???? CANNOT ASSIGN MATERIALS TO OBJECTS????

    private bool renderMat;
    // Use this for initialization
    void Start () {
        Cardboard.SDK.OnTrigger += TriggerPulled;
        renderMat = PlayerPrefs.GetInt("RenderMat") == 1;
        ApplyMaterialLayer();
	}
	
    void TriggerPulled()
    {
        PlayerPrefs.SetInt("RenderMat", renderMat ? 1 : 0);
        SceneManager.LoadScene("LayerScene", LoadSceneMode.Single);
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space"))
        {
            renderMat = !renderMat;
            ApplyMaterialLayer();
        }
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
}
