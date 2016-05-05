using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RadialProgressBar : MonoBehaviour, ICardboardGazeResponder {


	public Transform LoadingBar;
	public string NewRoom;

	[SerializeField] private float currentAmount;
	[SerializeField] private float speed;
	bool gazing = false;
//	player = GameObject.FindGameObjectWithTag(Tags.player).transform;
//	private Transform camera = GameObject.FindGameObjectWithTag("MainCamera").transform;

	// Use this for initialization
	void Start () {
//		print (GameObject.Find ("CardboardMain").transform.position);
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
			GameObject.Find ("CardboardMain").GetComponent <Jump>().move (NewRoom);
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
