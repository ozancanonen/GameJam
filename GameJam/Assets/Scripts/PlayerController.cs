using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private Vector2 movement;

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
        GetCharacterInputs();
        Animate();
        FlipCharacterinXAxisIfNeeded();
    }

    //we are using FixedUpdate for all physical related stuff 
    void FixedUpdate()
    {
        //we make the player move according to the player Input, we multiplied the value with time to make the movement at a constant speed 
        //not relative to fps
        rb.MovePosition(rb.position + movement * moveSpeed*Time.fixedDeltaTime);
    }


    void GetCharacterInputs()
    {
        //returns a value between -1 and 1 according to the pressed buttons that are defined for Horizontal in Unity(they are changeable)
        //we assign these values to "movement"

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FlipCharacterinXAxisIfNeeded()
    {
        //i think if we make the animation itself with the mirrored sprites the code will be cleaner but for now i think we can use this 
        if (movement.x < 0)
        {
            sp.flipX = true;
        }
        else
        {
            sp.flipX = false;
        }
    }

    void Animate()
    {
        //we set the values we get from the player for the animator
        if (movement != Vector2.zero)
        {
            anim.SetFloat("Horizontal", movement.x);
            anim.SetFloat("Vertical", movement.y);
        }
        anim.SetFloat("Speed", movement.magnitude);
        //i tried out the make the player look the way it just moved but for now it doesn't work compeletely i'm working on it

    }
}
