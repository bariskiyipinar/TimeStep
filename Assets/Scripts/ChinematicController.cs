using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class ChinematicController : MonoBehaviour
{
    private PlayableDirector chinematic;
    public GameObject player; 
    private MonoBehaviour playerScript;

    private void Start()
    {
        chinematic = GetComponent<PlayableDirector>();
        playerScript = player.GetComponent<PlayerFuture>(); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerScript.enabled = false; 
            chinematic.Play();

         
            chinematic.stopped += OnTimelineFinished;
           
        }
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        playerScript.enabled = true;
        chinematic.stopped -= OnTimelineFinished;

        SceneManager.LoadScene("Menü");
    }
}
