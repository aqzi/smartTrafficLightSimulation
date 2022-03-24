using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public List<GameObject> vehicles = new List<GameObject>();
    public Vector3 direction = new Vector3(0, 0, 0); //direction to spawn the vehicles
    public float startSpeedOfCar = 17;
    public int rotation = 0;
    private bool spawn = true;
    private List<string> history = new List<string>();
    private int count = 0;
    private string path = "Assets/Simulations/";
    private int max  = 25;
    private int min = 5;

    void Start()
    {
        if(GameEvents.current != null)
        {
            GameEvents.current.onSaveSimulationToFile += onSaveSimulationToFile;
        }

        if(Settings.current != null)
        {
            this.path = string.Format("{0}{1}.txt", this.path, Settings.current.simulationFilename);
            if(Settings.current.saveSimulationToFile) FileUtil.DeleteFileOrDirectory(this.path);
            this.max = Settings.current.max;
            this.min = Settings.current.min;
        }

        this.loadSimulationFromFile();
    }

    void Update()
    {
        if(!Settings.current.loadSimulationFromFile)
        {
            int value = Random.Range(this.min, this.max);
            if(Settings.current.generateData && spawn) StartCoroutine(spawnCar(value));
            else if(spawn) StartCoroutine(spawnCar(value));
        } else
        {
            if(spawn && this.count < this.history.Count)
            {
                string val = this.history[this.count];
                string[] values = val.Split('_');

                StartCoroutine(spawnCarFromFile(System.Int32.Parse(values[0]), System.Int32.Parse(values[1])));

                this.count++;
            }
        }
    }

    IEnumerator spawnCar(int timeToWait)
    {
        this.spawn = false;

        yield return new WaitForSeconds(timeToWait);

        int carModel = Random.Range(0, 5);
        GameObject gameObject = Instantiate(vehicles[carModel]);
        
        //trick to place cars on the ground
        gameObject.transform.position = new Vector3(transform.position.x, -0.43f, transform.position.z);
        gameObject.transform.parent = transform.parent;
        gameObject.transform.Rotate(direction * 90);

        float x = this.direction.x * this.startSpeedOfCar * Time.deltaTime;
        float z = this.direction.z * this.startSpeedOfCar * Time.deltaTime;

        gameObject.transform.Translate(new Vector3(x, 0, z));

        Road road = transform.parent.gameObject.GetComponent<Road>();
        road.changeAmountOfCars(true);

        if(Settings.current.saveSimulationToFile)
        {
            this.history.Add(string.Format("{0}_{1}", timeToWait, carModel));
        }
        
        this.spawn = true;
    }

    IEnumerator spawnCarFromFile(int timeToWait, int carModel)
    {
        this.spawn = false;

        yield return new WaitForSeconds(timeToWait);

        GameObject gameObject = Instantiate(vehicles[carModel]);
        
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

    private void onSaveSimulationToFile()
    {
        if(!Settings.current.saveSimulationToFile) return;

        string name = transform.gameObject.name;
        char lastCharacter = name[name.Length-1];

        StreamWriter writer = new StreamWriter(this.path, true);
        writer.WriteLine(string.Format("{0}|{1}", lastCharacter, System.String.Join(",", this.history.ToArray())));
        writer.Close();
    }

    private void loadSimulationFromFile()
    {
        if(!Settings.current.loadSimulationFromFile) return;

        string name = transform.gameObject.name;
        char lastCharacter = name[name.Length-1];

        StreamReader reader = new StreamReader(this.path);
        string line;

        while((line = reader.ReadLine()) != null)
        {
            string[] splitLine = line.Split('|');

            if(splitLine[0] == lastCharacter.ToString())
            {
                string[] simulationString = splitLine[1].Split(',');
                
                foreach(string l in simulationString)
                {
                    this.history.Add(l);
                }
            }
        }

        reader.Close();
    }
}
