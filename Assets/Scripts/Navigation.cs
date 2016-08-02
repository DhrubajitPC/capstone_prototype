using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Navigation : MonoBehaviour
{
	public float movementSpeed;
	public float lightIntensity;

    private Vector3 startingLocation;
    private Rigidbody agent;
    private int movementType = 1; 

    void Start()
    {
		lightIntensity = PlayerPrefs.GetFloat ("LightIntensity");
		movementSpeed = PlayerPrefs.GetFloat ("MovementSpeed");
        agent = GetComponent<Rigidbody>();
        startingLocation = this.transform.position;
		RenderSettings.ambientIntensity = lightIntensity;
		RenderSettings.ambientLight = Color.white;
    }

    public void move(Vector3 position)
    {
        if (movementType == 1)
        {
            jump(position);
        }
	}

    private void jump(Vector3 position)
    {
        this.transform.position = position;
    }

    public void setMovementMode(int value)
    {
        movementType = value;
    }

	private void disableHumanCollider(GameObject human){
		Collider[] col = human.GetComponentsInChildren<Collider> ();
		foreach (Collider c in col) {
			c.enabled = false;
		}
	}

	private void enableHumanCollider(GameObject human){
		Collider[] col = human.GetComponentsInChildren<Collider> ();
		foreach (Collider c in col) {
			c.enabled = true;
		}
	}

    void Update()
    {
		GameObject[] human = GameObject.FindGameObjectsWithTag ("Human");
		foreach (GameObject h in human) {
			float distFromHuman = Vector3.Distance (h.transform.position, transform.position);
			print (distFromHuman);
			if (distFromHuman < 1.48f) {
				disableHumanCollider (h);
			} else {
				enableHumanCollider (h);
			}
		}
		
		if (movementType == 0 || Input.GetKey("space"))
        {
			agent.MovePosition(this.transform.position+GameObject.Find("Head").transform.forward * Time.deltaTime * movementSpeed);
        }
    }
}