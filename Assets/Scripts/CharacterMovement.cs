using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public string groundTag;
    [SerializeField] private float jumpForce;
    [SerializeField] private float dashForce;
    [SerializeField] Rigidbody2D rb;
    public bool isGrounded;
    [SerializeField] private CharacterAnimationController animationController;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    private Vector3 m_Velocity = Vector3.zero;
    private float movement_event;
    public bool right;
    public bool left;
    public void Jump(float movement)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Jump");
            move(movement, true, false);
        }
    }
    public void DashEvent()
    {
        move(movement_event, false, true);
    }
    public void Dash(float movement)
    {

        if (Input.GetKeyDown(KeyCode.Q) && Mathf.Abs(rb.linearVelocity.x) > 0 )
        {
            movement_event = movement;
            animationController.SetDashtrigger();
        }
    }
    public void move(float move, bool jump, bool dash)
    {
        Vector3 targetVelocity = new Vector2(move * 10f, rb.linearVelocity.y);
        rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        if (jump)
        {
            isGrounded = false;
        }

        if (dash)
        {
            if (rb.linearVelocity.x == 0)
            {
                rb.AddForce(new Vector2(dashForce, 0f));
            }
            if (rb.linearVelocity.x > 0)
            {
                rb.AddForce(new Vector2(dashForce, 0f));
            }

            if (rb.linearVelocity.x < 0)
            {
                rb.AddForce(new Vector2(-dashForce, 0));
            }

            animationController.SetEndDashtrigger();
        }
    }
    public void CheckLookAt()
    {

        if (rb.linearVelocity.x > 0)
        {
            right = true;
            left = false;
        }

        if (rb.linearVelocity.x < 0)
        {
            right = false;
            left = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == groundTag)
        {
            Debug.Log("here");
            isGrounded = true;
          
            
        }
    }

    public void PerformJump()
    {
        rb.AddForce(new Vector2(0f, jumpForce));
    }
}
