using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour, ITargetCombat
{
    //[SerializeField] es para que podamos manipular y ver esta variable desde el inspector de Unity.
    public float fallMultiplier;
    public float lowJumpMultiplier;

    [SerializeField] 
    private GameManager gameManager;

    [Header("Power Up Variables")]
    [SerializeField] private PowerUpId _currentPowerUp; //Variable para el powerup   

    [SerializeField]
    private PowerUpId currentPowerUp
    {
        get
        {
            return _currentPowerUp;
        }
        set
        {
            if (_currentPowerUp != value)
            {
                GameManager.instance.UpdatePowerUp(value);
            }
            _currentPowerUp = value;
        }
    }

    [SerializeField] private int _powerUpAmount;    //Variable para saber cuantos items de powerUp ha consumido 

    [SerializeField]
    private int powerUpAmount
    {
        get
        {
            return _powerUpAmount;
        }

        set
        {
            if (value != _powerUpAmount)
            {
                GameManager.instance.UpdatePowerUp(value);
            }
            _powerUpAmount = value;
        }
    }

    [SerializeField] SpellLauncherController bluePotionLauncher;        //Variable con la referencia a el spellLauncherController

    [SerializeField] SpellLauncherController redPotionLauncher;

    private int _coins = 0;     //Variable para el conteo de las monedas    
    private int coins
    {
        get
        {
            return _coins;
        }
        set
        {
            if (_coins != value)
            {
                GameManager.instance.UpdateCoins(value);
            }

            _coins = value;
        }
    }


    [Header("Health Variables")]
    private bool died;
    [SerializeField]private int _health = 0;
    //El script Gamemanager gestiona la variable de salud de esata manera, gracias al get y al set.
    [SerializeField]
    int health
    {
        get
        {
            return _health;
        }
        set
        {
            if (_health != value)
            {
                GameManager.instance.UpdateHealth(value);
            }
            _health = value;
        }
    }

    [SerializeField] DamageFeedbackEffect damageFeedbackEffect;  //Referencia a la clase DamageFeedBack

    [Header("Attack Variables")]
    [SerializeField] SwordController swordController;   //Referencia a la logica de la espada con el da�o y la colision.

    [Header("Checked Variables")]
    [SerializeField] LayerChecker footA;
    [SerializeField] LayerChecker footB;

    [Header("Boolean Variables")]
    private bool jumpPressed = false; //Cuando se salte es true
    private bool playerIsOnGround;     //Variable que sera true cuando el jugador este en el suelo, lo cual podemos comprobar con el metodoHandleIsGrounding
    public bool canDoubleJump;      //Indica si puede hacer el doble salto.
    public bool playerIsRecovering; //Variable de recuperacion cuando el heroe recibe da�o, para que no pueda recibir da�o de otra fuente
    public bool playerIsAttacking;  //Variable para indicar que esta atacando
    public bool playerIsUsingPowerUp; //Variable para indicar que esta lanzando una magia
    public bool isLookingRight; //Variable para saber hacia donde mira el heroe, de esta manera se aplicara el retroceso en el sentido contrario al recibir 
                                //danio
    public bool isControlable = true;

    [Header("Animation Variables")]
    [SerializeField] AnimatorController animatorController;

    [Header("Interruption Variables")]
    public bool canCheckGround;
    public bool canMove;
    public bool canFlip;    //Variable para ver si puede girar el personaje

    [Header("Movement Variables")]
    [SerializeField] private float speed;   //Con esta variable indicamos con la rapidez que queremos que se mueva el gameObject.
    [SerializeField] private Vector2 movementDirection;     //Con esta variable recuperamos los datos del eje horizontal y vertical definido en unity
    private Rigidbody2D rigidbody2D;    //Clase que contiene funciones para mover el personaje.
    [SerializeField] private float jumpForce;   //Variable para indicar la fuerza del salto.    
    [SerializeField] private float doubleJumpForce; //Variable para indicar la fuerza del doble salto.
    [SerializeField] private float damageForce;    //Variable para saber con cuanta fuerza se va a hacer el empujon al recibir danio
    [SerializeField] private float damageForceUp;   //Variable para saber con cuanta fuerza hacia arriba se va a hacer el empujon al recibir danio

    [Header("Attack Variables")]
    private bool attackPressed = false;
    private bool usePowerUpPressed = false;     //Variable de la magia, si se preta la tecla se pone en true

    [Header("Audio Variables")]
    [SerializeField] AudioClip attackSfx;       //Variable para los efectos de sonido al atacar
    [SerializeField] AudioClip jumpSfx;    //Variable para los efectos de sonido al saltar
    [SerializeField] AudioClip dobleJumpSfx;    //Variable para los efectos de sonido al saltar doble
    [SerializeField] AudioClip hurtSfx;         //Variable para los efectos de sonido al ser herido
    [SerializeField] AudioClip dieSfx;         //Variable para los efectos de sonido al morir
    public static HeroController instance;

    
    private void Awake()
    {
        if (instance == null)
        {            
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }else{
            Destroy(this.gameObject);
        }

    
        SaveSystem.instance.Load(out HeroData heroData);
        if (heroData != null)
        {
            coins = heroData.coins;
            currentPowerUp = heroData.currentPowerUpId;
            powerUpAmount = heroData.powerUpAmount;
        }

        health = 10;
    }

    //Metodo que se ejecuta al comenzar el juego
    void Start()
    {
        canFlip = true;
        canCheckGround = true;
        canMove = true;
        rigidbody2D = GetComponent<Rigidbody2D>(); //Inicializamos la referencia al cuerpo del heroe, para poder moverlo.
        animatorController.Play(AnimationId.Idle);  //Al iniciar el juego la animacion es la de reposo
    }

    //Metodo que se ejecuta cada vez que hay un cambio en el juego
    void Update()
    {
        if (!died&&isControlable)  //Solo se pueden realizar acciones si no se ha muerto y es controlable
        {
            HandleIsGrounding();
            HandleControls();
            HandleMovement();
            HandleFlip();
            HandleJump();
            HandleAttack();
            HandleUsePowerUp();
        }        
    }

    //Metodo para hacer controlable al hero o no en funcion de las necesidades
    public void SetIsControlable(bool isControlable)
    {
        this.isControlable = isControlable;
        if (!this.isControlable)
        {
            StopAllCoroutines();
            animatorController.Play(AnimationId.Idle);
            rigidbody2D.velocity = Vector2.zero;
        }       
    }

    //Metodo para ir guardando el oro que consigue el heroe
    public void GiveCoin()
    {
        coins = Mathf.Clamp(coins + 1, 0, 10000000);
    }

    //Metodo para dar vida con el consumible de la pocion
    public void GiveHealthPoint()
    {
        health = Mathf.Clamp(health + 1, 0, 10);    //Suma vida del 1 al 10
    }

    //Metodo para asignar el valor de la variable de powerUpID
    public void ChangePowerUp(PowerUpId powerUpId, int amount)
    {
        currentPowerUp = powerUpId;
        powerUpAmount = amount;
        Debug.Log(currentPowerUp);
    }

    //Metodo para checkear que se pulsan las teclas de movimiento del heroe
    void HandleControls()
    {
        //Vector2 es una clase de Unity,
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));//Comprobar que se esta pretando las teclas de movimiento horizontal y vertical
        jumpPressed = Input.GetButtonDown("Jump");  //Obtenemos el input del salto
        attackPressed = Input.GetButtonDown("Attack");
        usePowerUpPressed = Input.GetButtonDown("UsePowerUp");  //Obtenemos el input de la magia
    }

    //Para controllar el movimiento del heroe, gracias al componente rigitbody2D que habiamos puesto previamente en el objeto hero.
    void HandleMovement()
    {
        if (!canMove) return;   //Si puede moverse porque esta atacando u otras condiciones return

        rigidbody2D.velocity = new Vector2(movementDirection.x * speed, rigidbody2D.velocity.y);//Mover cuando se pretan las teclas de movimiento

        //Animaciones al movernos
        if (playerIsOnGround)       //Solo se ejecutaran estas animaciones si estamos en el suelo
        {         
            if (Mathf.Abs(rigidbody2D.velocity.x) > 0)  //Cambiar animacion segun se esta quieto o en movimiento, si el rigidbody2d es mayor a 0 se esta moviendo, por lo tanto se pone la animacion de run.
            {
                animatorController.Play(AnimationId.Run);
            }
            else
            {
                if(Mathf.Abs(rigidbody2D.velocity.y)==0)
                animatorController.Play(AnimationId.Idle);
            }
        }
    }

    //Metodo que permite girar al personaje cuando pulsamos a y d
    void HandleFlip()
    {
        if (!canFlip) return;   //Si no puede girar el personaje retorna, por ejemplo en el caso del retroceso al recibir da�o, para que no mire hacia alli.

        if (rigidbody2D.velocity.magnitude > 0)     //Condici�n para saber que esta en movimiento el gameObject hero.
        {        
            if (rigidbody2D.velocity.x > 0)      //Si se esta moviendo la horizontal positivo, es decir la tecla d, que rote el dibujo
            {
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                isLookingRight = true;
            }
            else if (rigidbody2D.velocity.x < 0)
            {
                this.transform.rotation = Quaternion.Euler(0, 180, 0);
                isLookingRight = false;
            }
        }
    }

    //Metodo para el control del salto del personaje
    void HandleJump()
    {
        if (canDoubleJump && jumpPressed && !playerIsOnGround)
        {  //Para poder hacer el doble salto cuando canDoubleJump sea true y estemos en el aire
            this.rigidbody2D.velocity = Vector2.zero;
            this.rigidbody2D.AddForce(Vector2.up * doubleJumpForce, ForceMode2D.Impulse);   //Aplica la fuerza del doble salto
            AudioManager.instance.PlaySfx(dobleJumpSfx);            //Efecto de sonido doble salto
            canDoubleJump = false;   //Para evitar que haga doble salto infinito
        }

        if (jumpPressed && playerIsOnGround)
        {   //Cuando se salte y este en el suelo, podra saltar.
            AudioManager.instance.PlaySfx(jumpSfx);
            this.rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);     //Se a�ade una fuerza hacia arriba para saltar
            StartCoroutine(HandleJumpAnimation());
            AudioManager.instance.PlaySfx(jumpSfx);            //Efecto de sonido salto
            canDoubleJump = true;
        }
        //if (rigidbody2D.velocity.y < 0)
        //{
        //    rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        //}
        //else if (rigidbody2D.velocity.y > 0 && !Input.GetButton("Jump"))
        //{
        //    rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        //}
    }

    //Metodo que comprueba si el personaje esta tocando el suelo
    void HandleIsGrounding()
    {
        if (!canCheckGround) return;
        playerIsOnGround = footA.isTouching || footB.isTouching;
    }

    //Metodo IEnumerator, lo cual es una corutina, que sirve para que las animaciones del salto y preparar salto no se ejecuten en el mismo segundo si no que vaya una detras de otra
    IEnumerator HandleJumpAnimation()
    {
        canCheckGround = false;
        playerIsOnGround = false;
        if (!playerIsAttacking)
        {
            animatorController.Play(AnimationId.PrepareJump);
            yield return new WaitForSeconds(0.3f);//Provoca un retraso de 0.3 seg
        }
        if (!playerIsAttacking)
        {
            animatorController.Play(AnimationId.Jump);
        }
        canCheckGround = true;
    }

    //Metodo para atacar con el heroe
    void HandleAttack()
    {
        if (attackPressed && !playerIsAttacking)    //Comprueba que el usuario no este atacando para que no se pueda abusar del ataque y que el ataque este presionado.
        {
            if (playerIsOnGround)
            {
                rigidbody2D.velocity = Vector2.zero;    //Anula la velocidad para que no se desplace al atacar si esta en tierra
            }            
            AudioManager.instance.PlaySfx(attackSfx);    //Instanciamos el efecto de audio con nuestra clase audio manager y le pasamos como parametro attackSfx
            animatorController.Play(AnimationId.Attack);    //Inicia la animacion de ataque.
            playerIsAttacking = true;
            swordController.Attack(0.1f, 0.3f);       //Marca la duracion del ataque
            StartCoroutine(RestoreAttack());    
        }
    }

    //Corrutina que nos indica cuanto va a durar la animacion de ataque
    IEnumerator RestoreAttack()
    {
        if (playerIsOnGround)
        {
            canMove = false;        //Evitar el movimiento del heroe si esta atacando en tierra
        }
        yield return new WaitForSeconds(0.25f); //Duracion del ataque
        playerIsAttacking = false;  //Despues de el tiempo que dure el ataque, se podra volver a atacar

        if (!playerIsOnGround)    //Si no estamos en tierra, reestablecemos la animacion de salto al acabar la de ataque  
        {
            animatorController.Play(AnimationId.Jump);
        }
        canMove = true;
    }

    public void PutOutBoundaries()
    {
        canFlip = false;
        this.transform.position = new Vector3(-15, 0, 0);
        rigidbody2D.velocity = Vector2.zero;
    }
    public void PutOnSpawnPosition(Vector2 position)
    {
        canFlip = true;
        this.transform.position = position;
    }

    //Metodo para atacar con magia con el heroe
    void HandleUsePowerUp()
    {
        if (usePowerUpPressed && !playerIsUsingPowerUp && currentPowerUp != PowerUpId.Nothing && currentPowerUp!=0)    //Comprueba que el usuario no esta ya atacando
                                                                                                  //y que tiene pociones azules para usar la magia
        { 
            if (playerIsOnGround)
            {
                rigidbody2D.velocity = Vector2.zero;    //Anula la velocidad para que no se desplace al atacar si esta en tierra
            }
            AudioManager.instance.PlaySfx(attackSfx);    
            animatorController.Play(AnimationId.UsePowerUp);    //Inicia la animacion de la magia.
            playerIsUsingPowerUp = true;

            //Si tenemos el beneficio azul usaremos los stacks del azul sino, el de rojo
            if (currentPowerUp == PowerUpId.BluePotion)
            { 
                bluePotionLauncher.Launch((Vector2)transform.right + Vector2.up * 0.3f); //Utiliza el SpellLauncherController con una azul y le pasa la direccion
            }
            if (currentPowerUp == PowerUpId.RedPotion)
            {
                redPotionLauncher.Launch(transform.right);
            }

            StartCoroutine(RestoreUsePowerUp());

            powerUpAmount--;    //Reduzco las pociones despues de usar la magia
            if (powerUpAmount <= 0)
            {  //Si se llega a cero se le pone nothing para que no pueda usar hasta que coga otra pocion.            
                currentPowerUp = PowerUpId.Nothing;
            }
        }
    }

    //Corrutina que nos indica cuanto va a durar la animacion de ataque
    IEnumerator RestoreUsePowerUp()
    {
        if (playerIsOnGround)
        {
            canMove = false;        //Evitar el movimiento del heroe si esta atacando en tierra
        }
        yield return new WaitForSeconds(0.25f); //Duracion de la magia
        playerIsUsingPowerUp = false;  //Despues de el tiempo que dure la magia, se podra volver a atacar

        if (!playerIsOnGround)    //Si no estamos en tierra, reestablecemos la animacion de salto al acabar la de ataque  
        {
            animatorController.Play(AnimationId.Jump);
        }
        canMove = true;
    }

    //Este metodo es para cubrir el recibir da�o en el heroe, es necesario ya que hemos implementado la interfaz ITargetCombat
    public void TakeDamage(int damagePoints)
    {
        //Si el jugador no se esta recuperando, puede recibir da�o
        if (!playerIsRecovering&&!died)
        {
            health = Mathf.Clamp(health - damagePoints, 0, 10); //Variable para restar salud, el minimo de da�o a recibir sera 0, y el maximo 10,
                                                                // que es el tope de la vida
            if(health <= 0) //Si ha muerto se le para el movimiento
            {
                AudioManager.instance.PlaySfx(dieSfx);
                died = true;
            }                                                             
            StartCoroutine(StartPlayerRecover());           

            if (isLookingRight) //Aplica una fuerza al heroe hacia la direccion contraria que mire, para simular el retroceso
            {
                rigidbody2D.AddForce(Vector2.left * damageForce + Vector2.up * damageForceUp, ForceMode2D.Impulse);    //A�ade un impulso hacia la izquierda
                                                                                                                       //y arriba
            }
            else
            {
                rigidbody2D.AddForce(Vector2.right * damageForce + Vector2.up * damageForceUp, ForceMode2D.Impulse);
            }
        }
    }

    //Corrutina para indicar cuanto dura la recuperacion del heroe al recibir da�o
    IEnumerator StartPlayerRecover()
    {
        canMove = false;    //Apagamos el movimiento mientras dure el retroceso
        canFlip = false;    //Apagamos canFlip para que no mire hacia donde retrocede
        animatorController.Play(AnimationId.Hurt);  //Animacion al recibir da�o
        AudioManager.instance.PlaySfx(hurtSfx);     //Efecto de sonido al recibir da�o
        yield return new WaitForSeconds(0.2f);
        canMove = true;
        canFlip = true;
        rigidbody2D.velocity = Vector2.zero; //Apagamos la velocidad del Rigid para que aplique bien la fuerza
        playerIsRecovering = true;  //Habilita que no pueda recibir da�o.
        damageFeedbackEffect.PlayBlinkDamageEffect();  //Comienza el parpadeo de da�o 
        yield return new WaitForSeconds(1.5f);
        damageFeedbackEffect.StopBlinkDamageEffect();   //Para el parpadeo de da�o
        playerIsRecovering = false;
    }
}

