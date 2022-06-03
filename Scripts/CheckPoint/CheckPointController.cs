using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour, ISaveGameScreen
{
    //Variable para saber si se esta mostrando
    private bool screenIsShowing;

    //Metodo para que cuando se oculte la pantalla apage la variable y vuelva a poner al heroe como controlable
    public void OnHideScreen()
    {
        screenIsShowing = false;
        HeroController.instance.SetIsControlable(true);

    }

    //Cuando colisiona muestra la pantalla y controla si sigue mostrandose, asignando tambien al personaje como no controlable para que no se pueda mover.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals(TagId.Player.ToString()) && !screenIsShowing)
        {
            SaveGameScreen.instance.ShowScreen(this);
            SaveGameScreen.instance.ShowScreen(this);
            HeroController.instance.SetIsControlable(false);
            screenIsShowing = true;
        }
    }
}
