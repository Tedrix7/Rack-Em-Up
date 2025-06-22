using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerBallHandler : MonoBehaviour
{
    public Transform handPosition;
    public GameObject ballPrefab;
    public TMP_Text streakText;
    public TMP_Text coinText;

    private GameObject heldBall;
    private int streak = 0;
    private int misses = 0;
    private bool lastShotScored = false;

    public int coins = 0;
    private int coinMultiplier = 1;
    private bool missProtection = false;

    void Start()
    {
        SpawnAndHoldBall();
        UpdateStreakText();
        UpdateCoinUI();
    }

    void Update()
    {
        if (BoostShop.IsShopActive)
            return;

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

        StartCoroutine(CheckForMiss(3f));
        StartCoroutine(RespawnBallAfterDelay(1f));

        heldBall = null;
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
            if (missProtection)
            {
                Debug.Log("Miss ignored due to protection.");
                yield break;
            }
            misses++;
            if (misses >= 3)
            {
                streak = 0;
                misses = 0;
                streakText.text = "Reset!";
                yield return new WaitForSeconds(1.5f);
                UpdateStreakText();
            }
            else
            {
                streakText.text = "Miss: " + misses + "/3";
            }

            Debug.Log("Missed shot! (" + misses + "/3), -10 coins");
        }
    }

    public void OnScore()
    {
        lastShotScored = true;
        streak++;
        misses = 0;

        if (streak % 10 == 0)
        {
            int earned = 50 * coinMultiplier;
            coins += earned;
            Debug.Log($"+{earned} coins for streak!");
            UpdateCoinUI();
        }

        UpdateStreakText();
        Debug.Log("Score! Current streak: " + streak);
    }

    void UpdateStreakText()
    {
        if (streakText != null)
            streakText.text = "Streak: " + streak;
    }

    public void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = "Coins: " + coins;
    }

    public void SetMissProtection(bool active)
    {
        missProtection = active;
    }

    public void SetCoinMultiplier(int multiplier)
    {
        coinMultiplier = multiplier;
    }
}
