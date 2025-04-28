using UnityEngine;
using System.Collections;

public class TrafficLightController : MonoBehaviour
{
    public GameObject redLight;
    public GameObject yellowLight;
    public GameObject greenLight;

    public float interval = 5f;

    void Start()
    {
        StartCoroutine(CycleTrafficLights());
    }

    IEnumerator CycleTrafficLights()
    {
        while (true)
        {
            // Green Light ON
            SetLights(green: true, yellow: false, red: false);
            yield return new WaitForSeconds(interval);

            // Yellow Light ON
            SetLights(green: false, yellow: true, red: false);
            yield return new WaitForSeconds(interval);

            // Red Light ON
            SetLights(green: false, yellow: false, red: true);
            yield return new WaitForSeconds(interval);
        }
    }

    void SetLights(bool green, bool yellow, bool red)
    {
        greenLight.SetActive(green);
        yellowLight.SetActive(yellow);
        redLight.SetActive(red);
    }
}
