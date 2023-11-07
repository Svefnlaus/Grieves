using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Starteffect : MonoBehaviour
{
    public TextMeshProUGUI textToBlink;
    public float blinkInterval = 0.5f;
    public string sceneToLoad;
    public float waitTimeBeforeLoad = 2.0f; // Adjust the wait time as needed
    [SerializeField] private AudioSource start;

    private bool isBlinking = true;

    private void Start()
    {
        StartCoroutine(BlinkText());
    }

    private void Update()
    {
        if (isBlinking && Input.GetKeyDown(KeyCode.Space))
        {
            isBlinking = false;
            start.Play();
            StartCoroutine(LoadNextSceneAfterDelay());
        }
    }

    private IEnumerator BlinkText()
    {
        while (isBlinking)
        {
            textToBlink.enabled = !textToBlink.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private IEnumerator LoadNextSceneAfterDelay()
    {
        yield return new WaitForSeconds(waitTimeBeforeLoad);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
