using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LayerMenuController : MonoBehaviour {

    public GameObject canvas;
    public UnityEngine.UI.Toggle toggleMat;
    public UnityEngine.UI.Toggle toggleFurn;
    public UnityEngine.UI.Toggle toggleHuman;
    public UnityEngine.UI.Toggle toggleView;
    public UnityEngine.UI.Toggle toggleNoise;
    public UnityEngine.UI.Toggle toggleFreeRoam;

    private bool renderMat;
    private bool showFurn;
    private bool showHuman;
    private bool showView;
    private bool enableNoise;
    private bool enableFreeRoam;

	private float movementSpeed;
	private float lightIntensity;
    // Use this for initialization
    void Start () {
        renderMat = false;
        showFurn = false;
        showHuman = false;
        showView = false;
        enableNoise = false;
        enableFreeRoam = false;
        if (!PlayerPrefs.HasKey("RenderMat"))
        {
            //Read from Defaults
            ImportCsv Defaults = new ImportCsv(WWWLoader.active_download_path + "layersettings", ":");
            for (int i = 0; i < Defaults.Count; i++)
            {
                switch (Defaults.Item(i, 0))
                {
                    case "Material":
                        toggleMat.isOn = bool.Parse(Defaults.Item(i, 2));
                        toggleMat.enabled = bool.Parse(Defaults.Item(i, 1));
                        break;
                    case "Furniture":
                        toggleFurn.isOn = bool.Parse(Defaults.Item(i, 2));
                        toggleFurn.enabled = bool.Parse(Defaults.Item(i, 1));
                        break;
                    case "Human":
                        toggleHuman.isOn = bool.Parse(Defaults.Item(i, 2));
                        toggleHuman.enabled = bool.Parse(Defaults.Item(i, 1));
                        break;
                    case "FreeMovement":
                        toggleFreeRoam.isOn = bool.Parse(Defaults.Item(i, 2));
                        toggleFreeRoam.enabled = bool.Parse(Defaults.Item(i, 1));
                        break;
                    case "View":
                        toggleView.isOn = bool.Parse(Defaults.Item(i, 2));
                        toggleView.enabled = bool.Parse(Defaults.Item(i, 1));
                        break;
                    case "Noise":
                        toggleNoise.isOn = bool.Parse(Defaults.Item(i, 2));
                        toggleNoise.enabled = bool.Parse(Defaults.Item(i, 1));
                        break;
					case "MovementSpeed":
						movementSpeed = Defaults.Itemf (i, 1);
						PlayerPrefs.SetFloat ("MovementSpeed", movementSpeed);
						break;
					case "LightIntensity":
						lightIntensity = Defaults.Itemf (i, 1);
						PlayerPrefs.SetFloat ("LightIntensity", lightIntensity);
						break;
                    default:
                        break;
                }
            }
        }
        else
        {
            //read from PlayerPrefs
            if (PlayerPrefs.GetInt("RenderMat") == 1)
            {
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
            if (PlayerPrefs.GetInt("ShowView") == 1)
            {
                toggleView.isOn = PlayerPrefs.GetInt("ShowView") == 1;
            }
            if (PlayerPrefs.GetInt("EnableNoise") == 1)
            {
                toggleNoise.isOn = PlayerPrefs.GetInt("EnableNoise") == 1;
            }
            if (PlayerPrefs.GetInt("EnableFreeRoam") == 1)
            {
                toggleFreeRoam.isOn = PlayerPrefs.GetInt("EnableFreeRoam") == 1;
            }
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
    public void ToggleView()
    {
        showView = !showView;
    }
    public void ToggleNoise()
    {
        enableNoise = !enableNoise;
    }
    public void ToggleFreeRoam()
    {
        enableFreeRoam = !enableFreeRoam;
    }

    public void savePlayerPrefs()
    {
        PlayerPrefs.SetInt("RenderMat", renderMat ? 1 : 0);
        PlayerPrefs.SetInt("ShowFurn", showFurn ? 1 : 0);
        PlayerPrefs.SetInt("ShowHuman", showHuman ? 1 : 0);
        PlayerPrefs.SetInt("ShowView", showView ? 1 : 0);
        PlayerPrefs.SetInt("EnableNoise", enableNoise ? 1 : 0);
        PlayerPrefs.SetInt("EnableFreeRoam", enableFreeRoam ? 1 : 0);
    }

    public void BackToRoom()
    {
        savePlayerPrefs();
        SceneManager.LoadScene("Dynamic Scene",LoadSceneMode.Single);
    }

    public void BackToDownloadMenu()
    {
        savePlayerPrefs();
        SceneManager.LoadScene("DownloadMenuScene", LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update () {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(canvas.transform.position);
//		print (screenPoint);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        if (!onScreen)
        {
            canvas.transform.position = this.transform.position + this.transform.Find("Head").forward.normalized * 200;
			canvas.transform.rotation = Quaternion.LookRotation(canvas.transform.position-this.transform.position);
        }
		
	}
}
