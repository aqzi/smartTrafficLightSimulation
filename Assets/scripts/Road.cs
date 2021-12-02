using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public GameObject marker;
    private int roadNr = 0;
    private bool isOpen_ = false;
    private Vector3 stopLocation = new Vector3(0, 0, 0);

    void Start()
    {
        string tag = transform.tag;
        this.roadNr = tag[tag.Length - 1] - '0';

        if(GameEvents.current != null) GameEvents.current.onActiveTrafficLight += onActiveTrafficLight;
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
}
