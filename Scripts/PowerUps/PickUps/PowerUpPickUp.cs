using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickUp : MonoBehaviour
{
    [SerializeField] PowerUpId powerUpId;   //Variable para identificar al powerup (PowerUpId) es un enum
    [SerializeField] TagId playerTag;       //Variable para identificar el playerTag y que no pueda cogerlo un enemigo
    [SerializeField] TagId groundTag;       //Variable para identificar donde esta el suelo, y que el item se detenga contra el suelo

    [SerializeField] AudioClip pickSfx;     //Variable para el sonido al coger el item
    [SerializeField] int maxAmount = 10;    //Variable para saber cuantos powerUps tenemos

    //Metodo para comprobar que la colision de los objetos del powerup se hace con el player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag.Equals(groundTag.ToString()))  //Cuando colisione con el ground le anula la velocidad y gravedad para que no caiga
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        if (collision.gameObject.tag.Equals(playerTag.ToString()))   //Si colisiona con el heroe y no con un enemigo
        {
            var amount = Random.Range(5, maxAmount);
            GameManager.instance.UpdatePowerUp(powerUpId, amount);   //Para que actualice en el hud 

            HeroController.instance.ChangePowerUp(powerUpId, amount);   //Inicializo el metodo del heroController relacionado con este script
            AudioManager.instance.PlaySfx(pickSfx);     //Reproduce sonido al pillar el item
            Destroy(this.gameObject);       //Se destruye el item despues de consumirlo
        }
    }
}
