using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField]private Rigidbody2D rb;
    [SerializeField] private CharacterMovement characterMovement;
    void Start()
    {
        
    }
    void Update()
    {
        anim.SetFloat("velocity", Mathf.Abs(rb.linearVelocity.x));
        anim.SetFloat("fallvelocity", rb.linearVelocity.y);
        if (characterMovement.isGrounded)
        {
            anim.SetBool("grounded",true);
        }
        else
        {
            anim.SetBool("grounded", false);
        }
    }
    public void SetStartShootTriger()
    {
        anim.SetTrigger("startShoot");
    }
    public void SetEndtShootTriger()
    {
        anim.SetTrigger("endShoot");
    }
    public void SetDashtrigger()
    {
        anim.SetTrigger("dash");
    }
    public void SetEndDashtrigger()
    {
        anim.SetTrigger("endDash");
    }
    public void SettriggerJump()
    {
        anim.SetTrigger("jump");
    }

}
