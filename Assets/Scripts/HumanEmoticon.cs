using UnityEngine;
using System.Collections;

public class HumanEmoticon : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Find("MoodCanvas").rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }

    public void setMood(float score)
    {

        string mood = "mood0";
        if (score >= -0.5 && score <= 0.5)
        {
            mood = "mood0";
        } else if (score < -0.5 && score >= -1)
        {
            mood = "mood1c";
        } else if (score < -1 && score >= -2)
        {
            mood = "mood2c";
        } else if (score < -2 && score >= -3)
        {
            mood = "mood3c";
        }
        else if (score < -3)
        {
            mood = "mood4c";
        }



        else if (score > 0.5 && score <= 1)
        {
            mood = "mood1h";
        }
        else if (score < 2 && score >= 1)
        {
            mood = "mood2h";
        }
        else if (score < 3 && score >= 2)
        {
            mood = "mood3h";
        }
        else if (score > 3)
        {
            mood = "mood4h";
        }

        transform.Find("MoodCanvas/"+mood).gameObject.SetActive(true);
    }
}
