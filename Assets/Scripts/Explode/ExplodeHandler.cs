using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeHandler : MonoBehaviour
{
    [SerializeField] GameObject originalObject;

    [SerializeField] GameObject model;

    Rigidbody[] rigidbodies;

    private void Awake()
    {
        rigidbodies = model.GetComponentsInChildren<Rigidbody>(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Explode(Vector3.forward);
    }

    public void Explode(Vector3 externalForce)
    {
        originalObject.SetActive(false);

        foreach (Rigidbody rigidBody in rigidbodies)
        {
            rigidBody.transform.parent = null;

            rigidBody.GetComponent<MeshCollider>().enabled = true;

            rigidBody.gameObject.SetActive(true);
            rigidBody.isKinematic = false;
            rigidBody.interpolation = RigidbodyInterpolation.Interpolate;
            rigidBody.AddForce(Vector3.up * 200 + externalForce, ForceMode.Force);
            rigidBody.AddTorque(Random.insideUnitSphere * 0.5f, ForceMode.Impulse);

            //Change the tag so other objects can explode after being hit by a carpart
            rigidBody.gameObject.tag = "CarPart";
        }
    }
}
