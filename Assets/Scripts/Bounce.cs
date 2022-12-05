using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public Animator anim;
    public PlayerController pc;
    bool bounce = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            pc.jumpHeight = pc.jumpHeight + 10;
            bounce = true;
            anim.SetBool("Bounce", bounce);

        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            pc.jumpHeight = pc.jumpHeight - 10;
            bounce = false;
            anim.SetBool("Bounce", bounce);

        }
    }
}
