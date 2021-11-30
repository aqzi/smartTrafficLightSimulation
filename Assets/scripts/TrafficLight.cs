using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    private bool trafficLight = false;

    public void updateTrafficLight()
    {
        this.trafficLight = !this.trafficLight;
    }

    public bool isTrafficLightActive()
    {
        return this.trafficLight;
    }
}
