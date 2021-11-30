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
    private int roadNr = 0;
    private List<int> openRoads = new List<int>() {0, 0};
    private bool leaveQueue = false;
    // private Car next = null;
    // private Car prev = null;
    // private Vector3 stop = new Vector3(0, 0, 0);
    // private bool goTowardsCrossroad = false;

    private void Awake() {
        if(GameEvents.current != null) GameEvents.current.onActiveTrafficLight += onActiveTrafficLight;
    }

    void Start()
    {
        setRoadNr();

        this.startRotation = transform.localRotation.eulerAngles.y;
    }

    void Update()
    {
        // if(this.goTowardsCrossroad) transform.Translate(new Vector3(1, 0, 0) * 10f * Time.deltaTime);

        // if(stop != new Vector3(0, 0, 0))
        // {
        //     if(roadNr == 1 && this.stop.x < transform.position.x) {
        //         this.goTowardsCrossroad = false; 
        //         return;
        //     }

        //     if(roadNr == 2 && this.stop.z > transform.position.z) {
        //         this.goTowardsCrossroad = false; 
        //         return;
        //     }

        //     if(roadNr == 3 && this.stop.x > transform.position.x) {
        //         this.goTowardsCrossroad = false; 
        //         return;
        //     }

        //     if(roadNr == 4 && this.stop.z < transform.position.z) {
        //         this.goTowardsCrossroad = false; 
        //         return;
        //     }
        // }

        //if(!this.leaveQueue && !this.openRoads.Contains(this.roadNr)) return;

        if(this.outsideCrossroad) {
            if(this.speed < this.maxSpeedToTurn) {
                this.speed += this.acc;
            }

            transform.Translate(new Vector3(1, 0, 0) * this.speed * Time.deltaTime);            
        } else {
            if(this.turn == Turn.NONE) {
                if(this.speed < this.maxSpeed) {
                    this.speed += this.acc;
                }

                transform.Translate(new Vector3(1, 0, 0) * this.speed * Time.deltaTime);
            } else
            {
                transform.Translate(new Vector3(1, 0, 0) * this.speed * Time.deltaTime);

                if(this.turn == Turn.RIGHT)
                {
                    transform.rotation *= Quaternion.AngleAxis((90f / 3.9f) * Time.deltaTime, Vector3.up);

                    if(this.roadNr == 4) 
                    {
                        if(transform.localRotation.eulerAngles.y < 270) this.turn = Turn.NONE;
                    } else
                    {
                        if(transform.localRotation.eulerAngles.y >= ((startRotation + 90) % 360)) this.turn = Turn.NONE;
                    }
                } else 
                {
                    transform.rotation *= Quaternion.AngleAxis((90f / 5.2f) * Time.deltaTime, -Vector3.up);

                    if(this.roadNr == 1)
                    {
                        if(transform.localRotation.eulerAngles.y < 270) this.turn = Turn.NONE;
                    } else if(this.roadNr == 2)
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

            // if(this.next != null)
            // {
            //     this.next.setPrevCar(null);
            //     this.next = null;
            // }
        }

        if(other.tag == "End")
        {
            Destroy(this.gameObject);
        }
    }

    private void onActiveTrafficLight(List<int> activeLights)
    {

        this.openRoads = activeLights;
    }

    public void setRoadNr()
    {
        string tag = transform.parent.parent.tag;
        this.roadNr = tag[tag.Length - 1] - '0';
    }

    public bool isRoadOpen()
    {
        return this.openRoads.Contains(this.roadNr);
    }

    // public void setNextCar(Car car)
    // {
    //     this.next = car;
    // }

    // public void setPrevCar(Car car)
    // {
    //     this.prev = car;
    // }

    // //Previous car should tell the next car where to stop and when to move.
    // public void setStop(Vector3 stopPosition)
    // {
    //     this.stop = stopPosition;
    // }

    // public void setGoTowardsCrossroad(bool value)
    // {
    //     this.goTowardsCrossroad = value;
    // }
}