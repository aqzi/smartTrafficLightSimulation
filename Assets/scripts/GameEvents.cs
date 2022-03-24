using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake() {
        current = this;
    }

    public event Action<List<int>> onActiveTrafficLight;
    public void activeTrafficLight(List<int> activeLights)
    {
        if(onActiveTrafficLight != null)
        {
            onActiveTrafficLight(activeLights);
        }
    }

    //Ask opposite road if going left is allowed.
    public event Action<int, Guid> onGoLeft;
    public void goLeft(int roadNr, Guid id)
    {
        if(onGoLeft != null)
        {
            onGoLeft(roadNr, id);
        }
    }

    public event Action<int, Guid> onGoLeftAllowed;
    //roadNr is the road where a car is allowed to move left
    public void goLeftAllowed(int roadNr, Guid id)
    {
        if(onGoLeftAllowed != null)
        {
            onGoLeftAllowed(roadNr, id);
        }
    }

    public event Action onSaveSimulationToFile;
    //roadNr is the road where a car is allowed to move left
    public void saveSimulationToFile()
    {
        if(onSaveSimulationToFile != null)
        {
            onSaveSimulationToFile();
        }
    }

    public event Action onResultsRequest;
    //roadNr is the road where a car is allowed to move left
    public void resultsRequest()
    {
        if(onResultsRequest != null)
        {
            onResultsRequest();
        }
    }

    public event Action<string, int, string> onResultsReceive;
    //roadNr is the road where a car is allowed to move left
    public void resultsReceive(string leavingCars, int timesStopped, string carCounter)
    {
        if(onResultsReceive != null)
        {
            onResultsReceive(leavingCars, timesStopped, carCounter);
        }
    }

    public event Action onResultsRequestType2;
    //roadNr is the road where a car is allowed to move left
    public void resultsRequestType2()
    {
        if(onResultsRequestType2 != null)
        {
            onResultsRequestType2();
        }
    }

    public event Action<string> onResultsReceiveType2;
    //roadNr is the road where a car is allowed to move left
    public void resultsReceiveType2(string decisions)
    {
        if(onResultsReceiveType2 != null)
        {
            onResultsReceiveType2(decisions);
        }
    }

    public event Action<string> onDecision;
    //roadNr is the road where a car is allowed to move left
    public void decision(string msg)
    {
        if(onDecision != null)
        {
            onDecision(msg);
        }
    }
}
