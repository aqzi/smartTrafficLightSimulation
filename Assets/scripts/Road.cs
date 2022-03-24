using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public GameObject marker;
    private int roadNr = 0;
    private bool isOpen_ = false;
    private Vector3 stopLocation = new Vector3(0, 0, 0);
    private int amountOfCars = 0;
    private List<string> leavingCars = new List<string>(); //keep count of amount of cars passing the crossroad
    private int timesStopped = 0;
    private List<Tuple<bool, float>> carCounter = new List<Tuple<bool, float>>(); //save spawn moments
    private float timer = 0.0f;

    void Start()
    {
        string tag = transform.tag;
        this.roadNr = tag[tag.Length - 1] - '0';

        if(GameEvents.current != null) 
        {
            GameEvents.current.onActiveTrafficLight += onActiveTrafficLight;
            GameEvents.current.onResultsRequest += onResultsRequest;
        }
    }

    void Update()
    {
        this.timer += Time.deltaTime;
    }

    private void onActiveTrafficLight(List<int> openRoads)
    {
        if(openRoads.Contains(this.roadNr))
        {
            this.isOpen_ = true;
            this.marker.gameObject.SetActive(true);
        } else 
        {
            this.isOpen_ = false;
            this.marker.gameObject.SetActive(false);
        }
    }

    public int getRoadNr()
    {
        return this.roadNr;
    }

    public bool isOpen()
    {
        return this.isOpen_;
    }

    public Vector3 getStopLocation()
    {
        return this.stopLocation;
    }

    public void setStopLocation(Vector3 stopLocation)
    {
        this.stopLocation = stopLocation;
    }

    public bool isEmpty()
    {
        return this.stopLocation == new Vector3(0, 0, 0);
    }

    public int oppositeRoad(int roadNr)
    {
        if(roadNr == 1 || roadNr == 3) return 4 - roadNr;
        else if(roadNr == 2 || roadNr == 4) return 6 - roadNr;
        else throw new System.Exception("Invalid roadNr.");
    }

    public void changeAmountOfCars(bool increase)
    {
        this.amountOfCars += increase ? 1 : -1;
        this.changeCarCounter(increase);
        if(this.amountOfCars < 0) this.amountOfCars = 0;
    }

    public int getAmountOfCars()
    {
        return this.amountOfCars;
    }

    public void addLeavingCar() 
    {
        this.leavingCars.Add(string.Format("{0}:{1}:{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));
    }

    public string getLeavingCars() 
    {
        return System.String.Join(", ", this.leavingCars.ToArray());
    }

    public void increaseTimesStopped()
    {
        this.timesStopped++;
    }

    public void decreaseTimesStopped()
    {
        this.timesStopped--;
    }

    public int getTimesStopped()
    {
        return this.timesStopped;
    }

    public void onResultsRequest() 
    {
        GameEvents.current.resultsReceive(this.getLeavingCars(), this.getTimesStopped(), this.getCarCounter());
    }

    public void changeCarCounter(bool increase)
    {
        this.carCounter.Add(new Tuple<bool, float>(increase, this.timer));
    }

    public string getCarCounter()
    {
        string returnVal = "";

        foreach(Tuple<bool, float> item in this.carCounter)
        {
            returnVal += item.ToString();
        }

        return this.getRoadNr() + ": " + returnVal;
    }
}
