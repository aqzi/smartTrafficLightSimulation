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
}
