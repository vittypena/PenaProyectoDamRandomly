using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SephirotController : MonoBehaviour, ITargetCombat
{
    //Enum con los distintos estados
    public enum SephirotState
    {
        Presentation,
        Teleport,
        FadeIn,
        MoveInScenario,
        LaunchProjectiles,
        LaunchThunders,
        Die
    }

    [Header("Animation")]
    [SerializeField] AnimatorController animatorController;
    [SerializeField] GameObject dieEffectPrefab;
    [SerializeField] GameObject rewardPrefab;

    [Header("Projectile")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject thunderPrefab;
    [SerializeField] GameObject projectilePivot;
    [SerializeField] float projectileForce;


    [Header("Visual Effects")]
    [SerializeField] GameObject feedbackProjectile;     //Variable para el efecto del proyectil
    [SerializeField] AlphaBlinkEffect alphaBlinkEffect;
    [SerializeField] DamageFeedbackEffect damageFeedbackEffect;

    [Header("WayPoints")]   //Puntos de teletransporte
    [SerializeField] WayPointControllerManager wayPointsManager;

    [Header("Boss States")]
    public SephirotState sephirotState = SephirotState.Presentation;    //Referencia al enumerado de estados, inicia en presentacion

    [Header("Health")]      //Variables de salud
    [SerializeField] int health = 100;
    [SerializeField] int healthMax = 100;

    [Header("Boss States")]
    [SerializeField] bool debugMode = false;    //Variable para debugear

    [Header("Rigid Variables")] //Variabls para manipular la velocidad de movimiento del boss
    [SerializeField] float moveSpeed = 6;

    [Header("Music Variables")]
    [SerializeField] AudioClip dieSfx;
    [SerializeField] AudioClip fireballSfx;
    [SerializeField] AudioClip presentationSfx;
    [SerializeField] AudioClip bosshurtSfx;
    [SerializeField] AudioClip deathbossSfx;
    [SerializeField] AudioClip avisoSfx;
    [SerializeField] AudioClip directMeLordSfx;
    [SerializeField] AudioClip moveVoiceSfx;

    private bool stateExecuted = false;     //Indica si esta ejecutando un estado
    private Rigidbody2D rigidbody2D;

    private bool canFloat = false;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        if (!debugMode)
        {
            sephirotState = SephirotState.Presentation; //Inicializa en el estado presentacion
        }
        canFloat = true;
    }

    void Update()
    {
        if (sephirotState != SephirotState.Die)//Si no esta muerto
        {

            if (!stateExecuted && sephirotState == SephirotState.Presentation)  //Si esta en el estado de Presentacion ejecuta presentacion
            {
                Presentation();
                stateExecuted = true;
            }
            if (!stateExecuted && sephirotState == SephirotState.Teleport)     //Ejecuta el metodo de Teleport
            {
                Teleport();
                stateExecuted = true;
            }
            if (!stateExecuted && sephirotState == SephirotState.FadeIn)        //Ejecuta el metodo de Aparecer
            {
                FadeIn();
                stateExecuted = true;
            }

            if (!stateExecuted && sephirotState == SephirotState.MoveInScenario)    //Ejecuta el metodo de Moverse por el escenario
            {
                MoveInScenario();
                stateExecuted = true;
            }
            if (!stateExecuted && sephirotState == SephirotState.LaunchProjectiles)     //Ejecuta el metodo de lanzar proyectiles
            {
                LaunchProjectiles();
                stateExecuted = true;
            }
            if (!stateExecuted && sephirotState == SephirotState.LaunchThunders)    //Ejecuta el metodo de lanzar thunders
            {
                LaunchThunders();
                stateExecuted = true;
            }

            if (sephirotState != SephirotState.MoveInScenario && canFloat)
            {
                rigidbody2D.velocity = new Vector2(0, Mathf.Cos(Time.time));
            }
        }
    }    

    //Metodo lanzar rayos
    void LaunchThunders()
    {
        StartCoroutine(_LaunchThunders());

    }
    //Corutina lanzar rayos
    IEnumerator _LaunchThunders()
    {
        yield return new WaitForSeconds(0.4f);
        animatorController.Play(AnimationId.Idle);

        //Direccion al primer punto
        var target = wayPointsManager.GetThunderPoint(); 
        var direction = (target - (Vector2)this.transform.position).normalized;     //Variable para la direccion 
        HandleFlip(direction.x);

        canFloat = false;
        AudioManager.instance.PlaySfx(avisoSfx);        
        while (Vector2.Distance(this.transform.position, target) > 2f)
        {
            rigidbody2D.velocity = direction.normalized * moveSpeed;

            yield return new WaitForSeconds(Time.deltaTime);
        }
        var positionX = HeroController.instance.transform.position.x;       //Instancia el trueno en la posicion del heroe
        yield return new WaitForSeconds(0.4f);

        canFloat = true;

        for (int i = 0; i < 10; i++)    //Instancia un trueno por cada i en la posicionX
        {
            if (i == 5)
            {
                positionX += 4;
            }
            var thunder = Instantiate(thunderPrefab, new Vector3(positionX - i * 3, 18.84f, 0), Quaternion.identity);
            var thunder2 = Instantiate(thunderPrefab, new Vector3(positionX + i * 3, 18.84f, 0), Quaternion.identity);
            thunder.GetComponent<RedThunderController>().LaunchThunder();       //Lanza el trueno en la posicion
            thunder2.GetComponent<RedThunderController>().LaunchThunder();
            yield return new WaitForSeconds(Time.deltaTime);
        }

        //Dispara una segunda oleada
        yield return new WaitForSeconds(1);
        positionX = HeroController.instance.transform.position.x;       //Instancia el trueno en la posicion del heroe
        yield return new WaitForSeconds(0.6f);

        canFloat = true;

        for (int i = 0; i < 10; i++)    //Instancia un trueno por cada i en la posicionX
        {
            if (i == 5)
            {
                positionX += 2;
            }
            var thunder = Instantiate(thunderPrefab, new Vector3(positionX - i * 3, 18.84f, 0), Quaternion.identity);
            var thunder2 = Instantiate(thunderPrefab, new Vector3(positionX + i * 3, 18.84f, 0), Quaternion.identity);
            thunder.GetComponent<RedThunderController>().LaunchThunder();       //Lanza el trueno en la posicion
            thunder2.GetComponent<RedThunderController>().LaunchThunder();
            yield return new WaitForSeconds(Time.deltaTime);
        }

        //Dispara una tercera oleada
        yield return new WaitForSeconds(1);
        positionX = HeroController.instance.transform.position.x;       //Instancia el trueno en la posicion del heroe
        yield return new WaitForSeconds(0.6f);

        canFloat = true;

        for (int i = 0; i < 10; i++)    //Instancia un trueno por cada i en la posicionX
        {
            if (i == 5)
            {
                positionX += 4;
            }
            var thunder = Instantiate(thunderPrefab, new Vector3(positionX + i * 3, 18.84f, 0), Quaternion.identity);
            var thunder2 = Instantiate(thunderPrefab, new Vector3(positionX - i * 3, 18.84f, 0), Quaternion.identity);
            thunder.GetComponent<RedThunderController>().LaunchThunder();       //Lanza el trueno en la posicion
            thunder2.GetComponent<RedThunderController>().LaunchThunder();
            yield return new WaitForSeconds(Time.deltaTime);
        }

        //Dispara una cuarta oleada
        yield return new WaitForSeconds(1);
        positionX = HeroController.instance.transform.position.x;       //Instancia el trueno en la posicion del heroe
        yield return new WaitForSeconds(0.4f);

        canFloat = true;

        for (int i = 0; i < 10; i++)    //Instancia un trueno por cada i en la posicionX
        {
            if (i == 5)
            {
                positionX += 2;
            }
            var thunder = Instantiate(thunderPrefab, new Vector3(positionX - i * 3, 18.84f, 0), Quaternion.identity);
            var thunder2 = Instantiate(thunderPrefab, new Vector3(positionX + i * 3, 18.84f, 0), Quaternion.identity);
            thunder.GetComponent<RedThunderController>().LaunchThunder();       //Lanza el trueno en la posicion
            thunder2.GetComponent<RedThunderController>().LaunchThunder();
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitForSeconds(3f);

        if (Random.Range(-10, 10) > 0)  //Alterna entre los estados de teletransportarse y lanzar proyectil
        {
            sephirotState = SephirotState.Teleport;
        }
        else
        {
            sephirotState = SephirotState.LaunchProjectiles;
        }
        stateExecuted = false;
    }

    //Metodo de lanzar proyectiles
    void LaunchProjectiles()
    {
        StartCoroutine(_LaunchProjectiles());
    }

    //Corutina de lanzar proyectiles
    IEnumerator _LaunchProjectiles()
    {
        yield return new WaitForSeconds(0.4f);
        animatorController.Play(AnimationId.LookATarget);

        yield return new WaitForSeconds(0.4f);
        if (health < (int)(((float)healthMax) * 0.5f))
        {
            for (int i = 0; i < 5; i++)     //Arroja 5 proyectiles
            {
                feedbackProjectile.SetActive(true);     //Ejecuta la animacion de lanzar proyetil
                var direction = (HeroController.instance.transform.position - this.transform.position).normalized;      //Marca la direccion para arrojarlo en la direccion del hero
                HandleFlip(direction.x);        //Cambia la direccion de la animacion hacia donde apunta
                yield return new WaitForSeconds(0.6f);
                animatorController.Play(AnimationId.Attack);
                AudioManager.instance.PlaySfx(fireballSfx);
                yield return new WaitForSeconds(0.2f);
                var projectile = Instantiate(projectilePrefab, projectilePivot.transform.position, Quaternion.identity);         //Instancia el proyectil en la direccion marcada
                projectile.GetComponent<Rigidbody2D>().AddForce(direction * projectileForce, ForceMode2D.Impulse);          //Dispara el proyectil en esa direccion
                yield return new WaitForSeconds(0.4f);
                //Dispara un segundo proyectil
                animatorController.Play(AnimationId.Attack);
                AudioManager.instance.PlaySfx(fireballSfx);
                direction = (HeroController.instance.transform.position - this.transform.position).normalized;      //Marca la direccion para arrojarlo en la direccion del hero
                HandleFlip(direction.x);        //Cambia la direccion de la animacion hacia donde apunta
                projectile = Instantiate(projectilePrefab, projectilePivot.transform.position, Quaternion.identity);
                projectile.GetComponent<Rigidbody2D>().AddForce(direction * projectileForce, ForceMode2D.Impulse);
                yield return new WaitForSeconds(0.1f);
                direction = (HeroController.instance.transform.position - this.transform.position).normalized;      //Marca la direccion para arrojarlo en la direccion del hero
                HandleFlip(direction.x);        //Cambia la direccion de la animacion hacia donde apunta
                projectile = Instantiate(projectilePrefab, projectilePivot.transform.position, Quaternion.identity);
                projectile.GetComponent<Rigidbody2D>().AddForce(direction * projectileForce, ForceMode2D.Impulse);
                yield return new WaitForSeconds(0.4f);
                //Dispara un tercer proyectil
                animatorController.Play(AnimationId.Attack);
                AudioManager.instance.PlaySfx(fireballSfx);
                direction = (HeroController.instance.transform.position - this.transform.position).normalized;      //Marca la direccion para arrojarlo en la direccion del hero
                HandleFlip(direction.x);        //Cambia la direccion de la animacion hacia donde apunta
                projectile = Instantiate(projectilePrefab, projectilePivot.transform.position, Quaternion.identity);
                projectile.GetComponent<Rigidbody2D>().AddForce(direction * projectileForce, ForceMode2D.Impulse);
                yield return new WaitForSeconds(0.1f);
                animatorController.Play(AnimationId.LookATarget);
                feedbackProjectile.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)     //Arroja 5 proyectiles
            {
                feedbackProjectile.SetActive(true);     //Ejecuta la animacion de lanzar proyetil
                var direction = (HeroController.instance.transform.position - this.transform.position).normalized;      //Marca la direccion para arrojarlo en la direccion del hero
                HandleFlip(direction.x);        //Cambia la direccion de la animacion hacia donde apunta
                yield return new WaitForSeconds(0.6f);
                animatorController.Play(AnimationId.Attack);
                AudioManager.instance.PlaySfx(fireballSfx);
                yield return new WaitForSeconds(0.2f);
                var projectile = Instantiate(projectilePrefab, projectilePivot.transform.position, Quaternion.identity);         //Instancia el proyectil en la direccion marcada
                projectile.GetComponent<Rigidbody2D>().AddForce(direction * projectileForce, ForceMode2D.Impulse);          //Dispara el proyectil en esa direccion

                yield return new WaitForSeconds(0.1f);
                animatorController.Play(AnimationId.LookATarget);
                feedbackProjectile.SetActive(false);
            }
        }
            
        feedbackProjectile.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        if (Random.Range(-10, 10) > 0)
        {
            sephirotState = SephirotState.Teleport;
        }
        else
        {
            sephirotState = SephirotState.MoveInScenario;
        }
        stateExecuted = false;
    }

    //Metodo de moverse por el escenario
    void MoveInScenario()
    {
        StartCoroutine(_MoveInScenario());
    }

    IEnumerator _MoveInScenario()
    {
        animatorController.Play(AnimationId.LookATarget); //Ejecuta la animacion de buscar al target
        var target = wayPointsManager.GetRandomPoint(); //Busca un punto aleatorio para moverse
        yield return new WaitForSeconds(0.4f);
        AudioManager.instance.PlaySfx(moveVoiceSfx);
        var direction = (target - (Vector2)this.transform.position).normalized;     //Variable para la direccion 
        HandleFlip(direction.x);

        while (Vector2.Distance(this.transform.position, target) > 2f)  //Mientras la distancia entre los dos puntos, la posicion actual y el target sea mayor a 0.1,
                                                                        //mueve el boss hasta acercarse lo suficiente.
        {
            rigidbody2D.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed + Mathf.Cos(Time.time));
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return new WaitForSeconds(0.5f);

        if (health < (int)(((float)healthMax) * 0.5f))  //Si la vida es mayor del 50% ejecuta los proyectiles sino la fase final
        {
            sephirotState = SephirotState.LaunchThunders;
        }
        else
        {
            sephirotState = SephirotState.LaunchProjectiles;
        }
        animatorController.Play(AnimationId.Idle);
        stateExecuted = false;
    }

    //Metodo auxiliar del mover boss para que mire hacia donde se mueve
    void HandleFlip(float x)
    {
        if (x < 0)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.identity;
        }
    }

    //Metodo de Aparecer
    void FadeIn()
    {
        StartCoroutine(_FadeIn());
    }
    //Corutina de aparecer
    IEnumerator _FadeIn()   //Se encarga de hacerlo visible y escoger uno de los dos posibles siguientes estados
    {
        yield return new WaitForSeconds(1);
        alphaBlinkEffect.PlayBlinkEffect();

        yield return new WaitForSeconds(1);
        alphaBlinkEffect.BecomeVisible();
        yield return new WaitForSeconds(1);

        if (Random.Range(-10, 10) > 0)
        {
            sephirotState = SephirotState.MoveInScenario;   //estado moverse por el escenario
        }
        else
        {
            sephirotState = SephirotState.LaunchProjectiles;    //estado lanzar proyectiles

        }
        stateExecuted = false;
    }

    //Metodo de teleport
    void Teleport()
    {
        StartCoroutine(_Teleport());
    }
    //Corutina del teleport
    IEnumerator _Teleport() //Espera un segundo, parpadea, se vuelve invisible y se traslada a otro punto random, despues de esto pasa al estado aparecer
    {
        yield return new WaitForSeconds(1.5f);
        AudioManager.instance.PlaySfx(directMeLordSfx);
        alphaBlinkEffect.PlayBlinkEffect();
        yield return new WaitForSeconds(2);
        alphaBlinkEffect.BecomeInvisible(); //Metodo Volverse Invisible
        var teleportPoint = wayPointsManager.GetRandomPoint();

        this.transform.position = teleportPoint;    //Reasigna la posicion a ese punto

        sephirotState = SephirotState.FadeIn;   //Invoca el metodo  de aparecer
        stateExecuted = false;
    }

    //Metodo del estado de presentacion
    void Presentation()
        {
            StartCoroutine(_Presentation());
        }
    //Corutina del estado presentacion
    IEnumerator _Presentation() //Parpadea el sprite durante 5 segundos y ejecuta el estado de teleport
    {
        AudioManager.instance.PlaySfx(presentationSfx);
        alphaBlinkEffect.PlayBlinkEffect();
        yield return new WaitForSeconds(3);
        alphaBlinkEffect.StopBlinkEffect();
        sephirotState = SephirotState.Teleport;
        stateExecuted = false;
    }

    //Metodo para recibir danio
    public void TakeDamage(int damagePoints)
    {
        health = Mathf.Clamp(health - damagePoints, 0, healthMax);
        damageFeedbackEffect.PlayBlinkDamageEffect(0.8f);
        
        if (health <= 0)    //Si es derrotado, para todas las coroutines y ejecuta la de morir
        {        
        StopAllCoroutines();
        StartCoroutine(Die());
        }
        else
        {
            AudioManager.instance.PlaySfx(bosshurtSfx);
        }
    }

    //Corutina de Die
    IEnumerator Die()
    {
        damageFeedbackEffect.PlayBlinkDamageEffectBoss();       //Para los efectos de feedback del jugador

        rigidbody2D.velocity = Vector3.zero;   //Evitar que se pueda mover el boss al morir
        sephirotState = SephirotState.Die;      //Cambiar al estado de die
        AudioManager.instance.PlayMusic(null);      //Quita la musica de la instancia
        yield return new WaitForSeconds(1);

        for (int j = 0; j < 3; j++)     //Instancia 3 explosiones con 10 miniexplosiones como efecto visual
        {
            damageFeedbackEffect.PlayBlinkDamageEffectBoss();       //Para los efectos de feedback del jugador
            for (int i = 0; i < 10; i++)
            {
                Instantiate(rewardPrefab, this.transform.position + new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0), Quaternion.identity);     //Instancia el efecto de recompensa

                Instantiate(dieEffectPrefab, this.transform.position + new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0), Quaternion.identity);  //Instancia el efecto de morir prefab
                yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));

            }            
            yield return new WaitForSeconds(0.3f);            
        }
        AudioManager.instance.PlaySfx(deathbossSfx);
        yield return new WaitForSeconds(5.0f);
        AudioManager.instance.PlaySfx(dieSfx);  //Instancia la musica de haber derrotado al boss

        FindObjectOfType<LevelManager>().FinalBossWasVanquished();      //Libera las paredes del boss    
        gameObject.SetActive(false);
    }
}