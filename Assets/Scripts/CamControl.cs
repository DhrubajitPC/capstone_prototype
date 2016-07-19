using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CamControl : MonoBehaviour {

	Scene scene;
	GameObject cam;
	GameObject capsule;

	private void makeCapsuleParent(){
		if (scene.name != "LayerScene") {
			capsule = GameObject.Find ("Capsule");
			cam.transform.parent = capsule.transform;
			cam.GetComponent<LayerMenuController> ().enabled = false;
			cam.GetComponent<MeshCollider> ().enabled = false;
		}
	}
	// Use this for initialization
	void Awake () {
		scene = SceneManager.GetActiveScene ();
		cam = GameObject.Find ("VRMain");
		print (scene.name);
		if(scene.name == "LayerScene"){
			DontDestroyOnLoad (cam);	
		}

		makeCapsuleParent ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
