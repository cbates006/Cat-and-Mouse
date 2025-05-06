using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mouse : MonoBehaviour
{
    public Vector2 moveDirection;
    private float speed = 10;
    private float randomness;

    void Start()
    {
        // Set the initial move direction to a normalized vector (for example, upwards-right)
        moveDirection = Random.insideUnitCircle.normalized;


    }

    // Update is called once per frame
    void Update()
    {
        // Move the bullet in the current direction
        transform.Translate(moveDirection * speed * Time.deltaTime);

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        ReflectDirection(other.gameObject);
    }

    //ReflectDirection
    void ReflectDirection(GameObject wall)
    {
        /*
        randomness = Random.Range(-1.0f, 1.0f);
        moveDirection.x = moveDirection.x + randomness;
        moveDirection.y = moveDirection.y + randomness;
        
        if (moveDirection.y >= 1.0f)
        {
            moveDirection.y = 0.5f;
        }
        
        if (moveDirection.y <= -1.0f)
        {
            moveDirection.y = -0.5f;
        }
        
        if (moveDirection.x >= 1.0f)
        {
            moveDirection.x = 0.5f;
        }
        
        if (moveDirection.x <= -1.0f)
        {
            moveDirection.x = -0.5f;
        }*/

        // Reflect the direction of the bullet based on the wall hit. For top and bottom change Y direction and right and left change X direction
        if (wall.CompareTag("WallTop") || wall.CompareTag("WallBottom"))
        {
            moveDirection = new Vector2(moveDirection.x, -moveDirection.y);
            //Debug.Log(moveDirection);
        }
        else if (wall.CompareTag("WallLeft") || wall.CompareTag("WallRight"))
        {
            moveDirection = new Vector2(-moveDirection.x, moveDirection.y);
        }
    }
}
