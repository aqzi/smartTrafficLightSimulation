using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//This class is used to save some results of the simulation
public class Results : MonoBehaviour
{
    public string leavingCarsFilename = "leavingCars";
    private string startTime = "";

    // Start is called before the first frame update
    void Start()
    {
        if(GameEvents.current!= null) 
        {
            GameEvents.current.onLeavingCarsSend += onLeavingCarsSend;
            GameEvents.current.onLeavingCarsRequest += onLeavingCarsRequest;
        }
        this.startTime = string.Format("{0}:{1}:{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
    }

    public void onLeavingCarsSend(string leavingCars) 
    {
        StreamWriter writer = new StreamWriter(String.Format("Assets/Results/{0}.txt", this.leavingCarsFilename), true);
        writer.WriteLine(leavingCars);
        writer.Close();
    }

    public void onLeavingCarsRequest()
    {
        StreamWriter writer = new StreamWriter(String.Format("Assets/Results/{0}.txt", this.leavingCarsFilename), true);
        writer.WriteLine(this.startTime);
        writer.Close();
    }
}
