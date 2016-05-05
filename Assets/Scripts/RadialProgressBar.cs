using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RadialProgressBar : MonoBehaviour, ICardboardGazeResponder {


	public Transform LoadingBar;
	public string NewRoom;
	public int style; //movement syle: 1 for jump, 2 for walk

	private float currentAmount = 0;
	private float speed = 50;
	private bool gazing = false;

    void Start()
    {

    }

	// Update is called once per frame
	void Update () {
		if (gazing) {
			if (currentAmount < 100) {
				currentAmount += speed * Time.deltaTime;
			}
		}
		LoadingBar.GetComponent<Image> ().fillAmount = currentAmount / 100;
		if (currentAmount >= 100) {
			switch (style) {
			case 1:
				GameObject.Find ("Capsule").GetComponent <Navmesh> ().jump (NewRoom);
				break;
			case 2:
				GameObject.Find ("Capsule").GetComponent<Navmesh> ().setDestination(NewRoom);
				break;
			default:
				GameObject.Find ("Capsule").GetComponent <Navmesh> ().jump (NewRoom);
				break;
			}
		}
	}

	public void OnGazeEnter(){
		gazing = true;
	}

	public void OnGazeExit(){
		gazing = false;
		currentAmount = 0;
	}

	public void OnGazeTrigger(){
	}
}
