using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    private Rigidbody2D body;
    private GameObject player;
    private Animator anim;
    private float timer;
    void Start()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player object not found! Make sure there is an object tagged 'Player' in the scene.");
            return;
        }

        // Calculate the direction toward the player
        Vector3 direction = player.transform.position - transform.position;

        // Move towards the player
        body.velocity = direction.normalized * speed;

        // Calculate the rotation angle in degrees 
        float turn = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply rotation to face the player
        transform.rotation = Quaternion.Euler(0, 180, turn-90);
    }
    //Projectile lifetime
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > 10)
        {
            Destroy(gameObject);
        }
    }
    //When the projectile hits an Object
    private void OnTriggerEnter2D(Collider2D collider)
    {
            anim.SetTrigger("OnHit");
            Destroy(gameObject);   
        
    }
}
