using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public enum Format { RAW, JPG, PNG, PPM, CSV };
    public enum DatasetType {TRAIN, TEST, OTHER}; //other is for testing in dev mode
    public enum Mode {NORMAL, SMART}; //mode 'smart' will run with smart traffic lights

    public static Settings current;
    public bool generateData = true;
    public string path;
    public Format format = Format.JPG;
    public DatasetType datasetType = DatasetType.TRAIN; //Is the data to train a model or to test it?
    public Mode mode = Mode.NORMAL;
    //Total lifetime of the simulation in seconds
    //floor(100, 3) = 33 --> 33*4 generated images --> uses 33*4*16Kb of memory (only for those images)
    public int time = 100;
    public bool saveSimulationToFile = false;
    public bool loadSimulationFromFile = false;
    private int counter; //used to name generated data

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

        yield return new WaitForSeconds(1);
        
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
