using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDemo : MonoBehaviour, ITargetCombat
{
    [SerializeField] int health;
    [SerializeField] DamageFeedbackEffect damageFeedBackEffect;

    //Metodo para restar puntos de da�o a la salud del objetivo
    public void TakeDamage(int damagePoints)
    {
        damageFeedBackEffect.PlayDamageEffect();    //A�ade efectos del da�o
        health -= damagePoints; //Resta los puntos de da�o introducidos a la salud.
    }
}

