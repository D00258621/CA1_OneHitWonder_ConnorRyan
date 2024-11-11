using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    //Setting Variables
    private float speed = 8f;
    //Variables changeable in Unity program
    [SerializeField] private float jumpHeight;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] TMP_Text HeadText;
    //Variables controlled here or by called Components
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;

    public int heads = 0;
    public int totalHeads = -1;

    //Variables that are able to be called anywhere within the project
    public GameObject attackPoint;
    public float radius;
    public LayerMask enemies;

    private void Start()
    {
        HeadText.text = heads + "/" + totalHeads;
        //Connecting to components of the Player Object to use information
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        if (totalHeads == -1)
        {
            totalHeads = GameObject.FindGameObjectsWithTag("Collectible").Length;
        }
    }
    private void Update()
    {

        //Locks character as to not move during attacks, so there isn't a character just gliding
        if (anim.GetBool("attacking") == true && isGrounded())
            speed = 0.0f;
        else if (anim.GetBool("attacking") != true && isGrounded())
            speed = 8.0f;

        //Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        //Change Character Orientation
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(3, 3, 1);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-3, 3, 1);

        //Setting animations to move 
        anim.SetBool("moving", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        //Initialises the Jump
        if (Input.GetKey(KeyCode.W) && isGrounded())
        {
            Jump();
        }

        //Attack animation
        if (Input.GetKey(KeyCode.Space)&&isGrounded())
        {
            anim.SetBool("attacking", true);
        }
        //Fixed bug where jumping and pressing attack at the right time saved an attack until jumping was finished
        if (!isGrounded())
        {
            anim.SetBool("attacking", false);
        }
        //Activates falling when character is falling
        if (!isGrounded() && !Input.GetKey(KeyCode.W))
        {
            anim.SetTrigger("falling");
        }
    }

    //Jump Method
    private void Jump()
    {
        speed = 8.0f;
        body.velocity = new Vector2(body.velocity.x, jumpHeight);
        anim.SetTrigger("jump");

    }

    //Dying when character is hit by Projectile
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            death();
        }
    }

    //Checks if Character is on the ground by shooting raycast below the character
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    } 

    //If player is hit by projectile he dies
    private void death()
    {
        anim.SetTrigger("death");
    }

    //adds text to the on screen display of collectibles picked up 
    public void addHeads()
    {
        heads++;
        HeadText.text = heads + "/" + totalHeads;
    }
    //Make game return to beginning after death 
    void GameLoop()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Third-Party Code: [https://youtu.be/rwO3TE1G3ag?si=WWjuegrYw0xNzL4F] 
    // Description: [Creates an attack point that when swinging the sword my character can get "hit" the enemy]
    // The code used was both Attack and endAttack
    //Attack Method
    public void Attack()
    {
        Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPoint.transform.position, radius, enemies);

        foreach(Collider2D enemyGameObject in enemy)
        {
            Debug.Log("Enemy Hit");
            enemyGameObject.GetComponent<Enemy>().health -= 100;
        }
    }

    //Stops attacking animation freezing or looping for no reason
    public void endAttack()
    {
        anim.SetBool("attacking", false);
    }

    //Was used to viusalise the hit box of characters attack
/*    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, radius);
    }*/
}
