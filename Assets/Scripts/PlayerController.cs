using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] public float jumpHeight = 5;

    [SerializeField] private float radius;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask platformMask;
    [SerializeField] private Transform checkPos;

    private Rigidbody2D rBody;
    private Animator anim;
    public SpriteRenderer spriteRender;
    public CinemachineVirtualCamera follwingCamera;
    public Vector3 startPosition;

    public static PlayerController instance;

    private bool isGrounded = true;
    private bool jumped = false;
    private bool canMoveInAir = true;

    void Start()
    {
        instance = this;
        rBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            rBody.velocity += new Vector2(0, jumpHeight);
            jumped = true;
        }
        bool groundedGround = Physics2D.OverlapCircle(checkPos.position, radius, groundMask);
        bool groundedPlayform = Physics2D.OverlapCircle(checkPos.position, radius, platformMask);
        bool grounded = groundedGround || groundedPlayform;
        if (grounded && !isGrounded)
        {
            anim.SetTrigger("Landed");
            canMoveInAir = true;
        }
        else if (!grounded && isGrounded)
        {
            anim.SetTrigger(jumped ? "Jump" : "Falling");
            jumped = false;
        }
        isGrounded = grounded;
    }

    private void FixedUpdate()
    {
        int direction = Convert.ToInt32(Input.GetKey(KeyCode.D)) - Convert.ToInt32(Input.GetKey(KeyCode.A));
        if (canMoveInAir)
        {
            rBody.velocity = new Vector2(direction * speed, rBody.velocity.y);
            anim.SetBool("isMoving", direction != 0);

            if (direction != 0)
            {
                spriteRender.flipX = direction == -1;
            }
        }
        else if (rBody.velocity.y <= 0.1)
        {
            canMoveInAir = true;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(checkPos.position, Vector3.back, radius);
    }
#endif
    public void KillPlayer()
    {
        transform.position = startPosition;
    }

    public void PushFromExplosion(Vector2 pushAmount)
    {
        canMoveInAir = false;
        rBody.velocity = pushAmount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Damage Area")
        {
            KillPlayer();
        }
    }
}