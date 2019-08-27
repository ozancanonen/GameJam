﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construct : MonoBehaviour
{
    public Rigidbody2D rigidB;
    public float speed;
    private bool move;
    // Start is called before the first frame update
    
    public void Move(Vector3 direction)
    {
        print(rigidB.bodyType);
        rigidB.bodyType = RigidbodyType2D.Dynamic;
        rigidB.AddForce(new Vector2(direction.x,direction.y)*speed);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (move)
        {
            if (col.gameObject.tag == "Wall" || col.gameObject.tag == "Player1" || col.gameObject.tag == "Player2")
            {
                move = false;
                rigidB.velocity.Set(0, 0);
            }
        }
    }

}
