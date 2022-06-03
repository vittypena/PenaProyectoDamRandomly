using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectilController : MonoBehaviour
{
    
    [SerializeField] TagId targetTag;
    [SerializeField] int damagePoints = 10;
    [SerializeField] AudioClip damageSfx;

    [SerializeField] GameObject explosionPrefab;    //Explosion del proyectil al impactar en el suelo

    //Metodo para indicar la direccion del proyectil, para que lo gire si es necesario.
    public void SetDirection(Vector2 direction)
    {
        if (direction.x < 0)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);

        }
    }

    //Metodo para aplicar daño con el proyectil
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals(targetTag.ToString()))
        {

            var component = collision.gameObject.GetComponent<ITargetCombat>();
            if (component != null)
            {
                component.TakeDamage(damagePoints);
            }
            AudioManager.instance.PlaySfx(damageSfx);

            if (explosionPrefab)    //Si no es nulo, es decir el proyectil azul, hacemos una instancia.
            {
                Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
            }
            Destroy(this.gameObject);
        }
    }
 
}
