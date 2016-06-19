using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class RoomControl : MonoBehaviour {

    public GameObject PlayerObj;
    public Vector3[] jumpLocations;

    private const string download_url = "https://luccan.github.io/capstone_prototype_assetbundle/renderbundle";
    private bool loadRotation = true;

    private List<Vector4> HumanCoords = new List<Vector4>(); //w is y rotation
    private List<Canvas> movementCanvases = new List<Canvas>();
    AssetBundle assetBundle;
    private GameObject BaseGeometry;
    private GameObject Furniture;

    private bool renderMat;
    private bool showFurn;
    private bool showHuman;
    private bool enableFreeRoam;
    private bool pathedTeleport;
    

    // Use this for initialization
    void Start () {
        StartCoroutine(initializeDynamicScene());
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
        if (loadRotation)
        {
            PlayerPrefs.SetFloat("QuarternionX", PlayerObj.transform.Find("VRMain/Head").rotation.x);
            PlayerPrefs.SetFloat("QuarternionY", PlayerObj.transform.Find("VRMain/Head").rotation.y);
            PlayerPrefs.SetFloat("QuarternionZ", PlayerObj.transform.Find("VRMain/Head").rotation.z);
            PlayerPrefs.SetFloat("QuarternionW", PlayerObj.transform.Find("VRMain/Head").rotation.w);
        }
        SceneManager.LoadScene("LayerScene", LoadSceneMode.Single);
    }

	// Update is called once per frame
	void Update () {

	}

    private IEnumerator initializeDynamicScene()
    {
        renderMat = PlayerPrefs.GetInt("RenderMat") == 1;
        showFurn = PlayerPrefs.GetInt("ShowFurn") == 1;
        showHuman = PlayerPrefs.GetInt("ShowHuman") == 1;
        enableFreeRoam = PlayerPrefs.GetInt("EnableFreeRoam") == 1;
        pathedTeleport = PlayerPrefs.GetInt("PathedTeleport") == 1;
        if (PlayerPrefs.GetInt("LoadLocation") == 1)
        {
            PlayerObj.transform.position = new Vector3(PlayerPrefs.GetFloat("LocationX"),
                PlayerPrefs.GetFloat("LocationY"), PlayerPrefs.GetFloat("LocationZ"));
            if (loadRotation)
            {
                //this is reset by cardboard/gear. Apply fix pls
                PlayerObj.transform.Find("VRMain/Head").rotation = new Quaternion(
                    PlayerPrefs.GetFloat("QuarternionX"), PlayerPrefs.GetFloat("QuarternionY"),
                    PlayerPrefs.GetFloat("QuarternionZ"), PlayerPrefs.GetFloat("QuarternionW"));
            }
            PlayerPrefs.SetInt("LoadLocation", 0);
        }
        yield return StartCoroutine(loadAssetBundle(download_url, 1)); //wait for this coroutine to finish
        //loadAssetBundle(download_url, 1);
        //yield return new WaitUntil(() => assetBundle != null);
        ApplyGeometryLayer();
        ApplyFurnitureLayer();
        ApplyMaterialLayer();
        ApplyHumanLayer();
        ApplyMovements();
        unloadAssetBundle();

        Cardboard.SDK.OnTrigger += TriggerPulled;

        yield return 1;
    }

    private IEnumerator loadAssetBundle(string url, int version)
    {
        // wait for the caching system to be ready
        while (!Caching.ready)
            yield return null;

        WWW www;
        try {
            // load AssetBundle file from Cache if it exists with the same version or download and store it in the cache
            www = WWW.LoadFromCacheOrDownload(url, version);
            //yield return www;

            Debug.Log("Loaded ");

            if (www.error != null)
            {
                GameObject.Find("ERROR").GetComponent<UnityEngine.UI.Text>().text = www.error;
            }
        } catch (Exception e)
        {
            throw new Exception("WWW download had an error: " + e.Message);
        }

        assetBundle = www.assetBundle;
        /*
        string path = "Assets/AssetBundleFiles/Windows/";
#if UNITY_ANDROID
        path = Application.dataPath + "!assets/AssetBundleFiles/Android/";
#endif
        GameObject.Find("ERROR").GetComponent<UnityEngine.UI.Text>().text = "Loading AssetBundle for " + path;
        AssetBundle assetBundle = AssetBundle.LoadFromFile(path + "renderbundle");*/
        if (assetBundle != null)
        {
            GameObject.Find("ERROR").GetComponent<UnityEngine.UI.Text>().text = "LOADED ";
            Furniture = assetBundle.LoadAsset<GameObject>("FurnitureMain.prefab");
            BaseGeometry = assetBundle.LoadAsset<GameObject>("Duxton Render.prefab");
        } else
        {
            GameObject.Find("ERROR").GetComponent<UnityEngine.UI.Text>().text = "FAIL TO LOAD ";
        }
    }

    void unloadAssetBundle()
    {
        // Unload the AssetBundles compressed contents to conserve memory
        if (assetBundle != null)
            assetBundle.Unload(false);
    }

    void ApplyGeometryLayer()
    {
        if (true)
        {
            GameObject geometry = (GameObject)Instantiate(BaseGeometry, 
                Vector3.zero, Quaternion.Euler(0.0f,-180.0f,0.0f)); //fix with standardized coor
            geometry.name = "BaseGeometryLayer";
        }
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
        if (showFurn)
        {
            GameObject furniture = (GameObject)Instantiate(Furniture, 
                new Vector3(6.252522f, 2.140625f, 3.574341f), //fix with standardized coor
                //Vector3.zero, 
                Quaternion.identity);
            furniture.name = "FurnitureLayer";
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
                //"TODO" instantiating with y to be 0.14 and not coord.y as a quick fix
                GameObject person = (GameObject)Instantiate(Resources.Load("prefabs/HumanFig"),
                    new Vector3(HumanCoords[i].x, 0.14f, HumanCoords[i].z),
                    Quaternion.Euler(0.0f, HumanCoords[i].w, 0.0f));
                person.name = "Person" + i;

                //Match location coordinates with closest CFD data
                CFDClosestPt CFD = new CFDClosestPt(HumanCoords[i].x, HumanCoords[i].z);
                Vector3 extWind = new Vector3(CFD.Vx * 0.8f, CFD.Vz * 0.8f, CFD.Vy * 0.8f);
                Vector3 ranWind = new Vector3(CFD.Vx * 0.4f, CFD.Vz * 0.4f, CFD.Vy * 0.4f);

                //Cloth Setting
                GameObject WindCloth = person.transform.Find("dress").gameObject;
                //GameObject WindCloth = (GameObject)Instantiate(GameObject.Find("WindCloth"), new Vector3(HumanCoords[i].x, 0.638f, HumanCoords[i].z), Quaternion.Euler(270f, HumanCoords[i].w + 180f, 0.0f));
                WindCloth.GetComponent<Cloth>().externalAcceleration = extWind;
                WindCloth.GetComponent<Cloth>().randomAcceleration = ranWind;

                GameObject Hair = person.transform.Find("hair1").gameObject;
                //GameObject WindCloth = (GameObject)Instantiate(GameObject.Find("WindCloth"), new Vector3(HumanCoords[i].x, 0.638f, HumanCoords[i].z), Quaternion.Euler(270f, HumanCoords[i].w + 180f, 0.0f));
                Hair.GetComponent<Cloth>().externalAcceleration = extWind * 2;
                Hair.GetComponent<Cloth>().randomAcceleration = ranWind * 2;

                GameObject Hair2 = person.transform.Find("hair2").gameObject;
                //GameObject WindCloth = (GameObject)Instantiate(GameObject.Find("WindCloth"), new Vector3(HumanCoords[i].x, 0.638f, HumanCoords[i].z), Quaternion.Euler(270f, HumanCoords[i].w + 180f, 0.0f));
                Hair2.GetComponent<Cloth>().externalAcceleration = extWind;
                Hair2.GetComponent<Cloth>().randomAcceleration = ranWind;

                person.GetComponent<TooltipPopup>().setTooltip(CFD.PPS.ToString("F0"), CFD.T.ToString("F1"), CFD.V.ToString("F2"), CFD.PMV);

                //set mood based on PMV here
                //random from 0-100 inclusive
                person.GetComponent<HumanEmoticon>().setMood(CFD.PMV);

            }
        }
    }
    void ApplyMovements()
    {
        if (enableFreeRoam)
        {
            PlayerObj.GetComponent<Navigation>().setMovementMode(0);
        }
        else
        {
            //create jumpProgressBars as necessary
            for (int i = 0; i < jumpLocations.Length; i++)
            {
                GameObject canvas = (GameObject)Instantiate(Resources.Load("prefabs/MovementCanvas"),
                    new Vector3(jumpLocations[i].x, jumpLocations[i].y, jumpLocations[i].z), 
                    Quaternion.identity);
                canvas.GetComponentInChildren<RadialProgressBar>().roomControl = this;
                movementCanvases.Add(canvas.GetComponent<Canvas>());
            }
            rotateMovementCanvases(transform.position);
            if (pathedTeleport)
            {
                PlayerObj.GetComponent<Navigation>().setMovementMode(2);
            } else
            {
                PlayerObj.GetComponent<Navigation>().setMovementMode(1);
            }
        }
    }

    //rotates all canvases to face targetLocation. Also hides the canvas if it is too close to current position.
    public void rotateMovementCanvases(Vector3 targetLocation)
    {
        for (int i = 0; i < movementCanvases.Count; i++)
        {
            //if too near
            if ((movementCanvases[i].transform.position- targetLocation).magnitude < 1.5f){
                movementCanvases[i].enabled = false;
            }
            else {
                movementCanvases[i].enabled = true;
                movementCanvases[i].transform.rotation = Quaternion.
                    LookRotation(movementCanvases[i].transform.position - targetLocation);
            }
        }
    }
}
