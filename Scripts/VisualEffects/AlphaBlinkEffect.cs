using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AlphaBlinkEffect : MonoBehaviour
{
    [SerializeField] float blinkSpeed = 10;
    private Tilemap tilemapRenderer;

    private SpriteRenderer spriteRenderer;

    private bool playBlinkEffect;

    Color color;

    private void Awake()
    {
        if (GetComponent<Tilemap>() != null)
        {
            tilemapRenderer = GetComponent<Tilemap>();
            color = tilemapRenderer.color;
        }
        if (GetComponent<SpriteRenderer>() != null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            color = spriteRenderer.color;
        }
    }

    public void PlayDamageEffect()
    {
        StartCoroutine(_PlayDamageEffect());

    }

    public IEnumerator _PlayDamageEffect()
    {
        color.a = 0;
        if (tilemapRenderer)
            tilemapRenderer.color = color;

        if (spriteRenderer)
            spriteRenderer.color = color;
        yield return new WaitForSeconds(0.3f);
        color.a = 1;
        if (tilemapRenderer)
            tilemapRenderer.color = color;


        if (spriteRenderer)
            spriteRenderer.color = color;

    }



    public void PlayBlinkEffect()
    {
        playBlinkEffect = true;
        StartCoroutine(_PlayBlinkEffect());
    }

    public void StopBlinkEffect()
    {
        playBlinkEffect = false;
        color.a = 1;
        if (tilemapRenderer)
            tilemapRenderer.color = color;


        if (spriteRenderer)
            spriteRenderer.color = color;

    }


    public void BecomeInvisible()
    {
        playBlinkEffect = false;
        color.a = 0;
        if (tilemapRenderer)
            tilemapRenderer.color = color;


        if (spriteRenderer)
            spriteRenderer.color = color;

    }
    public void BecomeVisible()
    {
        playBlinkEffect = false;
        color.a = 1;
        if (tilemapRenderer)
            tilemapRenderer.color = color;


        if (spriteRenderer)
            spriteRenderer.color = color;

    }
    private IEnumerator _PlayBlinkEffect()
    {
        float cosValue = 0;

        while (playBlinkEffect)
        {
            cosValue = Mathf.Cos(Time.time * blinkSpeed);

            color.a = cosValue < 0 ? 0 : cosValue;
            if (tilemapRenderer)
                tilemapRenderer.color = color;


            if (spriteRenderer)
                spriteRenderer.color = color;

            yield return !playBlinkEffect;
        }
    }
}
