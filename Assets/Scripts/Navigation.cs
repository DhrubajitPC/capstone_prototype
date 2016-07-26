using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Navigation : MonoBehaviour
{
    public float movementSpeed = 1;

    private Vector3 startingLocation;
    private Rigidbody agent;
    private int movementType = 1; //0 is free roam, 1 teleport, 2 pathed teleport


    void Start()
    {
        agent = GetComponent<Rigidbody>();
        startingLocation = this.transform.position;
    }

    public void move(Vector3 position)
    {
        if (movementType == 1)
        {
            jump(position);
        }
        else if (movementType == 2)
        {
            setDestination(position);
        }
    }

    private void setDestination(Vector3 position)
    {
        agent.MovePosition(position);
    }

    private void jump(Vector3 position)
    {
        this.transform.position = position;
    }

    public void setMovementMode(int value)
    {
        movementType = value;
    }


    void Update()
    {
		
		if (movementType == 0 || Input.GetKey("space"))
        {
			agent.MovePosition(this.transform.position+GameObject.Find("Head").transform.forward * Time.deltaTime * movementSpeed);
            //this.transform.position += transform.Find("VRMain/Head").transform.forward * Time.deltaTime * movementSpeed;
        }
    }
}