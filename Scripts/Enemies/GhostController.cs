using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour, ITargetCombat
{
    //Añado interfaz para recibir el daño
    //Distintos estados del automata (maquina controladora de estados)
    public enum GhostState
    {
        Inactive,
        Patrol,
        ChasePlayer
    }

    [SerializeField] WayPointControllerManager wayPointsManager;     //Punto que debe seguir en el movimiento
    [SerializeField] int health = 2;    //Salud del fantasma
    [SerializeField] GhostState ghostState = GhostState.Inactive;       //Estado del fantasma
    [SerializeField] DamageFeedbackEffect damageFeedbackEffect;
    [SerializeField] LayerChecker ghostVision;  //Variable para controlar el campo de vision del ghost
    [SerializeField] float speed = 1;
    [SerializeField] GameObject destructionPrefab;

    private Rigidbody2D rigidbody2D;

    private bool active;

    private bool isExecutingState = false;

    private Vector2 currentWayPoint;   

    Vector2 moveDirection;

    //Metodo para obtener el punto actual de movimiento y el rigidbody
    private void Awake()
    {
        currentWayPoint = wayPointsManager.GetRandomPoint();

        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    //Metodo para ir actualizando el estado del fantasma
    void Update()
    {
        if (active)
        {
            if (ghostState == GhostState.Patrol)
            {
                Patrol();
            }

            if (ghostState == GhostState.ChasePlayer)
            {
                ChasePlayer();
            }
        }
    }

    //Metodo para seguir al player mientras siga en el rango de vision
    void ChasePlayer()
    {
        var direction = (Vector2)HeroController.instance.transform.position - (Vector2)this.transform.position; //La direccion es la posicion del player

        rigidbody2D.velocity = direction.normalized * speed;
        
        if (direction.x < 0)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        
        if (!ghostVision.isTouching)    //Si no esta en rango de vision del jugador, pasa a patrullar
        {
            ghostState = GhostState.Patrol;
        }
    }

    //Metodo para cuando estemos en el estado de patrol(patrullar) que se mueva entre los distintos puntos de los wayPoint que hayamos puesto
    void Patrol()
    {
        var direction = currentWayPoint - (Vector2)this.transform.position;

        if (direction.x < 0)//If para rotar el fantasma segun la direccion a la que mire
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        rigidbody2D.velocity = direction.normalized * speed;
        if (Vector2.Distance(currentWayPoint, this.transform.position) < 0.1f)
        {
            currentWayPoint = wayPointsManager.GetRandomPoint();
        }

        if (ghostVision.isTouching)     //Cuando alcanza la camara al heroe, cambia el estado de dejar de patrullar a seguir al player, mientras le siga viendo
        {
            ghostState = GhostState.ChasePlayer;
        }
    }

    //Metodo para cuando entra la camara en contacto con el ghost para que empiece el estado de patrol
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("MainCamera"))
        {
            active = true;
        }

        if (collision.gameObject.tag.Equals("MainCamera") && ghostState == GhostState.Inactive)
        {
            ghostState = GhostState.Patrol;
        }

    }
    //Metodo que se aplica cuando salga del rango de la camara
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("MainCamera"))
        {
            active = false;
            rigidbody2D.velocity = Vector2.zero;
        }
    }

    //Metodo para recibir danio
    public void TakeDamage(int damagePoints)
    {
        health = Mathf.Clamp(health - damagePoints, 0, 100);
        damageFeedbackEffect.PlayDamageEffect();
        if (health <= 0)
        {
            if (destructionPrefab)
            {
                Instantiate(destructionPrefab, (Vector2)this.transform.position + Vector2.up * 0.6F, Quaternion.identity);
            }
            Destroy(this.gameObject);
        }
    }
}
