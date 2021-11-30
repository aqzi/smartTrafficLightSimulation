using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public int time = 10; //traffic light changes each 30 seconds
    public TrafficLight light1; //light1 corresponds with the traffic light at road 1
    public TrafficLight light2;
    public TrafficLight light3;
    public TrafficLight light4;
    private bool changeLight = true;

    void Start() {
        light1.updateTrafficLight();
        light3.updateTrafficLight();

        if(light1.isTrafficLightActive() && light3.isTrafficLightActive()) 
        {
            if(GameEvents.current != null) GameEvents.current.activeTrafficLight(new List<int>() {1, 3});
        }
    }

    void Update()
    {
        if(changeLight) StartCoroutine(updateTrafficLight());
    }

    IEnumerator updateTrafficLight()
    {
        changeLight = false;

        yield return new WaitForSeconds(this.time);

        light1.updateTrafficLight();
        light2.updateTrafficLight();
        light3.updateTrafficLight();
        light4.updateTrafficLight();

        if(light1.isTrafficLightActive() && light3.isTrafficLightActive()) 
        {
            if(GameEvents.current != null) GameEvents.current.activeTrafficLight(new List<int>() {1, 3});
        }

        if(light2.isTrafficLightActive() && light4.isTrafficLightActive()) 
        {
            if(GameEvents.current != null) GameEvents.current.activeTrafficLight(new List<int>() {2, 4});
        }

        changeLight = true;
    }
}
