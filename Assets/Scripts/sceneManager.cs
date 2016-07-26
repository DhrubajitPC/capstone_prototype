using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
//using UnityEditor;


public class sceneManager : MonoBehaviour {

	public void loadLayerScene(){
		SceneManager.LoadScene("LayerScene");
	}
}
