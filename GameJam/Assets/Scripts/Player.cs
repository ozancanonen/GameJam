using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public string enemyBulletTag;
    public string horizontalMovementInputButtons;
    public string verticalMovementInputButtons;
    public string fireMovementInputButtons;
    public string Skill1MovementInputButtons;
    public string Skill2MovementInputButtons;

    public GameObject particleParentObject;
    public GameObject bullet;
    public GameObject bulletParticles;
    public GameObject deadParticles;
    public GameObject wall;
    public GameObject fakeWall;

    private bool immobolized = false;
    public float channelingTime;


    public Slider HealthSliderObject;
    public float moveSpeed;
    public float playerHealth;
    private Vector2 movement;
    public BoxCollider2D melee;
    public Transform firingPos;
    public Transform bulletSpawnPos;
    public Transform particleSpawnPos;
    private GameObject particleObject;
    private bool canShoot;
    private bool inMeleeRange;
    private Rigidbody2D meleeInteraction;

    private SpriteRenderer sp;
    private Rigidbody2D rb;
    private Animator anim;
    private GameManager gm;

    void Start()
    {
        canShoot = true;
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        sp = gameObject.GetComponent < SpriteRenderer>();
        gm = GetComponentInParent<GameManager>();

    }

    void Update()
    {
        if (!immobolized)
        {
            if (Input.GetButtonDown(fireMovementInputButtons))
            {
                if (canShoot)
                {
                    canShoot = false;
                    StartCoroutine(Shoot());
                }
            }

            if (Input.GetButtonDown(Skill1MovementInputButtons))
            {
                Wall();
            }
<<<<<<< Updated upstream
            if (Input.GetButtonDown(Skill2MovementInputButtons))
            {
                FakeWall();
            }
            GetCharacterInputs();
            Animate();
            ProjectileRotationManager();
=======
        }

        if (Input.GetButtonDown(Skill1MovementInputButtons))
        {
            Wall();
        }

        if (Input.GetButtonDown(Skill2MovementInputButtons))
        {
            FakeWall();
>>>>>>> Stashed changes
        }
    }

    //we are using FixedUpdate for all physical related stuff 
    void FixedUpdate()
    {
        //we make the player move according to the player Input, we multiplied the value with time to make the movement at a constant speed 
        //not relative to fps
        if (!immobolized)
            rb.MovePosition(rb.position + movement * moveSpeed*Time.fixedDeltaTime);
    }

    //returns a value between -1 and 1 according to the pressed buttons that are defined for Horizontal in Unity(they are changeable)
    //we assign these values to "movement"
    void GetCharacterInputs()
    {
        movement.x = Input.GetAxisRaw(horizontalMovementInputButtons);
        movement.y = Input.GetAxisRaw(verticalMovementInputButtons);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (gameObject.tag != col.gameObject.tag)
            if (col.gameObject.tag == "Player1" || col.gameObject.tag == "Player2")
            {
                inMeleeRange = true;
                meleeInteraction = col.attachedRigidbody;
            }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (gameObject.tag != col.gameObject.tag)
            if (col.gameObject.tag == "Player1" || col.gameObject.tag == "Player2")
                inMeleeRange = false;
    }
    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        HealthSliderObject.value = playerHealth;
        if (playerHealth <= 0)
        {
            Dead();
            gm.PlayerIsDeath(gameObject.tag);
            Destroy(gameObject);
        }
    }
    void ProjectileRotationManager()
    {
        //
        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        {
            if (movement.x > 0)
            {
                firingPos.rotation = Quaternion.Euler(0, 0,0 );
                //particleSpawnPos.rotation = Quaternion.Euler(0, 90f, 0);
            }
            else
            {
                firingPos.rotation = Quaternion.Euler(0, 0, 180);
                //particleSpawnPos.rotation = Quaternion.Euler(180, 90f, 0);
            }
        }
        if (Mathf.Abs(movement.y) > Mathf.Abs(movement.x))
        {
            if (movement.y > 0)
            {
                firingPos.rotation = Quaternion.Euler(0, 0, 90f);
                //particleSpawnPos.rotation = Quaternion.Euler(270f, 90f, 0);
            }
            else
            {
                firingPos.rotation = Quaternion.Euler(0, 0, -90f);
                //particleSpawnPos.rotation = Quaternion.Euler(90f, 90f, 0);
            }
        }
    }

    //we set the values we get from the player for the animator
    void Animate()
    {
        //To make the player look the way it last moved when stopped we don't make the movement Vector2 at he last frame to trigger
        //the right animation in the blend tree
        if (movement != Vector2.zero)
        {
            anim.SetFloat("Horizontal", movement.x);
            anim.SetFloat("Vertical", movement.y);
        } 
        anim.SetFloat("Speed", movement.magnitude);
    }

    IEnumerator Shoot()
    {
        channelingTime = 2.0f;
        if (inMeleeRange)
            meleeInteraction.AddRelativeForce(transform.right * 5000);
        yield return new WaitForSeconds(0.25f);
        immobolized = true;
        while(Input.GetButton(fireMovementInputButtons))
        {
            yield return new WaitForSeconds(channelingTime);
            Instantiate(bullet, bulletSpawnPos.position, firingPos.rotation);
            particleObject = Instantiate(bulletParticles, particleSpawnPos.position, particleSpawnPos.rotation);
            particleObject.transform.parent = particleParentObject.transform;
            StartCoroutine(DestroyThisAFter(particleObject, 1));
        }
        immobolized = false;
        canShoot = true;
    }

    void Dead()
    {
        particleObject=Instantiate(deadParticles, firingPos.position, firingPos.rotation);
        particleObject.transform.parent = particleParentObject.transform;
        StartCoroutine(DestroyThisAFter(particleObject, 1));
    }

    void Wall()
    {
        particleObject = Instantiate(wall, firingPos.position, firingPos.rotation);
        particleObject.transform.parent = particleParentObject.transform;
    }

    void FakeWall()
    {
        particleObject = Instantiate(fakeWall, firingPos.position, firingPos.rotation);
        particleObject.transform.parent = particleParentObject.transform;
    }

    IEnumerator DestroyThisAFter(GameObject thisObject,float destroyAfter)
    {
        yield return new WaitForSeconds(destroyAfter);
        Destroy(thisObject);
    }
}
