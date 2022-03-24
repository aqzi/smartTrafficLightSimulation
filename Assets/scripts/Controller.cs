using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public enum ReactionTime {NORMAL, FAST};
    public int time = 10; //traffic light changes each 10 seconds
    public int timeOfOrange = 2;
    public TrafficLight light1; //light1 corresponds with the traffic light at road 1
    public TrafficLight light2;
    public TrafficLight light3;
    public TrafficLight light4;
    private bool changeLight = true;
    private float timer = 0.0f;
    private string decision = "same";
    private float counter = 0.0f;


    void Start() {
        light1.setLightColor(true);
        light3.setLightColor(true);

        if(light1.isLightGreen() && light3.isLightGreen()) 
        {
            if(GameEvents.current != null)
            {
                GameEvents.current.activeTrafficLight(new List<int>() {1, 3});
            }
        }

        if(GameEvents.current != null) GameEvents.current.onDecision += onDecision;
        if(Settings.current != null)
        {
            if(Settings.current.mode == Settings.Mode.SMART) this.changeLight = false;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        counter += Time.deltaTime;

        if(Settings.current != null)
        {
            if(Settings.current.mode == Settings.Mode.NORMAL)
            {
                if(changeLight) StartCoroutine(updateTrafficLight());
            } else
            {
                if(!this.changeLight)
                {
                    if(this.timer > this.time)
                    {
                        this.changeLight = true;
                        this.timer = 0.0f;
                        GameEvents.current.activeTrafficLight(new List<int>() {0, 0});
                    }
                } else 
                {
                    if(this.timer > this.timeOfOrange || this.decision.Contains("fast"))
                    {
                        if(this.decision == "same")
                        {
                            this.setTrafficLightColors(
                                !light1.isLightGreen(), !light2.isLightGreen(), 
                                !light3.isLightGreen(), !light4.isLightGreen()
                            );
                        } else
                        {
                            if(this.decision.Contains("1") && this.decision.Contains("3"))
                            {
                                this.setTrafficLightColors(true, false, true, false);
                            }

                            if(this.decision.Contains("2") && this.decision.Contains("4"))
                            {
                                this.setTrafficLightColors(false, true, false, true);
                            }
                        }

                        this.notifyTrafficLights();

                        this.changeLight = false;
                        this.timer = 0.0f;
                    }
                }
            }
        }
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

        this.notifyTrafficLights();

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

    public void onDecision(string msg)
    {
        if(this.decision != msg) this.counter = 0.0f; //reset counter

        if(this.decision != msg && msg != "same")
        {
            this.changeLight = true;
            this.timer = 0.0f;
            GameEvents.current.activeTrafficLight(new List<int>() {0, 0});
        }

        this.decision = msg;

        if(msg == "same") this.time = 10;

        // if(msg == "road_1_3_green" || msg == "road_2_4_green" ||
        //     msg == "road_1_3_green_fast" || msg == "road_2_4_green_fast")
        // {
        //     this.changeLight = true;
        //     this.timer = 0.0f;
        //     GameEvents.current.activeTrafficLight(new List<int>() {0, 0});
        // }
    }

    // private Tuple<int, int> getRoads(string msg)
    // {
    //     string[] msgList = msg.Split('_');
    //     if(msgList.Length < 3) throw new Exception("Msg is invalid");
    //     return new Tuple<int, int>(Int16.Parse(msgList[1]), Int16.Parse(msgList[2]));
    // }

    private void notifyTrafficLights()
    {
        if(light1.isLightGreen() && light3.isLightGreen()) 
        {
            GameEvents.current.activeTrafficLight(new List<int>() {1, 3});
        }

        if(light2.isLightGreen() && light4.isLightGreen()) 
        {
            GameEvents.current.activeTrafficLight(new List<int>() {2, 4});
        }
    }

    private void setTrafficLightColors(bool l1, bool l2, bool l3, bool l4)
    {
        light1.setLightColor(l1);
        light2.setLightColor(l2);
        light3.setLightColor(l3);
        light4.setLightColor(l4);
    }
}
