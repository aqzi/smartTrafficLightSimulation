using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public List<GameObject> vehicles = new List<GameObject>();
    public Vector3 direction = new Vector3(0, 0, 0); //direction to spawn the vehicles
    public float startSpeedOfCar = 17;
    public int rotation = 0;
    public Car lastAddedCar;
    private bool spawn = true;

    void Update()
    {
        if(spawn) StartCoroutine(spawnCar(Random.Range(2, 6)));
    }

    IEnumerator spawnCar(int timeToWait)
    {
        this.spawn = false;

        yield return new WaitForSeconds(5);

        GameObject gameObject = Instantiate(vehicles[Random.Range(0, 5)]);
        Car car = gameObject.GetComponent<Car>();

        this.lastAddedCar = car;
        
        car.transform.position = transform.position;
        car.transform.parent = transform.parent;
        car.transform.Rotate(direction * 90);

        car.transform.Translate(this.direction * this.startSpeedOfCar * Time.deltaTime);

        // if(this.lastAddedCar != null)
        // {
        //     this.lastAddedCar.setNextCar(car);
        //     car.setPrevCar(this.lastAddedCar);

        //     // if(!lastCarInQueue.isRoadOpen())
        //     // {
        //     //     car.setStop(transform.position);
        //     // }
        // }

        this.spawn = true;
    }
}
