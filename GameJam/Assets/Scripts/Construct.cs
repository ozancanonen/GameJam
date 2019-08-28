﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construct : MonoBehaviour
{
    public Rigidbody2D rigidB;
    public float speed;
    private bool move;
    private Vector3 direction;
    public GameObject collisionDirection;
    private GameManager gm;
    private bool bonfire;
    // Start is called before the first frame update

    private void Start()
    {
        gm = gameObject.GetComponentInParent<GameManager>();
    }
    public void Move(Vector3 d, Quaternion rotation)
    {
        print("I am moving");
        direction = d;
        collisionDirection.transform.rotation = rotation;
        if(move && !bonfire)
            StartCoroutine(Travel());
    }
    IEnumerator Travel()
    {
        while (move)
        {
            gameObject.transform.position = new Vector3((transform.position.x + (direction.x * speed / 90)), (transform.position.y + (direction.y * speed / 90)), transform.position.z);
            yield return null;
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Wall" || col.gameObject.tag == "Player1" || col.gameObject.tag == "Player2" || col.gameObject.tag == "Destroyer");
        move = false;

        if (col.gameObject.tag == "player1Bullet"|| col.gameObject.tag == "player2Bullet")
        {
            bonfire = true;
            move = false;
            GameObject particleObject = Instantiate(gm.flameParticle,gameObject.transform.position+new Vector3(0,0,-4f),
            Quaternion.Euler(270f,90f,0f));
            particleObject.transform.parent = gm.particleParentObject.transform;
            particleObject = Instantiate(gm.layer2Light, gameObject.transform.position,Quaternion.identity);
            particleObject.transform.parent = gm.particleParentObject.transform;
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            Destroy(col.gameObject);
            StartCoroutine(Death());
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Wall" || col.gameObject.tag == "Player1" || col.gameObject.tag == "Player2" || col.gameObject.tag == "Destroyer");
            move = true;
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(6);
        Destroy(gameObject);
    }
}
