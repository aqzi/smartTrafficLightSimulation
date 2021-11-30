using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Turn 
{
    LEFT,
    RIGHT,
    NONE
}

public class Car : MonoBehaviour
{
    public float speed = 0f;
    public float acc = 0.1f;
    public float maxSpeedToTurn = 5f;
    public float maxSpeed = 17f;
    private bool outsideCrossroad = true;
    private Turn turn = Turn.NONE;
    private float startRotation = 0;
    private bool leaveQueue = false;
    private Road road;
    private bool move = true;

    void Start()
    {
        this.road = transform.parent.GetComponent<Road>();
        this.startRotation = transform.localRotation.eulerAngles.y;
    }

    void Update()
    {
        if(!this.move && this.road.isOpen()) this.move = true;

        if(this.outsideCrossroad) {
            if(this.speed < this.maxSpeedToTurn) this.speed += this.acc;

            if(this.move) transform.Translate(new Vector3(1, 0, 0) * this.speed * Time.deltaTime);
        } else {
            if(this.turn == Turn.NONE) {
                if(this.speed < this.maxSpeed) this.speed += this.acc;

                transform.Translate(new Vector3(1, 0, 0) * this.speed * Time.deltaTime);
            } else
            {
                transform.Translate(new Vector3(1, 0, 0) * this.speed * Time.deltaTime);

                if(this.turn == Turn.RIGHT)
                {
                    transform.rotation *= Quaternion.AngleAxis((90f / 3.9f) * Time.deltaTime, Vector3.up);

                    if(this.road.getRoadNr() == 4) 
                    {
                        if(transform.localRotation.eulerAngles.y < 270) this.turn = Turn.NONE;
                    } else
                    {
                        if(transform.localRotation.eulerAngles.y >= ((startRotation + 90) % 360)) this.turn = Turn.NONE;
                    }
                } else 
                {
                    transform.rotation *= Quaternion.AngleAxis((90f / 5.2f) * Time.deltaTime, -Vector3.up);

                    if(this.road.getRoadNr() == 1)
                    {
                        if(transform.localRotation.eulerAngles.y < 270) this.turn = Turn.NONE;
                    } else if(this.road.getRoadNr() == 2)
                    {
                        if(transform.localRotation.eulerAngles.y > 90) this.turn = Turn.NONE;
                    } else
                    {
                        if(transform.localRotation.eulerAngles.y <= (startRotation - 90)) this.turn = Turn.NONE;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Decision_point")
        {
            int value = Random.Range(1, 100);

            if(value < 70)
            {
                this.turn = Turn.NONE;
            } else if(value < 85)
            {
                this.turn = Turn.LEFT;
            } else 
            {
                this.turn = Turn.RIGHT;
            }

            this.outsideCrossroad = false;
            this.leaveQueue = true;
        }

        if(other.tag == "End") Destroy(this.gameObject);

        if(other.tag == "Stop" && !this.road.isOpen()) this.move = false;

        if(other.tag == "Prevent_backside_collision") this.move = false;
    }
}