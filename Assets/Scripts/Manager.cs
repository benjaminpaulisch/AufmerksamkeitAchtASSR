//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.LSL4Unity.Scripts; // reference the LSL4Unity namespace to get access to all classes
using Valve.VR;
using System.Collections;

public class Manager : MonoBehaviour {

    //public interface:
    [Header("Object References")]
    
    public LSLMarkerStream marker;
    public InfoDisplayManager infoManager;

    [Header("Baseline configuration")]
    public float baselineDuration = 60f;    //seconds


    //program control and status:
    private int programStatus;
    private int expBlockRunNo = 0;
    private int currentBlockRunNo = 0;
    private float currentTime;
    private bool baselineStarted = false;
    private bool baselineEnd = false;
    private int baselineRunNo = 0;
    private int baselineAssrRunNo = 0;


    //paricipant inputs:
    private string participantID = "";
    private bool idSet = false;
    private int participantAge = 0;
    private bool ageSet = false;
    private string participantSex = "?";
    private bool sexSet = false;
    private string participantGamingExp = "?";
    private bool gamingSet = false;
    private bool configComplete = false;


    //other vars:
    private string tempMarkerText;


    //UI gameobjects
    private GameObject buttonExpBlockA, buttonPlankRoof, buttonStartBlockA, buttonStartBlockB, buttonStartBaseline, buttonStartBaselineASSR, textMissingInputs;
    private GameObject inputParticipantID, inputParticipantAge, inputParticipantSex, inputParticipantGamingExp;
    private GameObject mainMenu, configMenu, canvasBlackScreen, blackScreenImage;



    // Use this for initialization
    void Start () 
    {
        //find game objects
        
        buttonStartBlockA = GameObject.Find("ButtonStartBlockA");
        buttonStartBlockB = GameObject.Find("ButtonStartBlockB");
        buttonStartBaseline = GameObject.Find("ButtonStartBaseline");
        buttonStartBaselineASSR = GameObject.Find("ButtonStartBaselineASSR");
        textMissingInputs = GameObject.Find("TextMissingInputs");
        inputParticipantID = GameObject.Find("InputParticipantID");
        inputParticipantAge = GameObject.Find("InputParticipantAge");
        inputParticipantSex = GameObject.Find("DropdownParticipantSex");
        inputParticipantGamingExp = GameObject.Find("DropdownParticipantGaming");
        mainMenu = GameObject.Find("CanvasMainMenu");
        configMenu = GameObject.Find("CanvasConfigMenu");
        canvasBlackScreen = GameObject.Find("CanvasBlackScreen");
        blackScreenImage = GameObject.Find("BlackScreenImage");


        //infoManager.UpdateFeetInsideDisplay(paradigm.GetInsidePlankCounter());

        StartMainMenu();
        
	}


    private void StartMainMenu()
    {
        programStatus = 0;

        mainMenu.SetActive(true);
        configMenu.SetActive(false);
        infoManager.HideDisplay();
        infoManager.HideStartMessage();
        infoManager.HideOutsideMessage();

     

    }


    public void StartConfiguration()
    {
        //This method is called from a button in the main menu

        programStatus = 1;

        mainMenu.SetActive(false);
        configMenu.SetActive(true);

    }

    /*
    public void StartCalibration()
    {
        //This method is called from a button in the main menu

        //programStatus = 2;

        CalibratePlank();

    } */


    public void StartExpBlock(string heading)
    {
        //This method is called from a button in the main menu

        programStatus = 3;
        mainMenu.SetActive(false);
        infoManager.ShowDisplay();


        if (heading == "A")
        {
            expBlockRunNo += 1;
            currentBlockRunNo = expBlockRunNo;

            
        }
        else if (heading == "B")
        {
            expBlockRunNo += 1;
            currentBlockRunNo = expBlockRunNo;

            
        }


        //write block start event
        //marker.Write("plank:start;location:" + plankPosition + ";runNo:" + tempNo.ToString());
        //Debug.Log("plank:start;location:" + plankPosition + ";runNo:" + tempNo.ToString());

        //participant infos:
        tempMarkerText ="participantID:" + participantID + ";" +
                        "participantAge:" + participantAge + ";" +
                        "participantSex:" + participantSex + ";" +
                        "participantGamingExp:" + participantGamingExp;
        marker.Write(tempMarkerText);
        Debug.Log(tempMarkerText);

        //set info display
        //infoManager.UpdateDisplay("experiment", plankPosition, tempNo.ToString(), "");
        //infoManager.UpdateDisplay(participantID, "experiment", plankPosition, currentBlockRunNo.ToString(), "");

        

    }


    public void StartBaseline(bool assr)
    {
        //This method is called from a button in the main menu

        programStatus = 4;
        baselineEnd = false;
        currentTime = 0;

        mainMenu.SetActive(false);
        infoManager.ShowDisplay();

        if (!assr)
        {
            baselineRunNo += 1;
            currentBlockRunNo = baselineRunNo;

            
        }
        else
        {
            baselineAssrRunNo += 1;
            currentBlockRunNo = baselineAssrRunNo;

            
        }
        

        //write baseline start marker
        tempMarkerText =
            "baseline:start;" +
            //"location:" + plankPosition + ";" +
            "runNo:" + currentBlockRunNo.ToString() + ";" +
            "duration:" + baselineDuration.ToString();

        marker.Write(tempMarkerText);
        Debug.Log(tempMarkerText);
        

        //set info display
        //infoManager.UpdateDisplay(participantID, "baseline", plankPosition, currentBlockRunNo.ToString(), "");

    }


    public void BackToMainMenu()
    {
        //this is called when pressing the "back to main menu" button in the configuration menu

        //save data from the inputs:

        //participantID
        if (inputParticipantID.GetComponent<InputField>().text != "")
        {
            idSet = true;
            participantID = inputParticipantID.GetComponent<InputField>().text;
        }
        else
            idSet = false;

        //participantAge
        if (inputParticipantAge.GetComponent<InputField>().text != "")
        {
            /*
            try
            {
                participantAge = int.Parse(inputParticipantAge.GetComponent<InputField>().text);
                ageSet = true;
            }
            catch (System.FormatException e)
            {
                marker.Write("FormatException: invalid input value for participant age. " + e.ToString());
                Debug.Log("FormatException: invalid input value for participant age.");
                Debug.LogException(e);
                participantAge = 0;
                inputParticipantAge.GetComponent<InputField>().text = "";
                ageSet = false;
            }*/

            participantAge = int.Parse(inputParticipantAge.GetComponent<InputField>().text);
            ageSet = true;
        }
        else
            ageSet = false;

        //participantSex
        if (!inputParticipantSex.GetComponent<Dropdown>().options[inputParticipantSex.GetComponent<Dropdown>().value].text.Equals("?"))
        {
            sexSet = true;
            participantSex = inputParticipantSex.GetComponent<Dropdown>().options[inputParticipantSex.GetComponent<Dropdown>().value].text;
        }
        else
            sexSet = false;

        //participantGamingExp
        if (!inputParticipantGamingExp.GetComponent<Dropdown>().options[inputParticipantGamingExp.GetComponent<Dropdown>().value].text.Equals("?"))
        {
            gamingSet = true;
            participantGamingExp = inputParticipantGamingExp.GetComponent<Dropdown>().options[inputParticipantGamingExp.GetComponent<Dropdown>().value].text;
        }
        else
            gamingSet = false;

        /*
        Debug.Log("participantID: " + participantID);
        Debug.Log("participantAge: " + participantAge.ToString());
        Debug.Log("participantSex: " + participantSex);
        Debug.Log("participantGamingExp: " + participantGamingExp);
        */

        //check if config is complete
        if (idSet && ageSet && sexSet && gamingSet)
        {
            configComplete = true;
        }
        else
        {
            configComplete = false;
        }

        //go back to main menu
        StartMainMenu();

    }


    // Update is called once per frame
    void Update () {

        try
        {
            switch (programStatus)
            {
                case 0: //main menu
                    {
                        //check if all inputs in Configuration have been given and alle calibration was made
                        if (configComplete)
                        {
                            buttonStartBaseline.GetComponent<Button>().interactable = true;
                            buttonStartBaselineASSR.GetComponent<Button>().interactable = true;
                            buttonStartBlockA.GetComponent<Button>().interactable = true;
                            buttonStartBlockB.GetComponent<Button>().interactable = true;
                            
                            textMissingInputs.SetActive(false);
                        }
                        else
                        {
                            buttonStartBaseline.GetComponent<Button>().interactable = false;
                            buttonStartBaselineASSR.GetComponent<Button>().interactable = false;
                            buttonStartBlockA.GetComponent<Button>().interactable = false;
                            buttonStartBlockB.GetComponent<Button>().interactable = false;

                            textMissingInputs.SetActive(true);
                        }
                        break;
                    }

                case 1: //configuration
                    {
                        break;
                    }
                case 2: //calibration
                    {
                        //set position of WalkSpace and City


                        break;
                    }
                case 3: //experiment
                    {
                        //check for abort by pressing the escape key
                        if (Input.GetKeyDown("escape"))
                        {
                            //abort paradigm:
                            //paradigm.AbortParadigm();

                            //marker.Write("experiment:abort");
                            //Debug.Log("experiment:abort");

                            //go to main menu
                            StartMainMenu();
                            
                        }
                        /*else
                        {
                            //check for space bar press to start the paradigm
                            if (!spacebarPressed)
                            {
                                //check if participant is inside plank
                                if (!paradigm.IsInsidePlank()) 
                                {
                                    //show message that participant is not inside the plank
                                    infoManager.ShowOutsideMessage();
                                    infoManager.HideStartMessage();
                                }
                                else
                                {
                                    //hide message that participant is not inside the plank
                                    infoManager.HideOutsideMessage();
                                    
                                    if (Input.GetKeyDown(KeyCode.Space))
                                    {                                
                                        spacebarPressed = true;
                                    
                                        //deactivate message to press space bar
                                        infoManager.HideStartMessage();

                                        //start paradigm
                                        paradigm.StartParadigm(participantID, plankPosition, currentBlockRunNo);
                                    
                                    }
                                    else
                                    {
                                        //display message to press space bar
                                        infoManager.ShowStartMessage();
                                    }
                                } 

                            }
                            else
                            {
                                //check paradigm status
                                if (!paradigm.GetParadigmStarted())
                                {
                                    //go to main menu
                                    StartMainMenu();
                                }
                            }                                   

                        }*/
                        break;
                    }
                case 4: //baseline
                    {
                        //check for abort by pressing the escape key
                        if (Input.GetKeyDown("escape"))
                        {
                            marker.Write("baseline:abort");
                            Debug.Log("baseline:abort");

                            baselineStarted = false;

                            //go to main menu
                            StartMainMenu();
                        }
                        else
                        {
                            //Run baseline
                            if (!baselineEnd)
                            {
                                currentTime += Time.deltaTime;

                                if (currentTime > baselineDuration)
                                {
                                    baselineEnd = true;

                                    //end of baseline
                                    marker.Write("baseline:end");
                                    Debug.Log("baseline:end");

                                    //play audio
                                    //baselineEndSound.volume = 1f;
                                    //baselineEndSound.Play();

                                    //go to main menu
                                    StartMainMenu();
                                }
                                else
                                {
                                    /*update desktop info texts
                                    int tempNo;
                                    if (plankPosition == "ground")
                                    {
                                        tempNo = baselineRunNo;
                                    }
                                    else
                                    {
                                        tempNo = baselineAssrRunNo;
                                    }
                                    infoManager.UpdateDisplay(participantID, "baseline", plankPosition, tempNo.ToString(), string.Format("{0}:{1:00}", (int)currentTime / 60, (int)currentTime % 60));
                                    */
                                }
                            }
                        }
                        break;
                    }
            }

        }
        catch (System.Exception e)  //catch errors and log them and write them to lsl stream, then throw the exception again
        {
            marker.Write(e.ToString());
            Debug.LogError(e);
            throw (e);
        }

    }
}
