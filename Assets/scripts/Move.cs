using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Turn 
{
    LEFT,
    RIGHT,
    NONE
}

public class Move : MonoBehaviour
{
    public float speed = 0f;
    public float acc = 0.1f;
    public float maxSpeedToTurn = 5f;
    public float maxSpeed = 17f;
    private bool stopped = true;
    private Turn turn = Turn.NONE;
    private float startRotation = 0;


    // Start is called before the first frame update
    void Start()
    {
        this.startRotation = transform.localRotation.eulerAngles.y;

        if(this.startRotation > 180) this.startRotation = - (360 - this.startRotation);
    }

    // Update is called once per frame
    void Update()
    {
        if(stopped) {
            if(this.speed < this.maxSpeedToTurn) {
                this.speed += this.acc;
            }

            transform.Translate(new Vector3(1, 0, 0) * speed * Time.deltaTime);            
        } else {
            if(turn == Turn.NONE) {
                if(speed < this.maxSpeed) {
                    speed += acc;
                }

                transform.Translate(new Vector3(1, 0, 0) * speed * Time.deltaTime);
            } else
            {
                transform.Translate(new Vector3(1, 0, 0) * speed * Time.deltaTime);

                if(turn == Turn.RIGHT)
                {
                    transform.rotation *= Quaternion.AngleAxis((90f / 3.9f) * Time.deltaTime, Vector3.up);

                    float y = transform.localRotation.eulerAngles.y;
                    if(y > 180) y = - (360 - y);

                    if(y >= (startRotation + 90)) turn = Turn.NONE;
                } else 
                {
                    // if(transform.position.z >= -9) {
                    //     transform.rotation *= Quaternion.AngleAxis((90f / 3.5f) * Time.deltaTime, -Vector3.up);

                    //     if(transform.localEulerAngles.y <= 180) 
                    //     {
                    //         turn = Turn.NONE;
                    //     }
                    // }

                    transform.rotation *= Quaternion.AngleAxis((90f / 5.2f) * Time.deltaTime, -Vector3.up);

                    // print("----------------------");
                    // print(startRotation - 90);

                    float y = transform.localRotation.eulerAngles.y;
                    if(y > 180) y = - (360 - y);

                    print(y);

                    if(y <= (startRotation - 90)) {
                        turn = Turn.NONE;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        // int value = Random.Range(1, 100);

        // if(value < 70)
        // {
        //     turn = Turn.NONE;
        // } else if(value < 85)
        // {
        //     turn = Turn.LEFT;
        // } else 
        // {
        //     turn = Turn.RIGHT;
        // }

        turn = Turn.RIGHT;

        stopped = false;
    }
}