using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBlock : MonoBehaviour
{
    [SerializeField] List<GameObject> tileMapBlock; //Creo una lista de gameobjects ( los bloques a bloquear )

    //Metodo para bloquear las entradas
    public void StartBlock()
    {
        StartCoroutine(_StartBlock());
    }
    //Corutina para bloquear las entradas
    private IEnumerator _StartBlock()
    {
        foreach (var block in tileMapBlock) //Por cada objeto en la lista
        {
            block.SetActive(true);  //Activa los gameObjects por lo tanto quedan bloqueadas las entradas
            block.GetComponent<AlphaBlinkEffect>().PlayBlinkEffect();   //Le pone el efecto
        }
        yield return new WaitForSeconds(2); //Espero 2 segundos
        foreach (var block in tileMapBlock)
        {
            block.GetComponent<AlphaBlinkEffect>().StopBlinkEffect();   //Para el efecto a los 2 segundos
        }
    }

    //Metodo inverso para desbloquear los bloques
    public void StartUnlock()
    {
        StartCoroutine(_StartUnlock());
    }
    private IEnumerator _StartUnlock()
    {
        foreach (var block in tileMapBlock)
        {
            block.SetActive(true);
            block.GetComponent<AlphaBlinkEffect>().PlayBlinkEffect();
        }
        yield return new WaitForSeconds(2);
        foreach (var block in tileMapBlock)
        {
            block.GetComponent<AlphaBlinkEffect>().StopBlinkEffect();
            block.SetActive(false);

        }

    }
}
