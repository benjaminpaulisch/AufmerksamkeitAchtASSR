//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.LSL4Unity.Scripts; // reference the LSL4Unity namespace to get access to all classes
using Valve.VR;
using System.Collections;

public class Manager : MonoBehaviour {

    //public interface:
    [Header("Baseline configuration")]
    public float baselineDuration = 60f;    //seconds

    [Header("Experiment configuration")]
    public int trialsPerBlock = 60;

    [Header("Audio configuration")]
    public float frequency1;
    public float frequency2;
    public float sampleRate = 44100;
    public float waveLengthInSeconds = 2.0f;
    public int percentVolume = 100;

    [Header("Object References")]    
    public LSLMarkerStream_ExpEvents marker;
    public InfoDisplayManager infoManager;     

    [Header("Debug")]
    public bool debugMode;


    //program control and status:
    private int programStatus;
    private int expBlockRunNo = 0;
    private int currentBlockRunNo = 0;
    private bool blockStarted = false;
    private bool blockEnd = false;
    private string currentCondition;
    private float currentTime;
    private bool baselineStarted = false;
    private bool baselineEnd = false;
    private int baselineRunNo = 0;
    private int baselineAssrRunNo = 0;
    private int currentTrialNo = 0;
    private bool firstTrialStarted = false;
    private string currentHeading;

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


    //audio
    private AudioSource audioSource;
    private int audioTimeIndex = 0;



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

        //initialize audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0; //force 2D sound
        audioSource.Stop(); //avoids audiosource from starting to play automatically


        StartMainMenu();
        
	}


    //public interface:
    public bool GetBlockStarted()
    {
        return blockStarted;
    }

    public bool GetBaselineStarted()
    {
        return baselineStarted;
    }

    public bool GetFirstTrialStarted()
    {
        return firstTrialStarted;
    }



    private void StartMainMenu()
    {
        programStatus = 0;

        mainMenu.SetActive(true);
        configMenu.SetActive(false);
        infoManager.HideDisplay();
        infoManager.HideStartMessage();
        infoManager.HideOutsideMessage();

        canvasBlackScreen.SetActive(false);

        //stop audio if its running
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioTimeIndex = 0;  //resets timer
        }                                                

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
        canvasBlackScreen.SetActive(false);

        currentHeading = heading;
        currentTrialNo = 0;
        blockStarted = true;
        firstTrialStarted = false;


        /*
        if (heading == "A")
        {
            expBlockRunNo += 1;
            currentBlockRunNo = expBlockRunNo;
            
        }
        else if (heading == "B")
        {
            expBlockRunNo += 1;
            currentBlockRunNo = expBlockRunNo;
            
        }*/
        expBlockRunNo += 1;
        currentBlockRunNo = expBlockRunNo;
        currentCondition = "experiment";


        //start audio
        audioTimeIndex = 0;  //resets timer before playing sound
        audioSource.Play();


        //write block start event
        marker.Write("expBlock:start;heading:" + currentHeading + ";runNo:" + currentBlockRunNo.ToString());
        Debug.Log("expBlock:start;heading:" + currentHeading + ";runNo:" + currentBlockRunNo.ToString());

        //participant infos:
        tempMarkerText ="participantID:" + participantID + ";" +
                        "participantAge:" + participantAge + ";" +
                        "participantSex:" + participantSex + ";" +
                        "participantGamingExp:" + participantGamingExp;
        marker.Write(tempMarkerText);
        Debug.Log(tempMarkerText);

        //set info display
        infoManager.UpdateDisplay(participantID, currentCondition, currentHeading, expBlockRunNo.ToString(), "");             

    }


    public void StartBaseline(bool assr)
    {
        //This method is called from a button in the main menu

        programStatus = 4;
        baselineStarted = true;
        baselineEnd = false;
        currentTime = 0;

        mainMenu.SetActive(false);
        infoManager.ShowDisplay();
        canvasBlackScreen.SetActive(true);

        string strASSR;

        if (!assr)
        {
            baselineRunNo += 1;
            currentBlockRunNo = baselineRunNo;
            strASSR = "no";
            currentCondition = "baseline";
            
        }
        else
        {
            baselineAssrRunNo += 1;
            currentBlockRunNo = baselineAssrRunNo;
            strASSR = "yes";
            currentCondition = "baselineASSR";
            
        }
        

        //write baseline start marker
        tempMarkerText =
            "baseline:start;" +
            "assr:" + strASSR + ";" +
            "runNo:" + currentBlockRunNo.ToString() + ";" +
            "duration:" + baselineDuration.ToString();

        Debug.Log(tempMarkerText);
        marker.Write(tempMarkerText);

        //set info display
        infoManager.UpdateDisplay(participantID, currentCondition, "", currentBlockRunNo.ToString(), "");

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


    private void runExperimentBlock()
    {
        //ToDo ?

        //check if audio is not running -> restart it
        if (!audioSource.isPlaying)
        {
            audioTimeIndex = 0;  //resets timer before playing sound
            audioSource.Play();
        }

    }

    private void EndExperimentBlock()
    {
        //play end sound:
        //PlaySound("endCondition");

        //stop audio        
        audioSource.Stop();
        audioTimeIndex = 0;  //resets timer


        //lsl marker
        marker.Write("expBlock:end");
        Debug.Log("exüBlock:end");

        //go to main menu
        StartMainMenu();
    }

    public void StartTrial()
    {
        currentTrialNo += 1;
        firstTrialStarted = true;


        //lsl marker
        tempMarkerText =
                "trial:start" + ";" +
                "trialNo:" + currentTrialNo.ToString() + ";" +
                "heading:" + currentHeading.ToString();

        marker.Write(tempMarkerText);
        Debug.Log(tempMarkerText);

        //set info display
        infoManager.UpdateDisplay(participantID, currentCondition, currentHeading, currentBlockRunNo.ToString(), currentTrialNo.ToString());


    }


    public void NextTrial()
    {
        //end current trial
        tempMarkerText =
            "trial:end;" +
            "trialNo:" + currentTrialNo.ToString();

        marker.Write(tempMarkerText);
        Debug.Log(tempMarkerText);


        //check if all trials have been run
        if (currentTrialNo < trialsPerBlock)
        {
            //start next trial
            StartTrial();
            
        }
        else
        {
            //end of block
            EndExperimentBlock();
        }

    }



    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            data[i] = CreateSine(audioTimeIndex, frequency1, sampleRate, percentVolume);

            if (channels == 2)
                data[i + 1] = CreateSine(audioTimeIndex, frequency2, sampleRate, percentVolume);

            audioTimeIndex++;

            //if timeIndex gets too big, reset it to 0
            if (audioTimeIndex >= (sampleRate * waveLengthInSeconds))
            {
                audioTimeIndex = 0;
            }
        }
    }

    //Creates a sinewave
    public float CreateSine(int timeIndex, float frequency, float sampleRate, int percentVolume)
    {
        return Mathf.Sin(2 * Mathf.PI * timeIndex * frequency / sampleRate )*percentVolume/100;
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
                        if (configComplete || debugMode)
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
                            marker.Write("experiment:abort");
                            Debug.Log("experiment:abort");

                            //go to main menu
                            StartMainMenu();
                            
                        }
                        else
                        {
                            /*check for space bar press to start the paradigm
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
                            }*/


                            //run experiment
                            runExperimentBlock();
                        }
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
                                    baselineStarted = false;

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
                                    //update desktop info texts
                                    int tempNo;
                                    if (currentCondition.Contains("ASSR"))
                                    {
                                        tempNo = baselineRunNo;
                                    }
                                    else
                                    {
                                        tempNo = baselineAssrRunNo;
                                    }
                                    infoManager.UpdateDisplay(participantID, currentCondition, "", tempNo.ToString(), string.Format("{0}:{1:00}", (int)currentTime / 60, (int)currentTime % 60));
                                    
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
