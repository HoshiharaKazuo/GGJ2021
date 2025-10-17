using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAtack : MonoBehaviour
{
    public float timeCharge;
    [SerializeField]private float charged_shoot_time_minimum;
    public GameObject bullet;
    public GameObject chargedBullet;
    public float bullet_normal_velocity;
    public float bullet_charged_velocity;
    public CharacterMovement characterMove;
    public CharacterAnimationController animController;
    private bool canIShoot = true;
    public void Sword()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Sword");
        }
    }
    public void Shoot()
    {

        if (Input.GetKey(KeyCode.E))
        {
            if (canIShoot)
            {
                animController.SetStartShootTriger();
                canIShoot = false;
            }
           
            timeCharge += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            StartCoroutine(ChargedShoot());
        }
    }
    public IEnumerator ChargedShoot()
    {
        if (timeCharge >= charged_shoot_time_minimum)
        {
            if ((characterMove.right)||(!characterMove.right && !characterMove.left))
            {
                yield return new WaitForSeconds(.5f);
                animController.SetEndtShootTriger();
                canIShoot = true;
                timeCharge = 0;
            }

            if (characterMove.left)
            {
                yield return new WaitForSeconds(.5f);
                animController.SetEndtShootTriger();
                canIShoot = true;
                timeCharge = 0;
            }
        }
        else
        {
          
            if(characterMove.right || (!characterMove.right && !characterMove.left))
            {
                yield return new WaitForSeconds(.5f);
                animController.SetEndtShootTriger();
                canIShoot = true;
                timeCharge = 0;
            }

            if (characterMove.left)
            {
                yield return new WaitForSeconds(.5f);
                animController.SetEndtShootTriger();
                canIShoot = true;
                timeCharge = 0;
            }
        }
    }
    public void InstantiateBulletEvent()
    {

        if (timeCharge >= charged_shoot_time_minimum)
        {
           
            if ((characterMove.right) || (!characterMove.right && !characterMove.left))
            {
                InsantiateBullet(bullet_charged_velocity, chargedBullet);
            }

            if (characterMove.left)
            {
                InsantiateBullet(-1 * bullet_charged_velocity, chargedBullet);
            }
        }
        else
        {
            Debug.Log("Normal Shoot");
          
            if (characterMove.right || (!characterMove.right && !characterMove.left))
            {
                InsantiateBullet(bullet_normal_velocity, bullet);
            }

            if (characterMove.left)
            {
                InsantiateBullet(-1 * bullet_normal_velocity, bullet);
            }
        }
    }
    private void InsantiateBullet(float velocity, GameObject bullet)
    {
        GameObject bl = Instantiate(bullet, transform.position, Quaternion.identity);
        bl.GetComponent<Rigidbody2D>().AddForce(new Vector2 (velocity,0));

    }
}
