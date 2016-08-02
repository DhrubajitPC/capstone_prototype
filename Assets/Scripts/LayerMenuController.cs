using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LayerMenuController : MonoBehaviour {

    GameObject canvas;
	Toggle toggleMat;
    Toggle toggleFurn;
    Toggle toggleHuman;
    Toggle toggleView;
    Toggle toggleNoise;
    Toggle toggleFreeRoam;
	Button viewButton;
	Button menuButton;


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
		toggleMat = GameObject.Find ("ToggleMaterial").GetComponent<UnityEngine.UI.Toggle>();
		toggleFurn = GameObject.Find ("ToggleFurniture").GetComponent<UnityEngine.UI.Toggle>();
		toggleHuman = GameObject.Find ("ToggleHuman").GetComponent<UnityEngine.UI.Toggle>();
		toggleView = GameObject.Find ("ToggleView").GetComponent<UnityEngine.UI.Toggle>();
		toggleNoise = GameObject.Find ("ToggleNoise").GetComponent<UnityEngine.UI.Toggle>();
		toggleFreeRoam = GameObject.Find ("ToggleMovement").GetComponent<UnityEngine.UI.Toggle>();

		canvas = GameObject.Find ("Canvas");

		viewButton = GameObject.Find ("ViewButton").GetComponent<Button>();
		menuButton = GameObject.Find ("MenuButton").GetComponent<Button>();
//		viewButton = GetComponent<Button> ();
//		menuButton = GetComponent<Button> ();

		viewButton.onClick.AddListener (() => BackToRoom ());
		menuButton.onClick.AddListener (() => BackToDownloadMenu ());

		toggleMat.onValueChanged.AddListener ((value) => {ToggleMat(value);});
		toggleFurn.onValueChanged.AddListener ((value) => {ToggleFurn(value);});
		toggleHuman.onValueChanged.AddListener ((value) => {ToggleHuman(value);});
		toggleView.onValueChanged.AddListener ((value) => {ToggleView(value);});
		toggleNoise.onValueChanged.AddListener ((value) => {ToggleNoise(value);});
		toggleFreeRoam.onValueChanged.AddListener ((value) => {ToggleFreeRoam(value);});

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
	
	void ToggleMat(bool val){
        renderMat = val;
    }
	void ToggleFurn(bool val)
    {
        showFurn = val;
    }
	void ToggleHuman(bool val)
    {
        showHuman = val;
    }
	void ToggleView(bool val)
    {
        showView = val;
    }
	void ToggleNoise(bool val)
    {
        enableNoise = val;
    }
	void ToggleFreeRoam(bool val)
    {
        enableFreeRoam = val;
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
		DestroyImmediate (this.gameObject);
        SceneManager.LoadScene("Gear Menu", LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update () {
		if (canvas==null)
			canvas = GameObject.Find ("Canvas");
		Vector3 screenPoint = Camera.main.WorldToViewportPoint(canvas.transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        if (!onScreen)
        {
            canvas.transform.position = this.transform.position + this.transform.Find("Head").forward.normalized * 200;
			canvas.transform.rotation = Quaternion.LookRotation(canvas.transform.position-this.transform.position);
        }
		
	}
}
