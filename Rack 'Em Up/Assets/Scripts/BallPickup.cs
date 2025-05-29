using UnityEngine;

public class PlayerBallHandler : MonoBehaviour
{
    public Transform handPosition;      // Drag HandPosition GameObject here
    public GameObject ballPrefab;       // Drag your basketball prefab here
    private GameObject heldBall;

    void Start()
    {
        // Spawn and hold the ball at the start
        SpawnAndHoldBall();
    }

    void Update()
    {
        // Only shoot when left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            ShootBall(5f);  // Adjust force as needed
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
        if (heldBall != null)
        {
            Rigidbody rb = heldBall.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Enable physics and detach from hand
                rb.isKinematic = false;
                heldBall.transform.SetParent(null);

                // Create a forward + upward throw direction to simulate arc
                Vector3 throwDirection = (handPosition.forward + handPosition.up * 0.5f).normalized;

                // Apply impulse force in the direction of the throw
                rb.AddForce(throwDirection * force, ForceMode.Impulse);
            }
            else
            {
                Debug.LogError("Held ball is missing Rigidbody!");
            }

            // Clear heldBall reference
            heldBall = null;
        }
        else
        {
            Debug.Log("No ball to shoot!");
        }
    }
}
