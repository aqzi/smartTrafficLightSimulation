using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public int time = 10; //traffic light changes each 10 seconds
    public int timeOfOrange = 2;
    public TrafficLight light1; //light1 corresponds with the traffic light at road 1
    public TrafficLight light2;
    public TrafficLight light3;
    public TrafficLight light4;
    private bool changeLight = true;

    void Start() {
        light1.setLightColor(true);
        light3.setLightColor(true);

        if(light1.isLightGreen() && light3.isLightGreen()) 
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

        GameEvents.current.activeTrafficLight(new List<int>() {0, 0});

        yield return new WaitForSeconds(this.timeOfOrange);

        light1.setLightColor(!light1.isLightGreen());
        light2.setLightColor(!light2.isLightGreen());
        light3.setLightColor(!light3.isLightGreen());
        light4.setLightColor(!light4.isLightGreen());

        if(light1.isLightGreen() && light3.isLightGreen()) 
        {
            GameEvents.current.activeTrafficLight(new List<int>() {1, 3});
        }

        if(light2.isLightGreen() && light4.isLightGreen()) 
        {
            GameEvents.current.activeTrafficLight(new List<int>() {2, 4});
        }

        changeLight = true;
    }

    private void setChangeLight(bool value)
    {
        if(value != this.changeLight)
        {
            StartCoroutine(orange());
            this.changeLight = value;
        }
    }

    IEnumerator orange()
    {
        yield return new WaitForSeconds(this.timeOfOrange);

        GameEvents.current.activeTrafficLight(new List<int>() {0, 0});
    }
}
