using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construct : MonoBehaviour
{
    public Rigidbody2D rigidB;
    public float speed;
    private bool move;
    private Vector3 direction;
    public GameObject collisionDirection;
    // Start is called before the first frame update
    
    public void Move(Vector3 d, Quaternion rotation)
    {
        print("I am moving");
        direction = d;
        collisionDirection.transform.rotation = rotation;
        move = true;
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
            if (col.gameObject.tag == "Wall" || col.gameObject.tag == "Player1" || col.gameObject.tag == "Player2" || col.gameObject.tag == "Destroyer")
            {
                print("I have stopped");
                move = false;
            }
    }

}
