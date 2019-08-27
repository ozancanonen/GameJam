using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float bulletSpeed;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * bulletSpeed;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (gameObject.tag == "player1Bullet")
            if(col.gameObject.tag == "Player2")
            {
            col.gameObject.GetComponent<Player>().TakeDamage(50,true);
            Destroy(gameObject);
            }
        if (gameObject.tag == "player2Bullet")
            if (col.gameObject.tag == "Player1")
            {
                col.gameObject.GetComponent<Player>().TakeDamage(50,true);
                Destroy(gameObject);
            }

    }


}
