using UnityEngine;
using System.Collections;

public class Navmesh : MonoBehaviour {

	NavMeshAgent agent;
//	Vector3 pos = new Vector3(3.9f,1f,6f);
	private Vector3 newRoom = new Vector3(-2.62f,2f,5.43f);
	private Vector3 startingPosition = new Vector3(5.22f,2f,-4.5f);
	void Start () {
		agent = GetComponent<NavMeshAgent>();
	}

	// Update is called once per frame
//	void Update () {
//		if ( Input.GetMouseButtonDown(1) ) {
//				agent.SetDestination(pos);
//		}
//	}

	public void move(string pos){
		switch (pos) {
		case "new room":
			agent.SetDestination (newRoom);
			break;
		default:
			agent.SetDestination (startingPosition);
			break;
		}
	}
}