using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerChecker : MonoBehaviour
{
    //Enumerador para el circulo en la colision
    public enum LayerChekerType
    {
        Ray,
        Circle
    }

    [SerializeField] LayerChekerType layerChekerType;

    [SerializeField] LayerMask targetMask;  //Permite mostrar el raycast en el ide.
    [SerializeField] Vector2 direction;     //Direccion hasta donde queremos que llege el Raycast para comprobar si toca.
    [SerializeField] float distance;        //Distancia hasta donde queremos que llege el Raycast para comprobar si toca.

    public bool isTouching; //Para comprobar si esta tocando el suelo.

    void Update()
    {
        if (layerChekerType == LayerChekerType.Ray) //Si es un rayo nuestra seleccion sera para los pies
        {
            isTouching = Physics2D.Raycast(this.transform.position, direction, distance, targetMask);
        }
        if (layerChekerType == LayerChekerType.Circle)  //Si es un circulo nuestra seleccion sera para loa colision frontal
        {
            isTouching = Physics2D.OverlapCircle(this.transform.position, distance, targetMask);
        }

        //isTouching = Physics2D.Raycast(this.transform.position, direction, distance, targetMask);
        //Para comprobar las fisicas y si estamos tocando, usamos la clase Physics2D y le pasamos como argumentos la                                                                                                 
        //posicion del objeto a comprobar, la direccion, distancia y el targetMask.
    }


    //Esta funcion es para depurar visualmente en el id los scripts.
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (isTouching)
        {
            Gizmos.color = Color.green;
        }
        else {
            Gizmos.color = Color.yellow;

        }
        if (layerChekerType == LayerChekerType.Ray)
        {

            Gizmos.DrawRay(this.transform.position, direction * distance);
        }
        if (layerChekerType == LayerChekerType.Circle)
        {

            Gizmos.DrawWireSphere(this.transform.position, distance);
        }
    }
#endif
}