using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Este enum sera el que le pasemos desde el hero.controller al metodo play. para indicar la animacion a reproducir.
public enum AnimationId
{
    Idle = 0,
    Run = 1,
    PrepareJump = 2,
    Jump = 3,
    Attack = 4,
    Hurt = 5,
    UsePowerUp = 6,
    Rise = 7,
    Walk = 8,
    LookATarget = 9
}

public class AnimatorController : MonoBehaviour
{
    Animator animator;

    //En este metodo recuperamos el componente del animator del ide de unity, para asignarselo a la variable animator
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    //Metodo para pausar la animacion cuando deje se usarla y que de esta manera libere recursos
    public void Pause()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();

        }
        animator.speed = 0;
    }

    //Metodo para despausar la animacion
    public void Unpause()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();

        }
        animator.speed = 1;
    }

    //Este metodo se encarga de reproducir las animaciones que le pasemos desde el controlador.
    public void Play(AnimationId animationId)
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();

        }
        animator.Play(animationId.ToString());
    }
}
