using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    private Vector2 movement;
    private Rigidbody2D rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //returns a value between -1 and 1 according to the pressed buttons that are defined for Horizontal in Unity(they are changeable)
        //we assign these values to "movement"
        movement.x= Input.GetAxisRaw("Horizontal");
        movement.y= Input.GetAxisRaw("Vertical");
    }

    //we are using FixedUpdate for all physical related stuff 
    void FixedUpdate()
    {
        //we make the player move according to the player Input, we multiplied the value with time to make the movement at a constant speed 
        //not relative to fps
        rb.MovePosition(rb.position + movement * moveSpeed*Time.fixedDeltaTime);
    }


}
