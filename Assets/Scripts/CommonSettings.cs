using UnityEngine;
using System.Collections;

public class CommonSettings : MonoBehaviour {

	// Use this for initialization
	void Start () {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 60;
    }
	
	// Update is called once per frame
	void Update () {
        // might want to set targetFrameRate again here
    }
}
