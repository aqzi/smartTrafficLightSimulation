using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//This class is used to save some results of the simulation
public class Results : MonoBehaviour
{
    private string filename = "results";
    private string startTime = "";
    private string type = "normal";
    private List<string> timesStopped = new List<string>();
    private List<string> leavingCars = new List<string>();
    private List<string> carCounter = new List<string>();
    private string decisions = "";

    // Start is called before the first frame update
    void Start()
    {
        if(GameEvents.current!= null) 
        {
            GameEvents.current.onResultsRequest += onResultsRequest;
            GameEvents.current.onResultsReceive += onResultsReceive;
            GameEvents.current.onResultsReceiveType2 += onResultsReceiveType2;
        }

        if(Settings.current != null) 
        {
            this.type = Settings.current.datasetType.ToString().ToLower();
            this.filename = Settings.current.resultsFilename;
        }

        this.startTime = string.Format("{0}:{1}:{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
    }

    void Update()
    {
        if(timesStopped.Count == 4 && leavingCars.Count == 5 && carCounter.Count == 4)
        {
            Settings.Mode mode = Settings.current.mode;
            if(mode == Settings.Mode.SMART && this.decisions != "")
            {
                writeToFile(String.Format("Assets/Results/{0}.txt", this.filename + "_smart"));
                this.timesStopped.Clear();
                this.leavingCars.Clear();
            }

            if(mode == Settings.Mode.NORMAL)
            {
                writeToFile(String.Format("Assets/Results/{0}.txt", this.filename + "_normal"));
                this.timesStopped.Clear();
                this.leavingCars.Clear();
            }
        }
    }

    public void onResultsRequest()
    {
        this.leavingCars.Add(this.startTime);
    }

    public void onResultsReceive(string leavingCars, int timesStopped, string carCounter)
    {
        this.leavingCars.Add(leavingCars);
        this.timesStopped.Add(timesStopped.ToString());
        this.carCounter.Add(carCounter);
    }

    public void onResultsReceiveType2(string decisions)
    {
        this.decisions = decisions;
    }

    private void writeToFile(string path)
    {
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("\nexperiment 1:");
        writer.WriteLine(string.Join(". ", this.carCounter.ToArray()));
        if(Settings.current.mode == Settings.Mode.SMART) writer.WriteLine(this.decisions);
        writer.WriteLine("\nexperiment 2:");
        writer.WriteLine(string.Join(", ", this.timesStopped.ToArray()));
        writer.Close();
    }
}
