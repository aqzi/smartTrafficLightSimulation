using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public List<GameObject> vehicles = new List<GameObject>();
    public Vector3 direction = new Vector3(0, 0, 0); //direction to spawn the vehicles
    public float startSpeedOfCar = 17;
    public int rotation = 0;
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
        
        //trick to place cars on the ground
        gameObject.transform.position = new Vector3(transform.position.x, -0.43f, transform.position.z);
        gameObject.transform.parent = transform.parent;
        gameObject.transform.Rotate(direction * 90);

        float x = this.direction.x * this.startSpeedOfCar * Time.deltaTime;
        float z = this.direction.z * this.startSpeedOfCar * Time.deltaTime;

        gameObject.transform.Translate(new Vector3(x, 0, z));

        Road road = transform.parent.gameObject.GetComponent<Road>();
        road.changeAmountOfCars(true);
        
        this.spawn = true;
    }
}
