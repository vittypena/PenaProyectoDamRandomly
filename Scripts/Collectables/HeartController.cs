using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartController : MonoBehaviour
{
    [SerializeField] TagId playerTag;   
    [SerializeField] TagId groundTag;

    [SerializeField] AudioClip pickSfx;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag.Equals(groundTag.ToString()))  //Al chocar contra la tierra frena
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        if (collision.gameObject.tag.Equals(playerTag.ToString()))  //Cuando colisiona con el hero
        {
            HeroController.instance.GiveHealthPoint();  //Instancia el metodo de dar puntos de salud del heroController
            AudioManager.instance.PlaySfx(pickSfx);
            Destroy(this.gameObject);
        }
    }
}
