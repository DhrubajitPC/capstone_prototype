using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class RoomControl : MonoBehaviour {

    public GameObject PlayerObj;
    public GameObject FurnitureMain;
    // public Vector4[] HumanCoords; // w is y rotation

    List<Vector4> HumanCoords = new List<Vector4>(); //Added by Lawrence :D

    private bool renderMat;
    private bool showFurn;
    private bool showHuman;
    private bool enableFreeRoam;
    private bool pathedTeleport;
    

    // Use this for initialization
    void Start () {

        Cardboard.SDK.OnTrigger += TriggerPulled;
        renderMat = PlayerPrefs.GetInt("RenderMat") == 1;
        showFurn = PlayerPrefs.GetInt("ShowFurn") == 1;
        showHuman = PlayerPrefs.GetInt("ShowHuman") == 1;
        enableFreeRoam = PlayerPrefs.GetInt("EnableFreeRoam") == 1;
        pathedTeleport = PlayerPrefs.GetInt("PathedTeleport") == 1;
        if (PlayerPrefs.GetInt("LoadLocation") == 1){
            PlayerObj.transform.position = new Vector3(PlayerPrefs.GetFloat("LocationX"),
                PlayerPrefs.GetFloat("LocationY"), PlayerPrefs.GetFloat("LocationZ"));
            PlayerObj.transform.rotation = new Quaternion(
                PlayerPrefs.GetFloat("QuarternionX"), PlayerPrefs.GetFloat("QuarternionY"),
                PlayerPrefs.GetFloat("QuarternionZ"), PlayerPrefs.GetFloat("QuarternionW"));
            PlayerPrefs.SetInt("LoadLocation", 0);
        }
        ApplyFurnitureLayer();
        ApplyMaterialLayer();
        ApplyHumanLayer();
        ApplyMovements();
    }



    void TriggerPulled()
    {
        PlayerPrefs.SetInt("RenderMat", renderMat ? 1 : 0);
        PlayerPrefs.SetInt("ShowFurn", showFurn ? 1 : 0);
        PlayerPrefs.SetInt("ShowHuman", showHuman ? 1 : 0);
        PlayerPrefs.SetInt("EnableFreeRoam", enableFreeRoam ? 1 : 0);
        PlayerPrefs.SetInt("PathedTeleport", pathedTeleport ? 1 : 0);
        //location
        PlayerPrefs.SetInt("LoadLocation", 1);
        PlayerPrefs.SetFloat("LocationX", PlayerObj.transform.position.x);
        PlayerPrefs.SetFloat("LocationY", PlayerObj.transform.position.y);
        PlayerPrefs.SetFloat("LocationZ", PlayerObj.transform.position.z);
        PlayerPrefs.SetFloat("QuarternionX", PlayerObj.transform.Find("CardboardMain/Head").rotation.x);
        PlayerPrefs.SetFloat("QuarternionY", PlayerObj.transform.Find("CardboardMain/Head").rotation.y);
        PlayerPrefs.SetFloat("QuarternionZ", PlayerObj.transform.Find("CardboardMain/Head").rotation.z);
        PlayerPrefs.SetFloat("QuarternionW", PlayerObj.transform.Find("CardboardMain/Head").rotation.w);
        SceneManager.LoadScene("LayerScene", LoadSceneMode.Single);
    }

	// Update is called once per frame
	void Update () {

	}

    void ApplyMaterialLayer()
    {
        if (!renderMat)
        {
            GameObject[] objs = GameObject.FindObjectsOfType<GameObject>();
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i].isStatic)
                {
                    MeshRenderer[] mrs = objs[i].GetComponents<MeshRenderer>();
                    for (int j = 0; j < mrs.Length; j++)
                    {
                        mrs[j].material = new Material(Shader.Find("Diffuse"));
                    }
                }
            }
        }
    }

    void ApplyFurnitureLayer()
    {
        if (!showFurn)
        {
            FurnitureMain.SetActive(false);
        }
    }
    void ApplyHumanLayer()
    {
        ImportCsv Human = new ImportCsv(@"Assets/imported/humancoords.csv");
        for (int i = 0; i < Human.Count; i++)
        {
            HumanCoords.Add(new Vector4(Human.Itemf(i, 0), Human.Itemf(i, 1), Human.Itemf(i, 2), Human.Itemf(i, 3)));
        }
        if (showHuman)
        {
            for (int i = 0; i < HumanCoords.Count; i++)
            {
                //Duplicate Human Figures
                GameObject person = (GameObject)Instantiate(GameObject.Find("HumanFigure"), new Vector3(HumanCoords[i].x, 0.14f, HumanCoords[i].z), //"TODO" instantiating with y to be 0.14 and not coord.y as a quick fix
                    Quaternion.Euler(0.0f, HumanCoords[i].w, 0.0f));
                person.name = "Person" + i;
                //person.AddComponent<TooltipPopup>();

                //Match location coordinates with closest CFD data
                CFDClosestPt CFD = new CFDClosestPt(HumanCoords[i].x, HumanCoords[i].z);

                //Duplicate Cloth
                GameObject WindCloth = (GameObject)Instantiate(GameObject.Find("WindCloth"), new Vector3(HumanCoords[i].x, 0.638f, HumanCoords[i].z), Quaternion.Euler(270f, HumanCoords[i].w + 180f, 0.0f));
                WindCloth.name = "WindCloth" + i;
                WindCloth.GetComponent<Cloth>().externalAcceleration = new Vector3(CFD.Vx/2, CFD.Vz / 2, CFD.Vy / 2);
                WindCloth.GetComponent<Cloth>().randomAcceleration = new Vector3(CFD.Vx / 2, CFD.Vz / 2, CFD.Vy / 2);

                //Display Data in Tooltip
                GameObject Tooltip = (GameObject)Instantiate(GameObject.Find("Tooltip"), new Vector3(HumanCoords[i].x, 0.638f, HumanCoords[i].z), Quaternion.Euler(270f, HumanCoords[i].w + 180f, 0.0f));
                Tooltip.name = "Tooltip" + i;
                Tooltip.GetComponent<TextMesh>().text = "Wind Speed = " + CFD.V.ToString("F2") + " m/s";
            }
           
        }
    }
    void ApplyMovements()
    {
        if (enableFreeRoam)
        {
            PlayerObj.GetComponent<Navmesh>().setMovementMode(0);
            Canvas[] allCanvas = GameObject.FindObjectsOfType<Canvas>();
            for (int i = 0; i < allCanvas.Length; i++)
            {
                RadialProgressBar[] rpb = allCanvas[i].GetComponentsInChildren<RadialProgressBar>();
                if (rpb.Length > 0)
                {
                    allCanvas[i].enabled = false;
                }
            }
        }
        else
        {
            if (pathedTeleport)
            {
                PlayerObj.GetComponent<Navmesh>().setMovementMode(2);
            } else
            {
                PlayerObj.GetComponent<Navmesh>().setMovementMode(1);
            }
        }
    }
}
