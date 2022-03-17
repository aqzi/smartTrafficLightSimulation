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

    void Start()
    {
        string tag = transform.tag;
        this.roadNr = tag[tag.Length - 1] - '0';

        if(GameEvents.current != null) 
        {
            GameEvents.current.onActiveTrafficLight += onActiveTrafficLight;
            GameEvents.current.onLeavingCarsRequest += onLeavingCarsRequest;
        }
    }

    void Update()
    {
        
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
        if(this.amountOfCars < 0) this.amountOfCars = 0;
    }

    public int getAmountOfCars()
    {
        return this.amountOfCars;
    }

    public void addLeavingCar() {
        this.leavingCars.Add(string.Format("{0}:{1}:{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));
    }

    public string getLeavingCars() {
        return System.String.Join(", ", this.leavingCars.ToArray());
    }

    public void onLeavingCarsRequest() {
        GameEvents.current.leavingCarsSend(this.getLeavingCars());
    }
}
