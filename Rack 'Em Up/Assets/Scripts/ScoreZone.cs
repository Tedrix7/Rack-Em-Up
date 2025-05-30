using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    public PlayerBallHandler playerHandler;  // Direct reference, set in Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (playerHandler != null)
            {
                playerHandler.OnScore();
            }
            else
            {
                Debug.LogWarning("PlayerBallHandler reference not assigned!");
            }
        }
    }
}
