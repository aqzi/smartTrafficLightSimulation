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
}
