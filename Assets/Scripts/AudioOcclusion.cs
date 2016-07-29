using UnityEngine;
using System.Collections;

public class AudioOcclusion : MonoBehaviour {
    public float rolloff = 0.05f;
    public float rollofflimit = 0.3f;
    public float maxvolume = 1f;

    // Initial Settings
    void Start ()
    {
        GetComponent<AudioSource>().volume = maxvolume;
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().playOnAwake = true;
        GetComponent<AudioSource>().minDistance = 0f;
       
    }


	// Linecasting
	void Update () {
        Vector3 capsule = new Vector3(GameObject.Find("Capsule").transform.position.x, 1.6f, GameObject.Find("Capsule").transform.position.z);
       if (Physics.Linecast(transform.position, capsule))
        {
            if (GetComponent<AudioSource>().volume > rollofflimit * maxvolume)
            {
                GetComponent<AudioSource>().volume -= rolloff;
            }
            else { GetComponent<AudioSource>().volume = rollofflimit * maxvolume; }
        }

        else
        {
            if (GetComponent<AudioSource>().volume < maxvolume)
            {
                GetComponent<AudioSource>().volume += rolloff;
            }
            else { GetComponent<AudioSource>().volume = maxvolume; }
        }

    }
}
