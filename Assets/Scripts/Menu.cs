using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject start, quit;
    [SerializeField] private Animator Houranim;
    [SerializeField] private Image hour;
    [SerializeField] private float fadeDuration = 2f;

    private bool isFading = false;

    private void Start()
    {
        start.SetActive(false);
        quit.SetActive(false);
        Houranim.enabled = true;

        StartCoroutine(HourTransparent());
        
    }

    private IEnumerator HourTransparent()
    {
        isFading = true;
        float elapsed = 0f;
        Color originalColor = hour.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            hour.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        hour.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f); 
        start.SetActive(true);
        quit.SetActive(true);
        isFading = false;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
