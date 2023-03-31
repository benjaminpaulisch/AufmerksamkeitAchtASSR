//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using Assets.LSL4Unity.Scripts; // reference the LSL4Unity namespace to get access to all classes


public class TriggerInsideDetection : MonoBehaviour
{    
    public Manager manager;

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
        //only while experiment is running
        if (manager.GetBlockStarted())
        {
            //lsl marker
            manager.marker.Write(other.name + " entered " + this.gameObject.name);
            print(other.name + " entered " + this.gameObject.name);

            //only if this scipt is attached to the mid-point object
            if (gameObject.name.Contains("MidPoint"))
            {
                if (!manager.GetFirstTrialStarted())
                {
                    manager.StartTrial();
                }
                else
                {
                    manager.NextTrial();
                } 
            }   
        }


    }

    private void OnTriggerExit(Collider other)
    {
        //only while experiment is running
        if (manager.GetBlockStarted())
        {
            //lsl marker
            manager.marker.Write(other.name + " exited " + this.gameObject.name);
            print(other.name + " exited " + this.gameObject.name);
        }

    }

}
