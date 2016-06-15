using UnityEngine;
using System.Collections;

public class HumanEmoticon : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //transform.Find("MoodCanvas").rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }

    public void setMood(int score)
    {
        transform.Find("MoodCanvas/HappyMood").gameObject.SetActive(false);
        transform.Find("MoodCanvas/GoodMood").gameObject.SetActive(false);
        transform.Find("MoodCanvas/BadMood").gameObject.SetActive(false);
        transform.Find("MoodCanvas/SadMood").gameObject.SetActive(false);
        string mood = "HappyMood";
        if (score < 25)
        {
            mood = "SadMood";
        } else if (score < 50)
        {
            mood = "BadMood";
        } else if (score < 75)
        {
            mood = "GoodMood";
        }
        transform.Find("MoodCanvas/"+mood).gameObject.SetActive(true);
    }
}
