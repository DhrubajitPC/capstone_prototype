using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TooltipPopup : MonoBehaviour, ICardboardGazeResponder
{
    //TO BE UPDATED ONCE MOVE INTO OCULUS LIBRARY
    private bool visible = true;
    string game = "asdTooltip0";
    
    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update()
    {
        // this.game = "Tooltip" + this.gameObject.name.Replace("Person", "");
        GameObject.Find(this.game.Replace("asd", "")).GetComponent<Renderer>().enabled = visible;


    }

    public void OnGazeEnter()
    {
        visible = false;
    }

    public void OnGazeExit()
    {
        visible = true;
    }

    public void OnGazeTrigger()
    {
    }
}
