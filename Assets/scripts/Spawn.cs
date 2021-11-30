using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public List<GameObject> vehicles = new List<GameObject>();
    public Vector3 direction = new Vector3(0, 0, 0); //direction to spawn the vehicles
    public GameObject queue; //queue that will contain cars
    public float startSpeedOfCar = 17;
    public int rotation = 0;
    private bool spawn = true;

    void Update()
    {
        if(spawn) 
        {
            StartCoroutine(spawnCar(Random.Range(2, 6)));
        }
    }

    IEnumerator spawnCar(int timeToWait)
    {
        this.spawn = false;

        yield return new WaitForSeconds(5);

        GameObject gameObject = Instantiate(vehicles[Random.Range(0, 5)]);
        Car car = gameObject.GetComponent<Car>();
        
        car.transform.position = transform.position;
        car.transform.parent = this.queue.transform;
        car.transform.Rotate(direction * 90);

        car.setRoadNr();

        car.transform.Translate(this.direction * this.startSpeedOfCar * Time.deltaTime);

        // Car lastCarInQueue = this.queue.transform.GetChild(this.queue.transform.childCount - 1).gameObject.GetComponent<Car>();

        // if(lastCarInQueue != null)
        // {
        //     lastCarInQueue.setNextCar(car);
        //     car.setPrevCar(lastCarInQueue);

        //     if(!lastCarInQueue.isRoadOpen())
        //     {
        //         car.setStop(transform.position);
        //     }
        // }

        this.spawn = true;
    }
}
