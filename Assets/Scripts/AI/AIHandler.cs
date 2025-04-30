using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHandler : MonoBehaviour
{
    [SerializeField] CarHandler carHandler;

    //Collision Detection
    [SerializeField] LayerMask otherCarslayerMask;

    [SerializeField] MeshCollider meshCollider;

    [Header("SFX")]
    [SerializeField] AudioSource honkHornAS;

    RaycastHit[] raycastHits = new RaycastHit[1];
    bool isCarAhead = false;
    float carAheadDistance = 0;

    //Lanes
    int drivingInLane = 0;

    //Timing
    WaitForSeconds wait = new WaitForSeconds(0.2f);

    private void Awake()
    {
        if(CompareTag("Player"))
        {
            Destroy(this);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateLessOfterCO());
    }

    // Update is called once per frame
    void Update()
    {
        float accelerationInput = 1.0f;
        float steerInput = 0.0f;

        if(isCarAhead)
        {
            accelerationInput = -1;

            if (carAheadDistance < 10 && !honkHornAS.isPlaying)
            {
                honkHornAS.pitch = Random.Range(0.5f, 1.1f);
                honkHornAS.Play();
            }
        }

        float desiredPositionX = Utils.CarLanes[drivingInLane];

        float difference = desiredPositionX - transform.position.x;

        if(Mathf.Abs(difference) > 0.05f)
        {
            steerInput = 1.0f * difference;
        }

        steerInput = Mathf.Clamp(steerInput, -1.0f, 1.0f);

        carHandler.SetInput(new Vector2 (steerInput, accelerationInput));

    }

    IEnumerator UpdateLessOfterCO()
    {
        while (true)
        {
            isCarAhead = CheckIfOtherCarsIsAhead();
            yield return wait;
        }
    }

    bool CheckIfOtherCarsIsAhead()
    {
        meshCollider.enabled = false;

        int numberOfHits = Physics.BoxCastNonAlloc(transform.position, Vector3.one * 0.25f, 
            transform.forward, raycastHits, Quaternion.identity, 2, otherCarslayerMask);

        meshCollider.enabled = true;

        if (numberOfHits > 0)
        {
            carAheadDistance = (transform.position - raycastHits[0].point).magnitude;
            return true;
        }
        return false;
    }

    //Events
    private void OnEnable()
    {
        // Set a random lane index
        drivingInLane = Random.Range(0, Utils.CarLanes.Length);

        // Update the car handler's lane accordingly
        carHandler.SetLane(drivingInLane);

        // Set a random speed
        carHandler.SetMaxSpeed(Random.Range(2, 4));
    }
}
