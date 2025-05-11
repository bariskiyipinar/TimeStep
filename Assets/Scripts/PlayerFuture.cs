using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFuture : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Rigidbody2D rb;
    private bool isgrounded = false;
    [SerializeField] private float jumpForce = 5f;
    private bool right = true;
    private float originalScaleX;
    private int direction = 1;
    private Animator CharacterAnim;
    [SerializeField]private Animator GunAnim;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float fireRange = 10f;
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] private GameObject GunFirePrefab;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScaleX = transform.localScale.x;
        CharacterAnim = GetComponent<Animator>();


        CharacterAnim.SetBool("IsWalking", false);
        CharacterAnim.SetBool("IsJumping", false);
        GunAnim.SetBool("GunIdle", true);
        GunAnim.SetBool("GunWalking", false);
    }

    private void Update()
    {
        if (isgrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }


        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        float distance = Vector3.Distance(attackPoint.position, mouseWorldPos);
        float moveX = Input.GetAxis("Horizontal");

        if (Input.GetMouseButtonDown(0) && distance < 3 &&   Mathf.Abs(moveX) > 0.01f)
        {
            

            GameObject firedBullet = Instantiate(GunFirePrefab, attackPoint.position, Quaternion.identity);
            Rigidbody2D rb = firedBullet.GetComponent<Rigidbody2D>();

            float GunFireSpeed = 5f;
            Vector2 dir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

            rb.AddForce(dir * GunFireSpeed, ForceMode2D.Impulse);
            Destroy(firedBullet, 2f);
            GunAttack();
        }

    }

    private void FixedUpdate()
    {
        Playermove();

       
    }

    void Playermove()
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

      
        bool isWalking = moveX != 0;
        CharacterAnim.SetBool("IsWalking", isWalking);


        GunAnim.SetBool("GunIdle", !isWalking);
        GunAnim.SetBool("GunWalking", isWalking);
    }

    void Jump()
    {
        Vector2 jump = new Vector2(0, jumpForce);
        rb.AddForce(jump, ForceMode2D.Impulse);
        isgrounded = false;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isgrounded && collision.gameObject.CompareTag("Ground"))
        {
            isgrounded = true;
        }

        if(collision.gameObject.CompareTag("AutoBullet"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);     
        }
    }
    void GunAttack()
    {

        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(attackPoint.position, direction, fireRange, hitLayers);

    }

   

}
