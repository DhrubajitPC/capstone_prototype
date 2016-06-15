using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Navigation : MonoBehaviour
{

    public string[] locationNames;
    public Vector3[] locations;
    public float movementSpeed = 1;

    private Vector3 startingLocation;
    private Dictionary<string, Vector3> jumpLocations;
    private Rigidbody agent;
    private int movementType = 1; //0 is free roam, 1 teleport, 2 pathed teleport
    void Start()
    {
        agent = GetComponent<Rigidbody>();
        startingLocation = this.transform.position;
        jumpLocations = new Dictionary<string, Vector3>();
        for (int i = 0; i < Mathf.Min(locationNames.Length, locations.Length); i++)
        {
            jumpLocations.Add(locationNames[i], locations[i]);
        }
    }

    public void move(string location_name)
    {
        if (movementType == 1)
        {
            jump(location_name);
        }
        else if (movementType == 2)
        {
            setDestination(location_name);
        }
    }

    private void setDestination(string location_name)
    {
        if (jumpLocations.ContainsKey(location_name))
        {
            agent.MovePosition(jumpLocations[location_name]);
        }
        else
        {
            agent.MovePosition(startingLocation);
        }
    }

    private void jump(string location_name)
    {
        if (jumpLocations.ContainsKey(location_name))
        {
            this.transform.position = jumpLocations[location_name];
            Debug.Log(jumpLocations[location_name]);
        }
        else
        {
            this.transform.position = startingLocation;
        }
    }

    public void setMovementMode(int value)
    {
        movementType = value;
    }

    void Update()
    {
        if (movementType == 0 || Input.GetKey("space"))
        {
            agent.MovePosition(this.transform.position+transform.Find("VRMain/Head").transform.forward * Time.deltaTime * movementSpeed);
            //this.transform.position += transform.Find("VRMain/Head").transform.forward * Time.deltaTime * movementSpeed;
        }
    }
}