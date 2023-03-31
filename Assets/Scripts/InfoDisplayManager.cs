using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoDisplayManager : MonoBehaviour {

	public GameObject infoCanvas;
	public GameObject participantText;
	public GameObject conditionText;
	public GameObject headingText;
	public GameObject runNoText;
	public GameObject progressLabel;
	public GameObject progressText;
	public GameObject startMessage;
	public GameObject outsidePlankMessage;
	//public GameObject feetInsideCanvas;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void ShowDisplay()
    {
		if (!infoCanvas.activeSelf)
		{
			infoCanvas.SetActive(true);
		}
	}

	public void HideDisplay()
    {
		if (infoCanvas.activeSelf)
        {
			infoCanvas.SetActive(false);
        }
			
    }


	public void ShowStartMessage()
    {
		if (!startMessage.activeSelf)
			startMessage.SetActive(true);
    }


	public void HideStartMessage()
    {
		if (infoCanvas.activeSelf)
			startMessage.SetActive(false);
    }


	public void ShowOutsideMessage()
    {
		if (!outsidePlankMessage.activeSelf)
			outsidePlankMessage.SetActive(true);
    }


	public void HideOutsideMessage()
    {
		if (outsidePlankMessage.activeSelf)
			outsidePlankMessage.SetActive(false);
    }


	public void UpdateDisplay(string participant, string condition, string heading, string runNo, string progress)
	{
		participantText.GetComponent<Text>().text = participant;
		conditionText.GetComponent<Text>().text = condition;
		headingText.GetComponent<Text>().text = heading;
		runNoText.GetComponent<Text>().text = runNo;
		progressText.GetComponent<Text>().text = progress;

		if (condition.Contains("baseline"))
        {
			progressLabel.GetComponent<Text>().text = "time:";
        }
		else
        {
			progressLabel.GetComponent<Text>().text = "trial:";
        }	

	}

    /*
	public void UpdateFeetInsideDisplay(int feetInside)
    {
		feetInsideCanvas.GetComponentInChildren<Text>().text = "Feet inside plank: " + feetInside.ToString();

		if (feetInside < 2)
		{
			feetInsideCanvas.GetComponentInChildren<Text>().color = Color.yellow;
		}
		else if (feetInside == 2)
		{
			feetInsideCanvas.GetComponentInChildren<Text>().color = Color.green;
		}
		else if (feetInside > 2)
		{
			feetInsideCanvas.GetComponentInChildren<Text>().color = Color.red;
		}
	}*/

}
