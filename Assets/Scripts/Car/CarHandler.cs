using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class CarHandler : MonoBehaviour
{
    [SerializeField] private float[] lanePositions = { -0.3f, 0.3f };
    [SerializeField] private float laneChangeSpeed = 5f;
    private int currentLane = 1;
    private bool isChangingLane = false;

    [SerializeField] Rigidbody rigidBody;

    [SerializeField] Transform gameModel;

    [SerializeField] ExplodeHandler explodeHandler;

    [Header("SFX")]
    [SerializeField] AudioSource carEngineAS;

    [SerializeField] AnimationCurve carPitchAnimationCurve;

    [SerializeField] AudioSource carSkidAS;

    [SerializeField] AudioSource carCrashAS;

    //Max Values
    float maxSteerVelocity = 2;
    float maxForwardVelocity = 30;
    float carMaxSpeedPercentage = 0;

    //Multipliers
    float accelerationMultiplier = 3;
    float breaksMultiplier = 15;
    float steeringMultiplier = 5;

    //Input
    Vector2 input = Vector2.zero;

    private bool hasBeenHit = false;
    private bool isInvulnerable = false;

    bool isExploded = false;

    bool isPlayer = true;

    // Start is called before the first frame update
    void Start()
    {
        isPlayer = CompareTag("Player");

        if (isPlayer)
        {
            carEngineAS.Play();

            switch (GameManager.Instance.CurrentDifficulty)
            {
                case GameManager.Difficulty.Easy:
                    SetMaxSpeed(25f);
                    accelerationMultiplier = 2f; // slower acceleration
                    break;
                case GameManager.Difficulty.Normal:
                    SetMaxSpeed(30f);
                    accelerationMultiplier = 3f;
                    break;
                case GameManager.Difficulty.Hard:
                    SetMaxSpeed(40f);
                    accelerationMultiplier = 5f; // accelerate faster 
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isExploded)
        {
            FadeOutCarAudio();
            return;
        }

        gameModel.transform.rotation = Quaternion.Euler(0, rigidBody.velocity.x * 5, 0);

        UpdateCarAudio();
    }

    private void FixedUpdate()
    {
        //Is Exploded
        if (isExploded)
        {
            //Apply drag
            rigidBody.drag = rigidBody.velocity.z * 0.1f;
            rigidBody.drag = Mathf.Clamp(rigidBody.drag, 1.5f, 10);

            //Move towards after the car has exploded
            rigidBody.MovePosition(Vector3.Lerp(transform.position, new Vector3(0, 0, transform.position.z), Time.deltaTime * 0.5f));
      
            return;
        
        }


        //Apply Acceleration
        if (input.y > 0)
            Accelerate();
        else
            rigidBody.drag = 0.2f;

        //Apply Brakes
        if (input.y < 0)
            Brake();

        Steer();

        if (rigidBody.velocity.z <= 0)
            rigidBody.velocity = Vector3.zero;
    }

    void Accelerate()
    {
        rigidBody.drag = 0;

        //Stay within the speed limit
        if (rigidBody.velocity.z >= maxForwardVelocity)
            return;

        rigidBody.AddForce(rigidBody.transform.forward * accelerationMultiplier * input.y);
    }

    void Brake()
    {
        //Dont brake unless we are going forward
        if (rigidBody.velocity.z <= 0)
            return;

        rigidBody.AddForce(rigidBody.transform.forward * breaksMultiplier * input.y);
    }

    void Steer()
    {
        float targetX = lanePositions[currentLane];
        Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
        Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * laneChangeSpeed);
        rigidBody.MovePosition(newPosition);
    }

    public void ChangeLane(int direction)
    {
    int newLane = currentLane + direction;
    newLane = Mathf.Clamp(newLane, 0, lanePositions.Length - 1);
    currentLane = newLane;
    }

    public void SetForwardInput(float vertical)
{
    input = new Vector2(0, vertical); // Only Y-axis is relevant
}

    void UpdateCarAudio()
    {
        if (!isPlayer)
            return;

        carMaxSpeedPercentage = rigidBody.velocity.z / maxForwardVelocity;

        carEngineAS.pitch = carPitchAnimationCurve.Evaluate(carMaxSpeedPercentage);

        if (input.y < 0 && carMaxSpeedPercentage > 0.2f)
        {
            if (!carSkidAS.isPlaying) 
                carSkidAS.Play();

            carSkidAS.volume = Mathf.Lerp(carSkidAS.volume, 1.0f, Time.deltaTime * 10);
        }
        else
        {
            carSkidAS.volume = Mathf.Lerp(carSkidAS.volume, 1.0f, Time.deltaTime * 30);
        }
    }

    void FadeOutCarAudio()
    {
        if (!isPlayer) 
            return;

        carEngineAS.volume = Mathf.Lerp(carEngineAS.volume, 0, Time.deltaTime * 10);
        carSkidAS.volume = Mathf.Lerp(carSkidAS.volume, 0, Time.deltaTime * 10);
    }
    public void SetInput(Vector2 inputVector)
    {
        inputVector.Normalize();

        input = inputVector;
    }

    public void SetMaxSpeed(float newMaxSpeed)
    {
        maxForwardVelocity = newMaxSpeed;
    }

    IEnumerator SlowDownTimerCO()
    {
        while (Time.timeScale > 0.2f)
        {
            Time.timeScale -= Time.deltaTime * 2;

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        while (Time.timeScale <= 1.0f)
        {
            Time.timeScale += Time.deltaTime;

            yield return null;
        }

        Time.timeScale = 1.0f;

    }
    private void SetCollidersTrigger(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (var col in colliders)
        {
            col.isTrigger = state;
        }
    }

    public void SetLane(int laneIndex)
    {
        currentLane = Mathf.Clamp(laneIndex, 0, lanePositions.Length - 1);
    }

    //Events
    private void OnCollisionEnter(Collision collision)
    {
        //AI cars will only explode when they hit the player or a car part
        if (!isPlayer)
        {
            if(collision.transform.root.CompareTag("Untagged"))
                return;

            if (collision.transform.root.CompareTag("Car AI"))
                return;
        }

        // Only the player has invulnerability logic
        if (isPlayer)
        {
            if (isInvulnerable)
                return;

            if (!hasBeenHit)
            {
                hasBeenHit = true;
                StartCoroutine(InvulnerabilityTimer());
                return; // skip crash logic on first hit
            }
        }

        if (isExploded) return;

        Vector3 velovity = rigidBody.velocity;
        explodeHandler.Explode( velovity * 45);

        isExploded = true;

        carCrashAS.volume = carMaxSpeedPercentage;
        carCrashAS.volume = Mathf.Clamp(carCrashAS.volume, 0.25f, 1.0f);

        carCrashAS.pitch = carMaxSpeedPercentage;
        carCrashAS.pitch = Mathf.Clamp(carCrashAS.volume, 0.3f, 1.0f);

        carCrashAS.Play();

        if (isPlayer)
        {
            FindObjectOfType<ScoreManager>().StopCounting();
            StartCoroutine(ShowDeathPanelWithDelay());
        }

        StartCoroutine(SlowDownTimerCO());

if (isPlayer)
{
    StartCoroutine(ShowDeathPanelWithDelay());
}
    }
    private IEnumerator InvulnerabilityTimer()
    {
        isInvulnerable = true;

        // Turn colliders into triggers to pass through objects
        SetCollidersTrigger(true);

        StartCoroutine(FlashWhileInvulnerable());

        yield return new WaitForSeconds(3f);

        // Return to normal collision behavior
        SetCollidersTrigger(false);
        isInvulnerable = false; isInvulnerable = false;
    }
    private IEnumerator FlashWhileInvulnerable()
    {
        Renderer[] renderers = gameModel.GetComponentsInChildren<Renderer>();

        float flashDelay = 0.2f;
        float timer = 0f;

        while (isInvulnerable)
        {
            foreach (Renderer rend in renderers)
            {
                rend.enabled = false;
            }

            yield return new WaitForSeconds(flashDelay);

            foreach (Renderer rend in renderers)
            {
                rend.enabled = true;
            }

            yield return new WaitForSeconds(flashDelay);

            timer += flashDelay * 2;
        }

        // Ensure renderers are visible again after invulnerability
        foreach (Renderer rend in renderers)
        {
            rend.enabled = true;
        }
    }
    private IEnumerator ShowDeathPanelWithDelay()
    {
        yield return new WaitForSecondsRealtime(2f);
        FindObjectOfType<DeathUIManager>().ShowDeathPanel();
    }
}
