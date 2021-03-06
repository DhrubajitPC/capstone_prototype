﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
//using UnityEditor;

public class RoomControl : MonoBehaviour {

    public GameObject PlayerObj;

    private bool loadRotation = true;

    private List<Vector4> HumanCoords = new List<Vector4>(); //w is y rotation
    private Vector3[] jumpLocations;
    private List<Canvas> movementCanvases = new List<Canvas>();
    AssetBundle assetBundle;
    private GameObject BaseGeometry;
    private GameObject FurnitureGeometry;
    private GameObject ViewGeometry;

    private bool renderMat;
    private bool showFurn;
    private bool showHuman;
    private bool showView;
    private bool enableNoise;
    private bool enableFreeRoam;

	private bool singleTap = false;
	private float tapTime = 0.3f;
	private float lastTap = 0;
    
    // Use this for initialization
    void Start () {
        Cardboard.SDK.OnTrigger += TriggerPulled;
        StartCoroutine(initializeDynamicScene());
    }

    void TriggerPulled()
    {
		if (!singleTap) {
            //single tap
			singleTap = true;
			StartCoroutine (GoToLayerScene());
		}

        //Double Tap
        ToggleFreeRoam();
    }

	private IEnumerator GoToLayerScene(){
		yield return new WaitForSeconds (tapTime);
		if (singleTap) {

            PlayerPrefs.SetInt("RenderMat", renderMat ? 1 : 0);
            PlayerPrefs.SetInt("ShowFurn", showFurn ? 1 : 0);
            PlayerPrefs.SetInt("ShowHuman", showHuman ? 1 : 0);
            PlayerPrefs.SetInt("ShowView", showView ? 1 : 0);
            PlayerPrefs.SetInt("EnableNoise", enableNoise ? 1 : 0);
            PlayerPrefs.SetInt("EnableFreeRoam", enableFreeRoam ? 1 : 0);
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
            singleTap = false;
		}
	}

    private void ToggleFreeRoam()
    {
        if ((Time.time - lastTap) < tapTime)
        {
			singleTap = false;
            enableFreeRoam = !enableFreeRoam;
        }

        if (enableFreeRoam)
        {
			foreach (var o in movementCanvases) {
				o.enabled = false;
			}
            PlayerObj.GetComponent<Navigation>().setMovementMode(0);
        }
        else {
			foreach (var o in movementCanvases) {
				o.enabled = true;
			}
			rotateMovementCanvases(transform.position);
            PlayerObj.GetComponent<Navigation>().setMovementMode(1);
        }
        lastTap = Time.time;
    }

	// Update is called once per frame
	void Update () {

	}

    private IEnumerator initializeDynamicScene()
    {
        renderMat = PlayerPrefs.GetInt("RenderMat") == 1;
        showFurn = PlayerPrefs.GetInt("ShowFurn") == 1;
        showHuman = PlayerPrefs.GetInt("ShowHuman") == 1;
        showView = PlayerPrefs.GetInt("ShowView") == 1;
        enableNoise = PlayerPrefs.GetInt("EnableNoise") == 1;
        enableFreeRoam = PlayerPrefs.GetInt("EnableFreeRoam") == 1;

        yield return StartCoroutine(loadAssetBundle());//wait for this coroutine to finish

        try
        {
            ApplyGeometryAndFurnitureLayer();
            ApplyHumanLayer();
            ApplyViewLayer();
            ApplyNoise();
            ApplyMovements();
            unloadAssetBundle();
        } catch (Exception e)
        {
            GameObject.Find("ERROR").GetComponent<UnityEngine.UI.Text>().text = e.Message;
        }

        //Setup player location and orientation
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
        else
        {
            PlayerObj.transform.position = jumpLocations[0];
        }
        rotateMovementCanvases(transform.position);

        yield return 1;
    }

    private IEnumerator loadAssetBundle()
    {
        yield return null;
        string file_path = WWWLoader.active_download_path + "renderbundle";

        assetBundle = AssetBundle.LoadFromFile(file_path);
        if (assetBundle != null)
        {
            GameObject.Find("ERROR").GetComponent<UnityEngine.UI.Text>().text = "LOADED ";
            FurnitureGeometry = assetBundle.LoadAsset<GameObject>("Furniture.prefab");
            BaseGeometry = assetBundle.LoadAsset<GameObject>("Walls.prefab");
            ViewGeometry = assetBundle.LoadAsset<GameObject>("View.prefab");
        }
        else
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

    void ApplyGeometryAndFurnitureLayer()
    {
        GameObject baseGeometry;
        if (showFurn)
        {
            baseGeometry = FurnitureGeometry;
        } else
        {
            baseGeometry = BaseGeometry;
        }
        GameObject geom = (GameObject)Instantiate(baseGeometry,
                baseGeometry.transform.position,
                baseGeometry.transform.rotation);
        geom.name = "BaseGeometryLayer";
        geom.transform.localScale = baseGeometry.transform.localScale;
        MeshRenderer[] mats = geom.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer m in mats)
        {
            m.material.shader = Shader.Find("Custom/DoubleSidedCutout");
        }

        if (!renderMat)
        {
            GameObject.Find("Lights/DirectionalLight").GetComponent<Light>().enabled = true;
            MeshRenderer[] mrs = geom.GetComponentsInChildren<MeshRenderer>();
            for (int j = 0; j < mrs.Length; j++)
            {
                mrs[j].material = Resources.Load<Material>("material/NoMaterial");
            }
        }
    }

    void ApplyViewLayer()
    {
        if (showView)
        {
            GameObject view = (GameObject)Instantiate(ViewGeometry,
                    ViewGeometry.transform.position,
                    ViewGeometry.transform.rotation);
            view.name = "ViewLayer";
            view.transform.localScale = ViewGeometry.transform.localScale;
            MeshRenderer[] mats = view.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer m in mats)
            {
                m.material.shader = Shader.Find("Custom/DoubleSidedCutout");
            }
        }
    }

    void ApplyHumanLayer()
    {
        ImportCsv Human = new ImportCsv(WWWLoader.active_download_path + "humancoords");
        for (int i = 0; i < Human.Count; i++)
        {
            HumanCoords.Add(new Vector4(Human.Itemf(i, 0), Human.Itemf(i, 1), Human.Itemf(i, 2), Human.Itemf(i, 3)));
        }
        if (showHuman)
        {	
			List<GameObject> hPrefab = new List<GameObject> ();
			hPrefab.Add((GameObject)Resources.Load ("prefabs/Human Prefabs/HumanFig"));
			hPrefab.Add((GameObject)Resources.Load ("prefabs/Human Prefabs/HumanFig (2)"));
			hPrefab.Add((GameObject)Resources.Load ("prefabs/Human Prefabs/HumanFig (3)"));
			hPrefab.Add((GameObject)Resources.Load ("prefabs/Human Prefabs/HumanFig (4)"));
			hPrefab.Add((GameObject)Resources.Load ("prefabs/Human Prefabs/HumanFig (5)"));

            ImportCsv cfdData = new ImportCsv(WWWLoader.active_download_path + "cfd");
            ImportCsv cfdOrigin = new ImportCsv(WWWLoader.active_download_path + "cfdorigin");

            for (int i = 0; i < HumanCoords.Count; i++)
            {
                //"TODO" instantiating with y to be 0.14 and not coord.y as a quick fix
				int index = UnityEngine.Random.Range(0,hPrefab.Count);
				GameObject person = (GameObject)Instantiate(hPrefab[index],
                    new Vector3(HumanCoords[i].x, HumanCoords[i].y, HumanCoords[i].z),
                    Quaternion.Euler(0.0f, HumanCoords[i].w, 0.0f));
                person.name = "Person" + i;

                //Match location coordinates with closest CFD data
                CFDClosestPt CFD = new CFDClosestPt(HumanCoords[i].x, HumanCoords[i].z, cfdData, cfdOrigin);
                Vector3 extWind = new Vector3(CFD.Vx * 0.8f, CFD.Vz * 0.8f, CFD.Vy * 0.8f);
                Vector3 ranWind = new Vector3(CFD.Vx * 0.4f, CFD.Vz * 0.4f, CFD.Vy * 0.4f);

                //Cloth Setting
				GameObject WindCloth = person.transform.Find("Skirt (1)").gameObject;
                //GameObject WindCloth = (GameObject)Instantiate(GameObject.Find("WindCloth"), new Vector3(HumanCoords[i].x, 0.638f, HumanCoords[i].z), Quaternion.Euler(270f, HumanCoords[i].w + 180f, 0.0f));
                WindCloth.GetComponent<Cloth>().externalAcceleration = extWind * 6;
				WindCloth.GetComponent<Cloth>().randomAcceleration = ranWind * 6;

				GameObject Hair = person.transform.Find("Hair (2)").gameObject;
                //GameObject WindCloth = (GameObject)Instantiate(GameObject.Find("WindCloth"), new Vector3(HumanCoords[i].x, 0.638f, HumanCoords[i].z), Quaternion.Euler(270f, HumanCoords[i].w + 180f, 0.0f));
                Hair.GetComponent<Cloth>().externalAcceleration = extWind * 8;
                Hair.GetComponent<Cloth>().randomAcceleration = ranWind * 8;

				GameObject Hair2 = person.transform.Find("Hair (3)").gameObject;
                //GameObject WindCloth = (GameObject)Instantiate(GameObject.Find("WindCloth"), new Vector3(HumanCoords[i].x, 0.638f, HumanCoords[i].z), Quaternion.Euler(270f, HumanCoords[i].w + 180f, 0.0f));
                Hair2.GetComponent<Cloth>().externalAcceleration = extWind * 8;
                Hair2.GetComponent<Cloth>().randomAcceleration = ranWind * 8;

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
            ImportCsv loc = new ImportCsv(WWWLoader.active_download_path + "jumplocations");
            jumpLocations = new Vector3[loc.Count];
            for (int i=0; i < loc.Count; i++)
            {
                jumpLocations[i] = new Vector3(loc.Itemf(i, 0), loc.Itemf(i, 1), loc.Itemf(i, 2));
            }

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
            PlayerObj.GetComponent<Navigation>().setMovementMode(1);

        }
    }
    void ApplyNoise()
    {
        ImportCsv Noise = new ImportCsv(WWWLoader.active_download_path + "noisecoords");
        List<Vector4> NoiseCoords = new List<Vector4>();
        for (int i = 0; i < Noise.Count; i++)
        {
			NoiseCoords.Add(new Vector4(Noise.Itemf(i, 0), Noise.Itemf(i, 1), Noise.Itemf(i, 2), Noise.Itemf(i, 3)));
        }

        if (enableNoise)
        {
            for (int i = 0; i < NoiseCoords.Count; i++)
            {
				string path = "prefabs/NoisePresets/";
				switch ((int) NoiseCoords [i].w) {
				case 4:
					path += "NoiseSourceEnclosed";
					break;
				case 3:
					path += "NoiseSourceTelevision";
					break;
				case 2:
					path += "NoiseSourceSeaside";
					break;
				case 1:
					path += "NoiseSourceNeighbourhood";
					break;
				default:
					path += "NoiseSourceCity";
					break;
				}

                //"TODO" instantiating with y to be 0.14 and not coord.y as a quick fix
                GameObject person = (GameObject)Instantiate(Resources.Load(path),
                    new Vector3(NoiseCoords[i].x, NoiseCoords[i].y, NoiseCoords[i].z),
                    Quaternion.identity);
                person.name = "Noise" + i;
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
