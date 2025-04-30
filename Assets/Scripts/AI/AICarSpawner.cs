using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] carAIPrefabs;

    private GameObject[] carAIPool;

    Transform playerCarTransform;

    //Timing
    float timeLastCarSpawned = 0;
    WaitForSeconds wait = new WaitForSeconds(0.5f);

    //Overlapped check
    [SerializeField] LayerMask otherCarslayerMask;

    Collider[] overlappedCheckCollider = new Collider[1];

    void Start()
    {
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;

        int poolSize = 20;
        switch (GameManager.Instance.CurrentDifficulty)
        {
            case GameManager.Difficulty.Easy:
                poolSize = 10;
                break;
            case GameManager.Difficulty.Normal:
                poolSize = 20;
                break;
            case GameManager.Difficulty.Hard:
                poolSize = 30;
                break;
        }
        carAIPool = new GameObject[poolSize];

        int prefabIndex = 0;

        for (int i = 0; i < carAIPool.Length; i++)
        {
            carAIPool[i] = Instantiate(carAIPrefabs[prefabIndex]);
            carAIPool[i].SetActive(false);

            prefabIndex++;

            //Loop the prefab Index if we run out of prefabs
            if (prefabIndex > carAIPrefabs.Length - 1)
                prefabIndex = 0;
        }

        StartCoroutine(UpdateLessOftenCO());
    }
    
    IEnumerator UpdateLessOftenCO()
    {
       while (true)
        {
            CleanUpCarsBeyondView();
            SpawnNewCars();
            yield return wait;
        }
    }

    void SpawnNewCars()
    {
        if (Time.time - timeLastCarSpawned < 1)
            return;

        GameObject carToSpawn = null;

        //Find a car to spawn
        foreach (GameObject aiCar in carAIPool)
        {
            //Skipp active cars
            if (aiCar.activeInHierarchy)
            {
                continue;
            }

            carToSpawn = aiCar;
            break;
        }

        //No car available to spawn
        if (carToSpawn == null)
            return;

        int randomLane = Random.Range(0, Utils.CarLanes.Length);
        float laneX = Utils.CarLanes[randomLane];
        Vector3 spawnPosition = new Vector3(laneX, 0, playerCarTransform.position.z + 100);

        if (Physics.OverlapBoxNonAlloc(spawnPosition, new Vector3(1, 1, 2), overlappedCheckCollider, Quaternion.identity, otherCarslayerMask) > 0)
            return;

        carToSpawn.transform.position = spawnPosition;
        carToSpawn.SetActive(true);

        timeLastCarSpawned = Time.time;
    }

    void CleanUpCarsBeyondView()
    {
        foreach (GameObject aiCar in carAIPool)
        {
            //Skip inactive cars
            if (!aiCar.activeInHierarchy)
                continue;
            
            //Check if AI car is too far ahead
            if (aiCar.transform.position.z - playerCarTransform.position.z > 200)
                aiCar.SetActive(false);

            //Check if AI car is toof ar behind
            if (aiCar.transform.position.z - playerCarTransform.position.z < -50)
                aiCar.SetActive(false);        
        }
    }
         
}
