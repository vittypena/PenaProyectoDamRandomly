using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastController : MonoBehaviour, ITargetCombat
{
    //Añado interfaz para recibir el daño
    //Distintos estados del automata (maquina controladora de estados)
    public enum HellGatoState
    {
        Inactive,
        ChasePlayer,
        WalkInTransformRight,
        Turn
    }

    [SerializeField] int health = 1;
    [SerializeField] HellGatoState hellGatoState = HellGatoState.Inactive;
    [SerializeField] DamageFeedbackEffect damageFeedbackEffect;
    [SerializeField] AnimatorController animatorController;
    [SerializeField] float speed = 4;
    [SerializeField] GameObject destructionPrefab;
    [SerializeField] LayerChecker groundChecker;
    [SerializeField] LayerChecker blockChecker;
    [SerializeField] LayerChecker visionRange;

    private Rigidbody2D rigidbody2D;

    private bool active;

    private bool isExecutingState = false;
    private void Awake()
    {
        hellGatoState = HellGatoState.Inactive;
        animatorController.Pause();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (active)
        {
            if (hellGatoState == HellGatoState.WalkInTransformRight)
            {
                WalkInTransformRight();

            }
            if (hellGatoState == HellGatoState.ChasePlayer)
            {
                ChasePlayer();

            }
        }
    }

    //Metodo para seguir al player mientras siga en el rango de vision
    void ChasePlayer()
    {
        var direction = (Vector2)HeroController.instance.transform.position - (Vector2)this.transform.position;

        rigidbody2D.velocity = new Vector2(direction.normalized.x * speed, rigidbody2D.velocity.y);
        if (direction.x < 0)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);

        }
        if (!visionRange.isTouching)
        {
            hellGatoState = HellGatoState.WalkInTransformRight;

        }
    }
    //Metodo para que camine hasta que encuentre un obstaculo o entre en rango de vision el heroe
    void WalkInTransformRight()
    {
        animatorController.Play(AnimationId.Idle);

        rigidbody2D.velocity = new Vector2(transform.right.x * speed, rigidbody2D.velocity.y);

        if (!groundChecker.isTouching || blockChecker.isTouching)
        {
            Turn();
        }

        if (visionRange.isTouching)
        {
            hellGatoState = HellGatoState.ChasePlayer;
        }
    }

    //Metodo para girar el sprite segun la direccion a la que mire
    void Turn()
    {
        if (this.transform.right == Vector3.right)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);

        }
    }


    //Metodo para que cuando colisione el rango de vision del beast con el heroe le pueda seguir
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("MainCamera"))
        {
            active = true;
            animatorController.Unpause();
        }

        if (collision.gameObject.tag.Equals("MainCamera") && hellGatoState == HellGatoState.Inactive)
        {
            hellGatoState = HellGatoState.WalkInTransformRight;
        }
    }

    //Metodo para que deje de seguirle cuando se salga del rango de vision
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("MainCamera"))
        {
            active = false;
            rigidbody2D.velocity = Vector2.zero;
            animatorController.Pause();
        }
    }

    //Metodo para poder recibir danio
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
