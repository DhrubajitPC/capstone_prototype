using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TooltipPopup : MonoBehaviour, ICardboardGazeResponder
{
    private string TextPPS;
    private string TextTemp;
    private string TextWind;
    private float FloatPMV;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setTooltip(string PPS, string Temp, string Wind, float PMV)
    {
        TextPPS = PPS;
        TextTemp = Temp;
        TextWind = Wind;
        FloatPMV = PMV;
    }

    public void OnGazeEnter()
    {
        GameObject.Find("Tooltip").GetComponent<Canvas>().enabled = true;
        GameObject.Find("PPS").GetComponent<Text>().text = this.TextPPS;
        GameObject.Find("Temp").GetComponent<Text>().text = this.TextTemp;
        GameObject.Find("Wind").GetComponent<Text>().text = this.TextWind;
        transform.Find("Glows").gameObject.SetActive(true);

        if (FloatPMV >= -0.5 && FloatPMV <=0.5)
        {
            GameObject.Find("PPS").GetComponent<Text>().color = new Color(0,0.5f,0);
        }

        else if (FloatPMV < -0.5 && FloatPMV >= -1)
        {
            GameObject.Find("PPS").GetComponent<Text>().color = new Color(0, 0.5f, 0.5f);
        }

        else if (FloatPMV > 0.5 && FloatPMV <= 1)
        {
            GameObject.Find("PPS").GetComponent<Text>().color = new Color(1f, 0.5f, 0);
        }

        else if (FloatPMV < -1)
        {
            GameObject.Find("PPS").GetComponent<Text>().color = new Color(0, 0, 0.5f);
        }

        else if (FloatPMV > 1)
        {
            GameObject.Find("PPS").GetComponent<Text>().color = new Color(0.5f, 0, 0);
        }


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
