using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMain : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator door1;
    [SerializeField] private Animator gate1;
    [SerializeField] private Animator door2;
    [SerializeField] private Animator gate2;
    [SerializeField] private Animator door3;
    [SerializeField] private Animator gate3;
    private bool right = true;
    private Animator CharacterAnim;
    private int direction = 1;
    private float originalScaleX;
    private bool isgrounded = false;
    private float jumpForce = 5f;


    private void Start()
    {
        CharacterAnim = GetComponent<Animator>();
        CharacterAnim.SetBool("IsRunning", false);
        originalScaleX = transform.localScale.x; 
        rb = GetComponent<Rigidbody2D>();
        door1.enabled = false;
        gate1.enabled = false;
        door2.enabled = false;
        gate2.enabled = false;
        gate3.enabled = false;
        door3.enabled = false;
        CharacterAnim.SetBool("Hurt-Animation", false);
        CharacterAnim.SetBool("IsJumping", false);
    }
    private void Update()
    {
        if (isgrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        else
        {
            CharacterAnim.SetBool("IsJumping", false);
        }
    }
    private void FixedUpdate()
    {
        PlayerMove();
    }

    void PlayerMove()
    {
        float moveX = Input.GetAxis("Horizontal") * speed;
        Vector3 move = new Vector3(moveX, 0, 0);
        transform.position += move * Time.deltaTime;

        if (moveX > 0 && !right)
        {
            direction = 1;
            transform.localScale = new Vector3(originalScaleX * direction, transform.localScale.y, transform.localScale.z);
            right = true;
        }
        else if (moveX < 0 && right)
        {
            direction = -1;
            transform.localScale = new Vector3(originalScaleX * direction, transform.localScale.y, transform.localScale.z);
            right = false;
        }

        CharacterAnim.SetBool("IsRunning", moveX != 0);
    }

   void Jump()
    {
        Vector2 jump = new Vector2(0, jumpForce);
        rb.AddForce(jump, ForceMode2D.Impulse);
        isgrounded = false;
        CharacterAnim.SetBool("IsJumping", true);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isgrounded && collision.gameObject.CompareTag("Ground"))
        {
            isgrounded = true;
        }

        if (collision.gameObject.CompareTag("Lav"))
        {
            CharacterAnim.SetBool("IsHurt", true);
            StartCoroutine(RestartSceneAfterDelay(2f));

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Door1"))
        {
            door1.enabled = true;
            gate1.enabled = true;
        }
        if (collision.gameObject.CompareTag("Door2"))
        {
            door2.enabled = true;
            gate2.enabled = true;
        }
        if (collision.gameObject.CompareTag("Door3"))
        {
            door3.enabled = true;
            gate3.enabled = true;
        }

        if (collision.gameObject.CompareTag("bat"))
        {
            CharacterAnim.SetBool("IsHurt", true);
            StartCoroutine(RestartSceneAfterDelay(2f));

        }
    }


    IEnumerator RestartSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



}
