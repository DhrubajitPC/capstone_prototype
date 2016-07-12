using UnityEngine;
using System.Collections;

public class AudioOcclusion : MonoBehaviour {
    private float rolloff = 0.05f;
    private float rollofflimit = 0.3f;

    // Initial Settings
    void Start ()
    {
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().playOnAwake = true;
        //GetComponent<AudioSource>().spatialBlend = 0.8f;
        GetComponent<AudioSource>().minDistance = 0f;
        //GetComponent<AudioSource>().maxDistance = 5f;
        GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;
    }


	// Linecasting
	void Update () {
        Vector3 capsule = new Vector3(GameObject.Find("Capsule").transform.position.x, 1.6f, GameObject.Find("Capsule").transform.position.z);
       if (Physics.Linecast(transform.position, capsule))
        {
            if (GetComponent<AudioSource>().volume > rollofflimit)
            {
                GetComponent<AudioSource>().volume -= rolloff;
            }
            else { GetComponent<AudioSource>().volume = rollofflimit; }
        }

        else
        {
            if (GetComponent<AudioSource>().volume < 1f)
            {
                GetComponent<AudioSource>().volume += rolloff;
            }
            else { GetComponent<AudioSource>().volume = 1f; }
        }

    }
}
