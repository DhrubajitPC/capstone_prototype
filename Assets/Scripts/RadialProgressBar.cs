using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RadialProgressBar : MonoBehaviour, ICardboardGazeResponder {

    public RoomControl roomControl;

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
		this.transform.Find("LoadingBar").GetComponent<Image>().fillAmount = currentAmount / 100;
		if (currentAmount >= 100) {
            GameObject.Find("Capsule").GetComponent<Navigation>().move(transform.parent.position);
            if (roomControl != null)
                roomControl.rotateMovementCanvases(transform.parent.position);
        }
	}

	public void OnGazeEnter(){
		gazing = true;
        ((Behaviour)transform.GetComponent("Halo")).enabled = true;
	}

	public void OnGazeExit(){
		gazing = false;
		currentAmount = 0;
        ((Behaviour)transform.GetComponent("Halo")).enabled = false;
    }

	public void OnGazeTrigger(){
	}
}
