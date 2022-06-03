using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFade : MonoBehaviour
{
    CanvasGroup canvasGroup;
    [SerializeField] float fadeSpeed;       //Velocidad del fade pasada por parametro

    //Obtiene el componente
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    //Metodo para meter el fade
    public void FadeIn()
    {
        if (!canvasGroup)
            canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(_FadeIn());
    }
    //Corutina para meter el fade
    public IEnumerator _FadeIn()
    {
        canvasGroup.alpha = 0;   //Cambia el valor del color al 0 por lo tanto no hay fade
        var alpha = canvasGroup.alpha;
        while (alpha < 1)   //Mientras no este el fade completo que lo vaya metiendo poco a poco
        {
            alpha += fadeSpeed;
            canvasGroup.alpha = alpha;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        canvasGroup.alpha = 1;
    }

    //Metodo para quitar el fade
    public void FadeOut()
    {
        if (!canvasGroup)
            canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(_FadeOut());
    }
    //Corutina para quitar el fade de forma inversa al FadeIn
    public IEnumerator _FadeOut()
    {
        Debug.Log("_FadeOut");
        canvasGroup.alpha = 1;
        var alpha = canvasGroup.alpha;
        while (alpha > 0)
        {
            Debug.Log("_FadeOut");

            alpha -= fadeSpeed;
            canvasGroup.alpha = alpha;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        canvasGroup.alpha = 0;
    }
}
