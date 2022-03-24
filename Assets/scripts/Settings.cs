using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public enum Format { RAW, JPG, PNG, PPM, CSV };
    public enum DatasetType {TRAIN, TEST, OTHER}; //other is for testing in dev mode
    public enum Mode {NORMAL, SMART}; //mode 'smart' will run with smart traffic lights

    public static Settings current;

    [Header("synthetic data settings")]
    public bool generateData = true;
    public string path;
    public Format format = Format.JPG;
    public DatasetType datasetType = DatasetType.TRAIN; //Is the data to train a model or to test it?
    public int counter; //used to name generated data

    [Header("execution settings")]
    public Mode mode = Mode.NORMAL;
    //Total lifetime of the simulation in seconds
    //floor(100, 3) = 33 --> 33*4 generated images --> uses 33*4*16Kb of memory (only for those images)
    public int time = 100;

    [Header("store/load simulation settings")]
    public bool saveSimulationToFile = false;
    public bool loadSimulationFromFile = false;
    public string simulationFilename = "simulation";

    [Header("spawn time settings: Random[min, max[")]
    public int min = 5;
    public int max = 25;

    [Header("store results")]
    public string resultsFilename = "results";

    public void Awake() 
    {
        current = this;
    }

    public void Start()
    {
        StartCoroutine(quitApplication(this.time));
    }

    //counter should increase each time an object calls it
    public int getCounter()
    {
        int tmp = counter;
        counter++;
        return tmp;
    }

    public bool checkFormat(Format format2)
    {
        return format == format2;
    }

    IEnumerator quitApplication(int time)
    {
        yield return new WaitForSeconds(time);

        GameEvents.current.saveSimulationToFile();
        GameEvents.current.resultsRequest();
        GameEvents.current.resultsRequestType2();

        yield return new WaitForSeconds(1); //wait a sec until events are finished
        
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
