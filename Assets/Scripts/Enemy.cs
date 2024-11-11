using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject projectile;
    public Transform projectilePos;

    private GameObject player;
    private Rigidbody2D body;
    private Animator anim;
    private float timer;
    private bool deathCheck = false;
    public float health;
    // Start is called before the first frame update
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
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && !deathCheck)
        {
            deathCheck = true;
            anim.SetTrigger("Edeath");
            
        }

        float distance = Vector2.Distance(transform.position, player.transform.position);
        
        if (distance < 30)
        {
            timer += Time.deltaTime;

            if (timer > 4)
            {
                timer = 0;
                anim.SetBool("Eattack", true);
            }
        }
    }

    //Called By the animator
    void Shoot()
    {
        Instantiate(projectile, projectilePos.position, Quaternion.identity);
        anim.SetBool("Eattack", false);
    }

    //Death 
    void death()
    {
        Destroy(gameObject);
    }
}
