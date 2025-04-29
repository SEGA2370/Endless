using System.Collections;
using UnityEngine;

public class PedestrianMover : MonoBehaviour
{
    [SerializeField] private float moveDuration = 4f; // Total time to move from x1 to x-1
    private Vector3 startPoint;
    private Vector3 endPoint;
    private bool movingToEnd = true;

    private void Start()
    {
        startPoint = new Vector3(1f, 0f, transform.position.z);
        endPoint = new Vector3(-1f, 0f, transform.position.z);

        transform.position = startPoint; // Make sure it starts at x=1
        transform.rotation = Quaternion.Euler(0f, -90f, 0f); // Face left initially

        StartCoroutine(MoveLoop());
    }

    private IEnumerator MoveLoop()
    {
        while (true)
        {
            Vector3 from = movingToEnd ? startPoint : endPoint;
            Vector3 to = movingToEnd ? endPoint : startPoint;
            float timer = 0f;

            while (timer < moveDuration)
            {
                transform.position = Vector3.Lerp(from, to, timer / moveDuration);
                timer += Time.deltaTime;
                yield return null;
            }

            // Snap to final position
            transform.position = to;

            // Flip direction
            movingToEnd = !movingToEnd;

            if (movingToEnd)
                transform.rotation = Quaternion.Euler(0f, -90f, 0f); // Face left
            else
                transform.rotation = Quaternion.Euler(0f, 90f, 0f); // Face right

            yield return null;
        }
    }
}
