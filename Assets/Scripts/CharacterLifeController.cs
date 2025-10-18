using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLifeController : MonoBehaviour
{
    private int totalLife;
    public int currentLife;
    void Start()
    {
        //otalLife = lifes.Length;
        currentLife = totalLife;
    }
    public void Getdamage()
    {
        if (currentLife > 0)
        {
            currentLife--;
        }
    }
    public void RegenLife()
    {
        if (currentLife < totalLife)
        {
            currentLife++;
        }
        
    }
}
