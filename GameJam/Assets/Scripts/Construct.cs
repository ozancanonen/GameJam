using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construct : MonoBehaviour
{
    public Rigidbody2D rigidB;
    public float speed;
    private bool move;
    // Start is called before the first frame update
    
    void Move(Vector3 direction)
    {
        move = true;
        rigidB.velocity.Set(direction.x*speed,direction.y*speed);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Wall" || col.gameObject.tag == "Player1" || col.gameObject.tag == "Player2")
        {
            move = false;
            rigidB.velocity.Set(0, 0);
        }
    }

}
