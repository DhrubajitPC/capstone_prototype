using UnityEngine;
using System.Collections;
using System;

public class RoomControl : MonoBehaviour {

    public GameObject BaseGeometryObj;
    public Material BaseGeometryMat; //???? CANNOT ASSIGN MATERIALS TO OBJECTS????

    private String baseMat = "none";
    // Use this for initialization
    void Start () {
        BaseGeometryMat.mainTexture = null;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space"))
        {
            MeshRenderer mr = BaseGeometryObj.GetComponentInChildren<MeshRenderer>() as MeshRenderer;
            if (baseMat == "none"){
                Texture t = Resources.Load("images/WDF_2069399") as Texture;
                BaseGeometryMat.mainTexture = t;
                //mr.sharedMaterial = BaseGeometryMat;
                baseMat = "material";
            }
            else{
                Texture t = new Texture();
                BaseGeometryMat.mainTexture = t;
                //mr.sharedMaterial = BaseGeometryMat;
                baseMat = "none";
            }
        }
	}
}
