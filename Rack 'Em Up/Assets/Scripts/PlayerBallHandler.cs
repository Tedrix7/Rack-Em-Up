using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerBallHandler : MonoBehaviour
{
    public Transform handPosition;
    public GameObject ballPrefab;
    public TMP_Text streakText;

    private GameObject heldBall;
    private int streak = 0;
    private bool lastShotScored = false;
    private bool missed = false;  // Track if last shot was a miss
    private Coroutine missCheckCoroutine;

    void Start()
    {
        SpawnAndHoldBall();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && heldBall != null)
        {
            ShootBall(6f);
        }
    }

    void SpawnAndHoldBall()
    {
        heldBall = Instantiate(ballPrefab, handPosition.position, handPosition.rotation);
        heldBall.transform.SetParent(handPosition);
        heldBall.transform.localPosition = Vector3.zero;
        heldBall.transform.localRotation = Quaternion.identity;

        Rigidbody rb = heldBall.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
    }

    void ShootBall(float force)
    {
        Rigidbody rb = heldBall.GetComponent<Rigidbody>();
        if (rb != null)
        {
            heldBall.transform.SetParent(null);
            rb.isKinematic = false;

            Vector3 throwDirection = (handPosition.forward + handPosition.up * 0.5f).normalized;
            rb.AddForce(throwDirection * force, ForceMode.Impulse);
        }

        lastShotScored = false;

        StartCoroutine(CheckForMiss(1f));  // Wait 3 seconds to detect miss

        heldBall = null;
        StartCoroutine(RespawnBallAfterDelay(1f));
    }

    IEnumerator RespawnBallAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnAndHoldBall();
    }

    IEnumerator CheckForMiss(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!lastShotScored)
        {
            missed = true;
            if (streakText != null)
            {
                streakText.text = "Miss";
            }
            Debug.Log("Missed shot!");
        }
    }

    public void OnScore()
    {
        lastShotScored = true;
        streak++;
        missed = false;  // Reset missed state
        if (streakText != null)
        {
            streakText.text = "Streak: " + streak;
        }
        Debug.Log("Score! Current streak: " + streak);
    }
}
