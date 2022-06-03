using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] AudioClip levelMusic;  //Variable que recibira el parametro de la musica ambiente

    [SerializeField] AudioClip finalBossMusic;  //Variable para la cancion del boss

    [SerializeField] float cameraSize;  //Variable del tamanio que le pondre a la camara desde el ide
    [SerializeField] GameObject finalBoss;
    [SerializeField] public bool isFinalBoss;

    void Start()
    {
        AudioManager.instance.PlayMusic(levelMusic); //Reproduce el sonido ambiente cuando se inicialice levelMusic
    }

    private bool activeBossFight;
    

    //Metodo para cuando derrote al enemigo que quite la musica del boss y los bloqueos del mapa
    public void FinalBossWasVanquished()
    {
        AudioManager.instance.PlayMusic(levelMusic);    
        var block = FindObjectOfType<BossBlock>();
        if (block)
        {
            block.StartUnlock();        //Desbloquea las puertas
        }
    }

    //Metodo para que cuando entre en el rango del boss empiece el combate y cambie la sala
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activeBossFight && isFinalBoss && collision.gameObject.tag.Equals(TagId.Player.ToString()))    //Si detecta al jugador y la batalla no esta activa  
        {
            activeBossFight = true; //Activa la variable de batalla

            AudioManager.instance.PlayMusic(finalBossMusic);    //Reproduce la musica al entrar en contacto con el player
            var block = FindObjectOfType<BossBlock>();  //Busco en la escena quien tiene el componente de los bloqueos
            if (block)  //Si el componente es de bloqueo lo bloquea
            {
                block.StartBlock();
            }
            if (finalBoss)  //Activa el boss
            {
               finalBoss.SetActive(true);
            }
            FindObjectOfType<CameraController>().ChangeCameraSize(cameraSize);  //Cambia el tamaño de la camara 
        }
    }
}
