using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectGap : MonoBehaviour
{
    public Road road;
    private int amountOfWaitingCars = 0;

    private void Start() {
        if(GameEvents.current != null) 
        {
            GameEvents.current.onGoLeft += onGoLeft;
        }
    }

    private void OnTriggerEnter(Collider other) {
        this.amountOfWaitingCars++;
    }

    private void OnTriggerExit(Collider other) {
        this.amountOfWaitingCars--;
    }

    private int roadToNotify(int roadNr)
    {
        if(roadNr == 1 || roadNr == 3) return 4 - roadNr;
        else if(roadNr == 2 || roadNr == 4) return 6 - roadNr;
        else throw new System.Exception("Invalid roadNr.");
    }

    private void onGoLeft(int roadNr, Guid id)
    {
        if(roadNr == this.road.getRoadNr() && amountOfWaitingCars == 0)
        {
            GameEvents.current.goLeftAllowed(roadToNotify(this.road.getRoadNr()), id);
        }
    }
}
