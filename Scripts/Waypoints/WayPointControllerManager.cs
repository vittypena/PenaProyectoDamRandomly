using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WayPointControllerManager : MonoBehaviour
{
    int indexPoint = 0; //Variable que nos indica el punto actual en el que se encuentra el elemento al que se le asigne el script

    //Este metodo nos otorga el proximo punto al que se tiene que mover segun la IA
    public Vector2 GetNextPoint()
    {
        if (indexPoint >= this.transform.childCount)    //Si el indice ya llego al limite reinicia el indice para evitar que sobrepase el limite
        {
            indexPoint = 0;
        }

        var position = this.transform.GetChild(indexPoint).transform.position;     //Recupero la posicion actual pasandole el punto donde se deberia encontrar
        indexPoint++;      //Sumamos 1 al indice

        return position;
    }

    //Obtiene un punto random entre los puntos disponibles
    public Vector2 GetRandomPoint()
    {
        return this.transform.GetChild(Random.Range(0, this.transform.childCount)).transform.position;
    }

    //Metodo para obtener el primer punto
    public Vector2 GetFirstPoint()
    {
        return this.transform.GetChild(0).transform.position;
    }

    //Metodo para ir a la posicion donde lanza el rayo el boss
    public Vector2 GetThunderPoint()
    {
        return this.transform.GetChild(9).transform.position;
    }

    //Esta funcion es para depurar visualmente el script en el ide 
    //Recorre todos los hijos de la ruta que le introduzcamos
    //Dibuja la ruta en un area dentro de una esfera mediante drawWireSphere, pasandole la posicion para que  la vaya dibujando en la esfera invisible
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int i = 0; i < this.transform.childCount; i++) { 
            Gizmos.DrawWireSphere(this.transform.GetChild(i).transform.position,0.3f);
            Handles.Label(this.transform.GetChild(i).transform.position," Point "+(i+1));
        }
    }
#endif
}
