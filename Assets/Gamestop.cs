using UnityEngine;
using UnityEngine.UI;

public class Gamestop : MonoBehaviour
{
    public static Gamestop Instance; // Singleton instance

    public GameObject player; // Reference to the player GameObject
    public GameObject gameOverCanvas; // Reference to the Canvas GameObject with game over image
    public float screenBoundaryPadding = 1f; // Padding to consider when checking if the player is on the screen
    public AudioSource go;

    private void Awake()
    {
        // Ensure there is only one instance of the GameManager
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is outside the screen boundaries
        if (!IsPlayerOnScreen())
        {

            // Call the function to handle player death
            HandlePlayerDeath();
        }
    }

    bool IsPlayerOnScreen()
    {
        if (player == null)
        {
            Debug.LogWarning("Player reference not set in GameManager!");
            return false;
        }

        // Get the player's position in screen coordinates
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(player.transform.position);

        // Check if the player is within the screen boundaries with padding
        return screenPoint.x > screenBoundaryPadding && screenPoint.x < Screen.width - screenBoundaryPadding &&
               screenPoint.y > screenBoundaryPadding && screenPoint.y < Screen.height - screenBoundaryPadding;
    }

    void HandlePlayerDeath()
    {
        // Disable the player
        if (player != null)
        {
            player.SetActive(false);
        }

        // Enable and show the game over canvas
        if (gameOverCanvas != null)
        {
            go.Play();
            gameOverCanvas.SetActive(true);
        }
    }
}
