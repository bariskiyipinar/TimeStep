using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private GameObject AutoBulletPrefab;
    [SerializeField] private float rotationSpeed=360;

    private void Start()
    {
        StartCoroutine(FireRoutine());
    }



    private  IEnumerator FireRoutine()
    {
        while (true)
        {
            GameObject bullet = Instantiate(AutoBulletPrefab, attackPoint.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = Vector2.right * bulletSpeed;

            Destroy(bullet, 3f);
            
            yield return new WaitForSeconds(3f);
        }
    }
}
