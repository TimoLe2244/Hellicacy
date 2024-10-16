using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class player  : MonoBehaviour
{
    [SerializeField] float walkSpd = 5f;
    [SerializeField] float sprintSpd = 8f;
    [SerializeField] bool canSprint = true;
    [SerializeField] bool isDash;
    [SerializeField] float dashAmount = 3f;
    [SerializeField] float dashCooldown = 2f;

    private bool canDash = true;
    private float lastDashTime; 
    public float currentSpd;

    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    Vector2 movement;

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        if (movement.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movement.x < 0)
        {
            spriteRenderer.flipX = false;
        }
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            isDash = true;
        }


    }

    private void FixedUpdate()
    {
        if (TestIfSprinting())
        {
            currentSpd = sprintSpd;
        }
        else
        {
            currentSpd = walkSpd;
        }

        rb.MovePosition(rb.position + movement.normalized * currentSpd * Time.fixedDeltaTime);

        if (isDash && canDash)
        {
            Dash();
        }
    }

    bool TestIfSprinting()
    {
        if (!canSprint) { return false; }

        if (Input.GetKey(KeyCode.LeftShift)) { return true; }

        return false;
    }

    void Dash() 
    {
        if (canDash)
        {
            rb.MovePosition(rb.position + movement.normalized * dashAmount);
            isDash = false;

            lastDashTime = Time.time;
            canDash = false;

            StartCoroutine(DashCooldown());
        }
    }

    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
