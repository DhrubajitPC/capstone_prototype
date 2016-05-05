using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Navmesh : MonoBehaviour {

    public string[] locationNames;
    public Vector3[] locations;
    public float movementSpeed = 2;

    private Vector3 startingLocation;
    private Dictionary<string, Vector3> jumpLocations;
	private NavMeshAgent agent;

    private bool autoWalk = false;
	void Start () {
		agent = GetComponent<NavMeshAgent>();
        startingLocation = this.transform.position;
        jumpLocations = new Dictionary<string, Vector3>();
        for (int i=0;i<Mathf.Min(locationNames.Length, locations.Length); i++)
        {
            jumpLocations.Add(locationNames[i], locations[i]);
        }
	}

	public void setDestination(string location_name){
        if (jumpLocations.ContainsKey(location_name))
        {
            agent.SetDestination(jumpLocations[location_name]);
        } else
        {
            agent.SetDestination(startingLocation);
        }
	}

    public void jump(string location_name)
    {
        if (jumpLocations.ContainsKey(location_name))
        {
            transform.position = jumpLocations[location_name];
        }
        else
        {
            transform.position = startingLocation;
        }
    }

    public void SetAutoWalk(bool value)
    {
        autoWalk = value;
    }

    void Update()
    {
        if (autoWalk || Input.GetKey("space"))
        {
            this.transform.position += transform.Find("CardboardMain/Head").transform.forward * Time.deltaTime * movementSpeed;
        }
    }
}