using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CamControl : MonoBehaviour {

	Scene scene;
	GameObject cam;
	GameObject capsule;
	GameObject canvas;

	bool gearMenu;

	private static bool spawned = false;
	// Use this for initialization
	void Start () {
		gearMenu = false;
		canvas = GameObject.Find ("Canvas");
		scene = SceneManager.GetActiveScene ();
		print (scene.name);
		cam = GameObject.Find ("VRMain");
//		print (scene.name);
		if (scene.name == "Gear Menu") {
			print ("Don't destroy on load");
			gearMenu = true;
//			GameObject[] cams = GameObject.FindGameObjectsWithTag ("Cam");
			DontDestroyOnLoad (cam);
//			if (cams.Length > 1) {
//				Destroy (cams [1]);	
//			}
			cam.GetComponent<LayerMenuController> ().enabled = false;
			cam.GetComponent<MeshCollider> ().enabled = false;
		} else if (scene.name == "LayerScene") {
			print ("lys");
			cam.GetComponent<LayerMenuController> ().enabled = true;
			cam.GetComponent<MeshCollider> ().enabled = true;
			DontDestroyOnLoad (cam);
		} else if (scene.name == "Dynamic Scene") {
			print ("dhns");
			capsule = GameObject.Find ("Capsule");
			cam.transform.parent = capsule.transform;
			cam.transform.localPosition = new Vector3 (0, 0, 0);
			cam.GetComponent<LayerMenuController> ().enabled = false;
			cam.GetComponent<MeshCollider> ().enabled = false;
			DontDestroyOnLoad (cam);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (gearMenu) {
			Vector3 screenPoint = Camera.main.WorldToViewportPoint (canvas.transform.position);
			bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
			if (!onScreen) {
				canvas.transform.position = cam.transform.position + cam.transform.Find ("Head").forward.normalized * 500;
				canvas.transform.rotation = Quaternion.LookRotation (canvas.transform.position - cam.transform.position);
			}
		}
	}
}
