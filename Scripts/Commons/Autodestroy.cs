using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Este metodo es para destruir el item del fuego azul al colisionar contra el suelo y otros items como la explosion de los enemigos al morir
public class Autodestroy : MonoBehaviour
{
    [SerializeField] float delay = 3;   //El tiempo que quiero que siga vivo el game object
    [SerializeField] AudioClip sfx;


    private void Awake()
    {
        if (sfx)
        {
            AudioManager.instance.PlaySfx(sfx);
        }
        Destroy(this.gameObject, delay);
    }
}
