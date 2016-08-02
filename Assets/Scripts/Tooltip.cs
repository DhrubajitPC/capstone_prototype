using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tooltip : MonoBehaviour
{
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        transform.position = Camera.main.transform.position + (Camera.main.transform.forward * 0.4f);
    }
}
