using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RadialProgressBar : MonoBehaviour {


	public Transform LoadingBar;

	[SerializeField] private float currentAmount;
	[SerializeField] private float speed;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (currentAmount < 100) {
			currentAmount += speed * Time.deltaTime;
		}
		LoadingBar.GetComponent<Image> ().fillAmount = currentAmount / 100;
	
	}
}
