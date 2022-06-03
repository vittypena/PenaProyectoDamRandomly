using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDemo : MonoBehaviour, ITargetCombat
{
    [SerializeField] int health;
    [SerializeField] DamageFeedbackEffect damageFeedBackEffect;

    //Metodo para restar puntos de daño a la salud del objetivo
    public void TakeDamage(int damagePoints)
    {
        damageFeedBackEffect.PlayDamageEffect();    //Añade efectos del daño
        health -= damagePoints; //Resta los puntos de daño introducidos a la salud.
    }
}

