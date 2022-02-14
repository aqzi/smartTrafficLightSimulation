using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectGap : MonoBehaviour
{
    public Road road;
    private int amountOfWaitingCars = 0;
    private (bool, Guid) oppositeRoadWantLeftTurn = (false, Guid.Empty);

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

    private void onGoLeft(int roadNr, Guid id)
    {
        if(roadNr == this.road.getRoadNr())
        {
            this.oppositeRoadWantLeftTurn = (true, id);

            if(amountOfWaitingCars == 0)
            {
                GameEvents.current.goLeftAllowed(this.road.oppositeRoad(roadNr), id);
                this.oppositeRoadWantLeftTurn = (false, Guid.Empty);
            }
        }

        //opposite of the opposite road gives back the original road
        if(this.road.getRoadNr() == this.road.oppositeRoad(roadNr) && this.oppositeRoadWantLeftTurn.Item1)
        {
            GameEvents.current.goLeftAllowed(roadNr, this.oppositeRoadWantLeftTurn.Item2);
        }
    }
}
