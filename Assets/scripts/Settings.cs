using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public enum Format { RAW, JPG, PNG, PPM, CSV };
    public enum DatasetType {TRAIN, TEST};

    public static Settings current;
    public bool generateData;
    public string path;
    public Format format = Format.JPG;
    public DatasetType datasetType = DatasetType.TRAIN; //Is the data to train a model or to test it?
    private int counter; //used to name generated data

    private void Awake() {
        current = this;
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
}
