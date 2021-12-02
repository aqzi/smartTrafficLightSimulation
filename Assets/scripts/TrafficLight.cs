using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    private bool trafficLight = false;

    public void setLightColor(bool light)
    {
        this.trafficLight = light;
    }

    public bool isLightGreen()
    {
        return this.trafficLight;
    }
}
