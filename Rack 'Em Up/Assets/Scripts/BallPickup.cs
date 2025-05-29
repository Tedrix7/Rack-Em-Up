using UnityEngine;
using System.Collections;

public class PlayerBallHandler : MonoBehaviour
{
    public Transform handPosition;      // Drag HandPosition GameObject here
    public GameObject ballPrefab;       // Drag your basketball prefab here
    private GameObject heldBall;

    void Start()
    {
        SpawnAndHoldBall();
    }

    void Update()
    {
        // Shoot when left mouse button is clicked, but only if we have a ball
        if (Input.GetMouseButtonDown(0) && heldBall != null)
        {
            ShootBall(6f);  // Adjust force as needed
        }
    }

    void SpawnAndHoldBall()
    {
        if (ballPrefab == null || handPosition == null)
        {
            Debug.LogError("Missing references! Assign ballPrefab and handPosition.");
            return;
        }

        // Instantiate the ball and parent it to the hand
        heldBall = Instantiate(ballPrefab);
        heldBall.transform.SetParent(handPosition);
        heldBall.transform.localPosition = Vector3.zero;
        heldBall.transform.localRotation = Quaternion.identity;

        // Disable physics so it stays in the hand
        Rigidbody rb = heldBall.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        else
        {
            Debug.LogError("ballPrefab is missing Rigidbody!");
        }
    }

    void ShootBall(float force)
    {
        Rigidbody rb = heldBall.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Detach from hand
            heldBall.transform.SetParent(null);

            // Enable physics to make it fly
            rb.isKinematic = false;

            // Create a forward + upward arc direction
            Vector3 throwDirection = (handPosition.forward + handPosition.up * 0.5f).normalized;
            rb.AddForce(throwDirection * force, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("Held ball is missing Rigidbody!");
        }

        // Clear heldBall to indicate the hand is now empty
        heldBall = null;

        // Start coroutine to spawn a new ball after 3 seconds
        StartCoroutine(RespawnBallAfterDelay(1f));
    }

    IEnumerator RespawnBallAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Only spawn a new ball if hand is empty
        if (heldBall == null)
        {
            SpawnAndHoldBall();
        }
    }
}
