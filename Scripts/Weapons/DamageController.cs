using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    [SerializeField] int damagePoints = 10;

    [SerializeField] TagId targetTag;//SerializeField significa que se puede cambiar el valor de la variable en unity


    //Metodo para inicializar los parametros del colisionador del arma
    private void Awake()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)//Detecta cuando ocurre la colision del gameObject (espada en este caso) y realiza el danio a la salud
    {
        if (collision.gameObject.tag.Equals(targetTag.ToString()))  //Trae el tag del objeto con el que estamos colisionando y si es igual al del controlador comprueba la colision.
        {
            var component = collision.gameObject.GetComponent<ITargetCombat>();//Comprueba la colision

            if (component != null)  //Si hay colision, haz danio
            {
                component.TakeDamage(damagePoints); //Hacemos 10 de danio por golpe
            }
        }
    }

    //Metodo para el daño del suelo con pinchos
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals(targetTag.ToString()))
        {

            var component = collision.gameObject.GetComponent<ITargetCombat>();
            if (component != null)
            {
                component.TakeDamage(damagePoints);
            }


        }
    }
}
