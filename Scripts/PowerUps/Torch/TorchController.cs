using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchController : MonoBehaviour, ITargetCombat
{
    [SerializeField] GameObject destructionEffect;  //Variable para el efecto cuando se destruya el item
    [SerializeField] AudioClip destructionSfx;  //Efecto de sonido

    //Variables que tendran los objetos coin y heart, para que al destruirse los cree aleatorio
    [SerializeField] GameObject coin;
    [SerializeField] GameObject heart;

    [SerializeField] List<GameObject> powerUps;     //Variable para introducir los powerUps

    public bool powerUpsOnly;   //Variable para indicar que solo va a dar powerUps
    public bool indestructible = false;

    //Metodo para recibir daño
    public void TakeDamage(int damagePoints)
    {
        Instantiate(destructionEffect, this.transform.position, Quaternion.identity);   //Instancia el efecto de destruirse en la posicion actual del item
        GameObject prefabToInstance = null; //Utiliza el resource donde se encuentran los items para instanciar los items aleatorios.

        if (!powerUpsOnly)      //Algoritmo para sacar un corazon o una manera de manera aleatoria segun un rango de numeros
        {
            if (Random.Range(-10, 10) > 0)
            {
                if (Random.Range(-10, 10) > 0)
                {
                    prefabToInstance = coin;
                }
                else
                {
                    if (Random.Range(-10, 10) > 0)
                    {
                        prefabToInstance = heart;
                    }
                }
            }
        }
        else
        {
             //Si no cumple las anteriores, selecciona un powerUps aleatorio a la fuerza (la pocion azul o la roja)            
                prefabToInstance = powerUps[Random.Range(0, powerUps.Count)];            
        }

        if (prefabToInstance != null)
        {
            Instantiate(prefabToInstance, this.transform.position, Quaternion.identity);

        }
        AudioManager.instance.PlaySfx(destructionSfx);
        if (!indestructible)
            Destroy(this.gameObject);
    }
}
