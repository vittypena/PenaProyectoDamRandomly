using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour, ITargetCombat
{
    //Le introduzco la interfaz de que va a ser objetivo de combate

    //Enum con los distintos estados del Skeleton
    public enum SkeletonState
    {
        Inactive,
        Rise,
        WalkInTransformRight,
        Turn
    }

    [SerializeField] int health = 1;
    [SerializeField] SkeletonState skeletonState = SkeletonState.Inactive;  //Variable para el parametro del estado del esqueleto
    [SerializeField] DamageFeedbackEffect damageFeedbackEffect;     //Variable para dar el feedback del daño
    [SerializeField] AnimatorController animatorController;     //Controlador animator
    [SerializeField] float speed = 1;
    [SerializeField] GameObject destructionPrefab;      //Variable para ponerle el efecto de destruccion de un item
    [SerializeField] LayerChecker groundChecker;        //Variable para comprobar si esta tocando tierra
    [SerializeField] LayerChecker blockChecker;     //Variable para comprobar si hay una pared en la direccion a la que va

    private Rigidbody2D rigidbody2D;

    private bool active;

    private bool isExecutingState = false;      //Variable para indicar si ya ejecuto el estado

    //Este metodo se encarga de invocar la animacion de emerger de la tierra y de marcar el estado del skeleton como inactive, asi como de inicializar el RigidBody2D
    private void Awake()
    {
        skeletonState = SkeletonState.Inactive;
        animatorController.Pause();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    //Metodo para upgradear los estados segun va compiendo las distintas condiciones
    void Update()
    {
        if (active) //Si la camara esta colisionando
        {

            if (!isExecutingState && skeletonState == SkeletonState.Rise)       //Si no ha emergido emerge, la variable de si ha ejecutado el estado
            {
                Rise();
                isExecutingState = true;
            }

            if (skeletonState == SkeletonState.WalkInTransformRight)    //Si tiene que andar anda
            {
                WalkInTransformRight();
            }



        }
    }

    //Metodo para empezar a andar si esta en ese estado
    void WalkInTransformRight()
    {
        animatorController.Play(AnimationId.Walk);

        //  rigidbody2D.velocity = transform.right * speed;
        rigidbody2D.velocity = new Vector2(transform.right.x * speed, rigidbody2D.velocity.y);

        if (!groundChecker.isTouching || blockChecker.isTouching)   //Si no esta tocando ni suealo ni hay mas pared, da la vuelta
        {
            Turn();
        }

    }

    //Metodo para girar el personaje si se invoca
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

    //Cuando se llama este metodo el esqueleto emerge con una corrutina para ejecutar la animacion
    void Rise()
    {
        StartCoroutine(_Rise());
    }


    IEnumerator _Rise()    //Corrutina del metodo Rise, para ejecutar la animacion y nada mas acabarla pasar al estado andar
    {
        yield return new WaitForSeconds(0.2F);

        animatorController.Play(AnimationId.Rise);
        yield return new WaitForSeconds(1.15F);
        skeletonState = SkeletonState.WalkInTransformRight;     //Al acabar pasa a andar

    }

    //Metodo para controlar cuando colisiona el maincamera colisionador con el colisionador del esqueleto
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("MainCamera"))  //Si colisiono con la camara activa el el skeleton state
        {
            active = true;
            animatorController.Unpause();

        }

        if (collision.gameObject.tag.Equals("MainCamera") && skeletonState == SkeletonState.Inactive) //Si colisiono con la camara activa y la boleana de si ha resurgido ya no es true, deberia hacer el estado de emerger del suelo.
        {
            skeletonState = SkeletonState.Rise;


        }


    }

    //Este metodo es la contraparte del anterior, cuando la camara no colisione lo pondra en false.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("MainCamera"))
        {
            active = false;
            animatorController.Pause();
        }
    }

    //Cuando apliquen daño al esqueleto le bajaremos salud
    public void TakeDamage(int damagePoints)
    {
        health = Mathf.Clamp(health - damagePoints, 0, 100);
        damageFeedbackEffect.PlayDamageEffect();
        if (health <= 0)
        {
            if (destructionPrefab)          //Instancia el efecto del destruir del prefab que ya hice
            {
                Instantiate(destructionPrefab, (Vector2)this.transform.position + Vector2.up * 0.6F, Quaternion.identity);
            }
            Destroy(this.gameObject);
        }
    }
}
