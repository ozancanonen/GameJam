﻿using System.Collections;
using System;
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
    public GameObject lanternObject;
    public GameObject bulletParticles;
    public GameObject deadParticles;
    public GameObject wall;
    public GameObject fakeWall;

    private bool doneChanneling;
    public float channelingTime;
    private float channelingTimeCounter;
    public float knockback;
    public float moveSpeed;
    public float playerHealth;
    public float lanternDurabilityMagnifier;
    private bool canShoot;
    private bool nearAlter;
    private bool lanternOn=true;
    private bool inMeleeRange;
    private bool immobolized = false;
    public Slider HealthSliderObject;
    public Slider lanternDurabilitySlider;
    private Vector2 movement;
    public BoxCollider2D melee;
    public Transform firingPos;
    public Transform bulletSpawnPos;
    public Transform particleSpawnPos;
    private GameObject particleObject;
    private Rigidbody2D meleeInteraction;

    private SpriteRenderer sp;
    private Rigidbody2D rb;
    private Animator anim;
    private GameManager gm;

    private float lanternDurability = 100;
    private float waitTime;
    private bool doingInteraction;
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
                    Attack();
                }
            }

            if (Input.GetButtonDown(Skill1MovementInputButtons))
            {
                StartCoroutine(buildConstruct(1f));
            }
            if (Input.GetButtonDown(Skill2MovementInputButtons))
            {
                Lantern();
            }
            GetCharacterInputs();
            Animate();
            ProjectileRotationManager();

        }

        if (nearAlter && lanternDurability < 100)
        {
            lanternDurability += lanternDurabilityMagnifier * Time.deltaTime;
            lanternDurabilitySlider.value = lanternDurability;
            if (lanternDurability >= 100)
            {
                Lantern();
            }
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
        {
            if (col.gameObject.tag == "Player1" || col.gameObject.tag == "Player2" || col.gameObject.tag == "Construct")
            {
                inMeleeRange = true;
                meleeInteraction = col.attachedRigidbody;
            }
        }
        if (col.tag == "Altar")
        {
            nearAlter = true;
        }
        if (col.tag == "fireCollider")
        {
            TakeDamage(100);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (gameObject.tag != col.gameObject.tag)
        {
            if (col.gameObject.tag == "Player1" || col.gameObject.tag == "Player2" || col.gameObject.tag == "Construct")
                inMeleeRange = false;
        }
        if (col.tag == "Altar")
        {
            nearAlter = false;
        }
    }
    public void TakeDamage(int damage)
    {
        if (lanternOn)
        {
            Lantern();
            lanternDurability = 0;
            lanternDurabilitySlider.value = lanternDurability;
        }
        else
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
    }

    void Lantern()
    {
        if (lanternOn)
        {
                lanternObject.SetActive(false);
                lanternOn = false;
        }
        else
        {
            if (lanternDurability >= 100)
            {
                lanternObject.SetActive(true);
                lanternOn = true;
            }
        }
    }


    void ProjectileRotationManager()
    {
        //d press
        if (movement == new Vector2(1, 0))
            firingPos.rotation = Quaternion.Euler(0, 0, 0);
        //a press
        if (movement == new Vector2(-1, 0))
            firingPos.rotation = Quaternion.Euler(0, 0, 180);
        //w press
        if (movement == new Vector2(0, 1))
            firingPos.rotation = Quaternion.Euler(0, 0, 90);
        //s press
        if (movement == new Vector2(0, -1))
            firingPos.rotation = Quaternion.Euler(0, 0, 270);
        //d & w press
        if (movement == new Vector2(1, 1))
            firingPos.rotation = Quaternion.Euler(0, 0, 45);
        //d & s press
        if (movement == new Vector2(1, -1))
            firingPos.rotation = Quaternion.Euler(0, 0, 315);
        //a & w press
        if (movement == new Vector2(-1,1))
            firingPos.rotation = Quaternion.Euler(0, 0, 135);
        //a & s press
        if (movement == new Vector2(-1, -1))
            firingPos.rotation = Quaternion.Euler(0, 0, 225);
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

    private void Attack()
    {
        canShoot = false;
        waitTime = Time.fixedTime + 0.25f;
        channelingTimeCounter = channelingTime + Time.fixedTime;
        StartCoroutine(Melee());
        StartCoroutine(Channel());
    }
    IEnumerator Melee()
    {
        int i = 0;
        while (waitTime > Time.fixedTime)
        {
            if (!Input.GetButton(fireMovementInputButtons))
            {
                i++;
                if (i > 2 && inMeleeRange)
                {
                    Interaction();
                }
            }
            yield return null;
        }
    }
    void Firebolt()
    {
        Instantiate(bullet, bulletSpawnPos.position, firingPos.rotation);
        particleObject = Instantiate(bulletParticles, particleSpawnPos.position, particleSpawnPos.rotation);
        particleObject.transform.parent = particleParentObject.transform;
        StartCoroutine(gm.DestroyThisAFter(particleObject, 1));
        canShoot = true;
    }

    void Interaction ()
    {
        {
            
            switch (gameObject.tag)
            {
                case "Player1":
                    if (meleeInteraction.gameObject.tag == "Player2")
                    {
                        meleeInteraction.AddForce(ForceDirection() * knockback);
                        meleeInteraction.gameObject.GetComponent<Player>().Immobolize(1);
                        meleeInteraction.gameObject.GetComponent<Player>().TakeDamage(0);
                        
                    }
                    if (meleeInteraction.gameObject.tag == "Construct")
                    {
                        print("Hitting the Construct");
                        moveConstruct();
                    }
                    break;
                case "Player2":
                    if (meleeInteraction.gameObject.tag == "Player1")
                    {
                        meleeInteraction.AddForce(ForceDirection() * knockback);
                        meleeInteraction.gameObject.GetComponent<Player>().Immobolize(1);
                        meleeInteraction.gameObject.GetComponent<Player>().TakeDamage(0);

                    }
                    meleeInteraction.AddForce(ForceDirection() * knockback);
                    if (meleeInteraction.gameObject.tag == "Construct")
                    {
                        moveConstruct();
                    }
                    break;
            }
        }

    }

    private void moveConstruct()
    {
        Construct c = meleeInteraction.gameObject.GetComponent<Construct>();
        c.Move(ForceDirection(), firingPos.rotation);
    }
    //ForceDirection returns the Vecotor3 
    Vector3 ForceDirection()
    {
        float rot = firingPos.rotation.eulerAngles.z;
        Vector3 trans = new Vector3(0,0,0);
        switch (rot)
        {
            case 0:
            case 315:
            case 360:
                trans = transform.right;
                break;
            case 180:
            case 135:
                trans = -transform.right;
                break;
            case 90:
            case 45:
                trans = transform.up;
                break;
            case 270:
            case 225:
                trans = -transform.up;
                break;
        }
        return trans;
    }
    void Dead()
    {
        particleObject=Instantiate(deadParticles, firingPos.position, firingPos.rotation);
        particleObject.transform.parent = particleParentObject.transform;
        StartCoroutine(gm.DestroyThisAFter(particleObject, 1));
    }

    IEnumerator buildConstruct(float waitingtime)
    {
        StartCoroutine(Immobolize(waitingtime));
        yield return new WaitForSeconds(waitingtime);
        particleObject = Instantiate(wall, bulletSpawnPos.position, wall.transform.rotation);
        particleObject.GetComponent<Construct>().collisionDirection.transform.rotation = firingPos.rotation;
        particleObject.transform.parent = particleParentObject.transform;
    }
    IEnumerator Immobolize(float channeling)
    {
        immobolized = true;
        yield return new WaitForSeconds(channeling);
        immobolized = false;
    }
    IEnumerator Channel()
    {
        while (Input.GetButton(fireMovementInputButtons))
        {
            immobolized = true;
            if (channelingTimeCounter < Time.fixedTime)
            {
                Firebolt();
                canShoot = true;
                channelingTimeCounter = channelingTime + Time.fixedTime;
            }
            yield return null;
        }
        canShoot = true;
        immobolized = false;
    }
}
