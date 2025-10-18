using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollisionController : MonoBehaviour
{
    public string enemyTag, powerUpTag;
    public CharacterLifeController lifeController;
    public IEnumerator  OnTriggerEnter2D(Collider2D collision)
    {
        yield return new WaitForSeconds(.2f);
        if (collision.gameObject.tag == enemyTag)
        {
            //yield return new WaitForSeconds(0);
            lifeController.Getdamage();

        }

        if (collision.gameObject.tag == powerUpTag)
        {
            lifeController.RegenLife();
           // yield return new WaitForSeconds(0);
        }
    }
}
