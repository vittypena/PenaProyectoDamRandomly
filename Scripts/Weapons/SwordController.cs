using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [SerializeField] int damagePoints = 1;

    [SerializeField] TagId targetTag;//SerializeField significa que se puede cambiar el valor de la variable en unity
    private Collider2D collider2D;

    //Metodo para inicializar los parametros del colisionador del arma
    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();        //Referencia al colisionador 2D del arma
        collider2D.enabled = false;     //Deshabilitamos en un principio el danio

    }

    //Metodo para habilitar y ejecutar el ataque
    public void Attack(float delay, float attackDuration)    //Le pasamos la duracion del ataque para volver a deshabilitar el colisionador
    {        
        StartCoroutine(_Attack(delay, attackDuration));    //Activa la corutina pasandole la duracion
    }

    //Corutina del ataque para calcular el tiempo que pasa atacando y deshabilitar el colisionador en ese periodo
    private IEnumerator _Attack(float delay, float attackDuration)   
    {
        yield return new WaitForSeconds(delay);
        collider2D.enabled = true;      //Activas el danio

        yield return new WaitForSeconds(attackDuration);    //Retraso con la duracion que pasen por argumento
        collider2D.enabled = false;     //Activa el colisionador
        yield return new WaitForSeconds(attackDuration);
        Awake();
    }

    private void OnTriggerEnter2D(Collider2D collision)//Detecta cuando ocurre la colision del gameObject (espada en este caso) y realiza el danio a la salud
    {
        if (collision.gameObject.tag.Equals(targetTag.ToString()))  //Trae el tag del objeto con el que estamos colisionando y si es igual al del controlador comprueba la colision.
        {
            var component = collision.gameObject.GetComponent<ITargetCombat>();//Comprueba la colision
            
            if (component != null)  //Si hay colision, haz danio
            {
                component.TakeDamage(damagePoints); //Hacemos 1 de danio por golpe
            }
        }
    }
}
