using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TooltipPopup : MonoBehaviour, ICardboardGazeResponder
{
    private string tooltipText;
    
    // Use this for initialization
    void Start () {
        if (tooltipText == null) {
            tooltipText = this.name;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setTooltip(string text)
    {
        tooltipText = text;
    }

    public void OnGazeEnter()
    {
        GameObject.Find("Tooltip").GetComponent<Canvas>().enabled = true;
        GameObject.Find("TooltipText").GetComponent<Text>().text = this.tooltipText;
        transform.Find("Glows").gameObject.SetActive(true);
        //Debug.Log(this.tooltipText);
    }

    public void OnGazeExit()
    {
        GameObject.Find("Tooltip").GetComponent<Canvas>().enabled = false;
        transform.Find("Glows").gameObject.SetActive(false);
    }

    public void OnGazeTrigger()
    {
    }
}
