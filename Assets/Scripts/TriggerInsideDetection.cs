//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using Assets.LSL4Unity.Scripts; // reference the LSL4Unity namespace to get access to all classes


public class TriggerInsideDetection : MonoBehaviour
{
    //public OddballParadigma paradigm;
    public LSLMarkerStream marker;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnTriggerEnter(Collider other)
    {

        //check if paradigm is active
        //if(paradigm.GetParadigmStarted())
        //{
            //lsl marker
            marker.Write(other.name + " entered plank");
            print(other.name + " entered plank");

            //only increment if feet collided
            if (other.name.Contains("Shoe"))
            {
                //paradigm.IncrementInsidePlankCounter();
            }

        //}

    }

    private void OnTriggerExit(Collider other)
    {

        //check if paradigm is active
        //if (paradigm.GetParadigmStarted())
        //{
            //lsl marker
            marker.Write(other.name + " exited plank");
            print(other.name + " exited plank");

            //only increment if feet collided
            if (other.name.Contains("Shoe"))
            {
                //paradigm.DecrementInsidePlankCounter();
            }

        //}

    }

}
