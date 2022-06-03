using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFeedbackEffect : MonoBehaviour
{
    [SerializeField] float blinkSpeed = 10;     //Velocidad del parpadeo del efecto
    private bool playBlinkEffect;     //Para que el efecto no sea infinito
    private Renderer renderer;

    //Metodo para recuperar el componente del renderer del Sprite
    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }
    
    //Metodo para reproducir los efectos del daño
    public void PlayDamageEffect()
    {
        StartCoroutine(_PlayDamageEffect());    //Ejecutamos la corutina _PlayDamageEffect()
    }

    //Corrutina para que el color del sprite regrese a el color del sprite original despues de x tiempo
    public IEnumerator _PlayDamageEffect()
    {
        renderer.material.SetFloat("_FlashAmount", 1f);    //Ponemos el sprite en blanco
        yield return new WaitForSeconds(0.3f);
        renderer.material.SetFloat("_FlashAmount", 0f);  //Regresa el sprite al color original 
    }

    //Metodo para comenzar el parpadeo del efecto
    public void PlayBlinkDamageEffect()
    {
        playBlinkEffect = true;
        StartCoroutine(PlayBlinkDamageEffects());
    }

    public void PlayBlinkDamageEffectBoss()
    {
        playBlinkEffect = true;
        StartCoroutine(PlayBlinkDamageEffects());
    }

    //Metodo para los efectos del boss
    public void PlayBlinkDamageEffect(float time)
    {
        if (!playBlinkEffect)
        {
            playBlinkEffect = true;
            StartCoroutine(_PlayBlinkDamageEffect(time));
        }
    }
    private IEnumerator _PlayBlinkDamageEffect(float time)
    {
        float cosValue = 0;
        float timeTemp = 0;
        while (playBlinkEffect)
        {
            cosValue = Mathf.Cos(Time.time * blinkSpeed);

            renderer.material.SetFloat("_FlashAmount", cosValue < 0 ? 0 : cosValue);
            timeTemp += Time.deltaTime;
            if (timeTemp > time)
            {
                playBlinkEffect = false;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        StopBlinkDamageEffect();
    }

    //Metodo para parar el parpadeo del efecto
    public void StopBlinkDamageEffect()
    {
        playBlinkEffect = false;
        renderer.material.SetFloat("_FlashAmount", 0);
    }

    //Corrutina para el tiempo del parpadeo del efecto
    public IEnumerator PlayBlinkDamageEffects()
    {
        float cosValue = 0;
        while (playBlinkEffect) //Mientras el efecto este en true
        {
            cosValue = Mathf.Cos(Time.time * blinkSpeed);   //Multiplicamos el tiempo que dure, por la velocidad del parpadeo,
                                                            //Time.time es una clase de unity en C#.            
            renderer.material.SetFloat("_FlashAmount", cosValue<0?0:cosValue);  //If Ternario, si es menor que 0 se le aplica 0 y
                                                                                //si no el valor de cosValue, que es el parpadeo
            yield return !playBlinkEffect;     //Solo se sale cuando sea falso.            
        }
    }
}
