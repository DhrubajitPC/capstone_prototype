using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Jump : MonoBehaviour {

	private Vector3 startingPostition;
	// Use this for initialization
	void Start () {
		startingPostition = transform.localPosition;
	}

	void Update(){
		print (startingPostition);
		if (Input.GetButtonDown("Fire1")){
			transform.localPosition = new Vector3 (4f, 1.7f, 5f);
		}
	}

}
