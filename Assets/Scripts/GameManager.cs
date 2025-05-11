using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public AudioSource bgSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (bgSound != null && !bgSound.isPlaying)
            {
                bgSound.Play();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
