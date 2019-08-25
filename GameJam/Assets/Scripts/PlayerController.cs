using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public string enemyBulletTag;
    public string horizontalMovementInputButtons;
    public string verticalMovementInputButtons;
    public GameObject bullet;
    public GameObject wall;
    public GameObject fakeWall;
    public Slider HealthSliderObject;
    public float moveSpeed;
    public float playerHealth=100;
    private Vector2 movement;
    public Transform firingPos;

    private SpriteRenderer sp;
    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        sp = gameObject.GetComponent < SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
        if (Input.GetButtonDown("Jump"))
        {
            Wall();
        }
        if (Input.GetButtonDown("Fire3"))
        {
            FakeWall();
        }
        GetCharacterInputs();
        Animate();
        ProjectileRotationManager();
    }

    //we are using FixedUpdate for all physical related stuff 
    void FixedUpdate()
    {
        //we make the player move according to the player Input, we multiplied the value with time to make the movement at a constant speed 
        //not relative to fps
        rb.MovePosition(rb.position + movement * moveSpeed*Time.fixedDeltaTime);
    }

    //returns a value between -1 and 1 according to the pressed buttons that are defined for Horizontal in Unity(they are changeable)
    //we assign these values to "movement"
    void GetCharacterInputs()
    {
        movement.x = Input.GetAxis(horizontalMovementInputButtons);
        movement.y = Input.GetAxis(verticalMovementInputButtons);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == enemyBulletTag)
        {
            playerHealth -= 10;
            HealthSliderObject.value = playerHealth;
            Destroy(col.gameObject);
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
            }
            else
            {
                firingPos.rotation = Quaternion.Euler(0, 0, 180);
            }
        }
        if (Mathf.Abs(movement.y) > Mathf.Abs(movement.x))
        {
            if (movement.y > 0)
            {
                firingPos.rotation = Quaternion.Euler(0, 0, 90f);
            }
            else
            {
                firingPos.rotation = Quaternion.Euler(0, 0, 270f);
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

    void Shoot()
    {
        Instantiate(bullet, firingPos.position, firingPos.rotation);
    }

    void Wall()
    {
        Instantiate(wall, firingPos.position, firingPos.rotation);
    }

    void FakeWall()
    {
        Instantiate(fakeWall, firingPos.position, firingPos.rotation);
    }
}
