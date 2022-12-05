using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector2 waypoint;
    public Vector2 waypoint2;
    private Vector2 currentTarget;
    public float speed = 2; 
    // Start is called before the first frame update
    void Start()
    {
        currentTarget = waypoint; 
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentTarget, Time.deltaTime * speed); 
        if (Vector2.Distance(transform.position, currentTarget) < 0.01f)
        {
            if (currentTarget == waypoint)
            {
                currentTarget = waypoint2; 
            }
            else if (currentTarget == waypoint2)
            {
                currentTarget = waypoint; 
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent = this.gameObject.transform;
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent = null;
        }
    }
}
