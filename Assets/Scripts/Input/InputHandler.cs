using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputHandler : MonoBehaviour
{
    [SerializeField] CarHandler carHandler;

    private void Awake()
    {
        if(!CompareTag("Player"))
        {
            Destroy(this);
            return;
        }
    }

    private void Update()
    {
#if UNITY_IOS || UNITY_ANDROID
        HandleTouchInput();
#else
        HandleKeyboardInput();
#endif

        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f; // Reset time scale before reloading
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void HandleKeyboardInput()
    {
        float vertical = 1f; // Always moving forward

        // Brake if Space is pressed
        if (Input.GetKey(KeyCode.S))
        {
            vertical = -1f;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            carHandler.ChangeLane(-1); // Move left
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            carHandler.ChangeLane(1); // Move right
        }

        carHandler.SetForwardInput(vertical);
    }

    void HandleTouchInput()
    {
        float vertical = 1f; // Always moving forward

        foreach (Touch touch in Input.touches)
        {
            Vector2 screenPos = touch.position;

            if (screenPos.y < Screen.height * 0.3f)
            {
                vertical = -1f; // Brake zone
            }
            else
            {
                if (touch.phase == TouchPhase.Began)
                {
                    if (screenPos.x < Screen.width / 2f)
                    {
                        carHandler.ChangeLane(-1); // Move Left
                    }
                    else
                    {
                        carHandler.ChangeLane(1);  // Move Right
                    }
                }
            }
        }

        carHandler.SetForwardInput(vertical);
    }
}