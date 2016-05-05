using UnityEngine;
using System.Collections;

public class Navmesh : MonoBehaviour {

	NavMeshAgent agent;
	private float movementSpeed = 2;
	private Vector3 newRoom = new Vector3(-2.62f,2f,5.43f);
//	private Vector3 newRoom = new Vector3(4f,2f,1f);
	private Vector3 startingPosition = new Vector3(5.22f,2f,-4.5f);
	void Start () {
		agent = GetComponent<NavMeshAgent>();
	}

	//for auto walk
//	public void FixedUpdate(){
//		GameObject.Find ("Capsule").transform.position += GameObject.Find ("Head").transform.forward * Time.deltaTime * movementSpeed;
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