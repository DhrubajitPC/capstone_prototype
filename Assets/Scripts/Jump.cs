using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour {

	private Vector3 startingPosition = new Vector3(5.22f,2f,-4.5f);
	private Vector3 newRoom = new Vector3(-2.62f,2f,5.43f);
	
	public void move(string pos){
		switch(pos){
			case "new room":
				GameObject.Find ("CardboardMain").transform.position = newRoom;
				break;
			default:
				GameObject.Find ("CardboardMain").transform.position = startingPosition;
				break;
		}
	}
}
