using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]  //Para que la clase pueda ser manipulada desde el IDE
public class BoundaryRange  //Clase para el limite de la camara
{
    public float min;
    public float max;

}

[Serializable]              //Clase para el tama�o de la camara
public class CameraSize
{
    public float min;
    public float max;

}

public class CameraController : MonoBehaviour
{
    [SerializeField] TagId target;      //Esta variable sirve para encontrar el gameObjectTarget de la camara
    [SerializeField] float offsetZ;     //Variable para controlar la camara en el eje Z, que es el eje del 3D
    [SerializeField] BoundaryRange boundaryX;   //Variable para los rangos de la camara en x
    [SerializeField] BoundaryRange boundaryY;   //Variable para los rangos de la camara en y
    [SerializeField] GameObject background; //Variable que inicializa el objeto background el cual contiene los fondos para poder cambiar el tamanio de la camara en el boss final
    [SerializeField] float sizeSpeed=1;     //Variable para el aumento de la camara
    [SerializeField] Vector2 scaleFactor;   //Variable para cambiar el background

    private GameObject targetGameObject;        //Variable para saber que item va a seguir la camara
    CameraSize cameraSizeX;     //Variable para indicar el tama�o de la camara en x
    CameraSize cameraSizeY;     //Variable para indicar el tama�o de la camara en y
    Vector3 vel;    //Variable para el vector de 3 posiciones

    private bool freezeCamera = false;  //Para que no siga al jugador al cargar la escena

    //Metodo para cambiar el tama�o de la camara en la habitacion del boss final
   public void ChangeCameraSize(float sizeCamera)
    {
        StartCoroutine(_ChangeCameraSize(sizeCamera));
    }
    //Corrutina del metodo ChangeCameraSize   
    IEnumerator _ChangeCameraSize(float sizeCamera)
    {
        var size = Camera.main.orthographicSize;    //Recupero el tamanio inicial de la camara
        while (size < sizeCamera)   //Mientras que no llegue al tama�o que he pasado por parametro
        {
            size += sizeSpeed;  //Le suma el parametro que le haya pasado
            Camera.main.orthographicSize = size;    //Reasisgno el valor
            background.transform.localScale = new Vector3(size * scaleFactor.x, size * scaleFactor.y, 1);   //Cambio el tamanio del background acorde a la camara
            GetSize();
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    void Start()
    {
        targetGameObject = GameObject.FindGameObjectWithTag(target.ToString()); //Busca el target de la camara al empezar el juego.
        GetSize();
    }

    //Metodo para obtener los parametros del tama�o de la camara en sus respectivos ejes e inicializarlos
    void GetSize()
    {

        cameraSizeX = new CameraSize();
        cameraSizeY = new CameraSize();

        cameraSizeX.min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - this.transform.position.x; //Se recupera la posicion inicial de la camara en el eje X
                                                                                                                //y se le resta la posicion actual de la camara
        cameraSizeX.max = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - this.transform.position.x; //Se recupera la posicion final de la camara en el eje X
                                                                                                                //y se le resta la posicion actual de la camara

        cameraSizeY.min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y - this.transform.position.y; //Se recupera la posicion inicial de la camara en el eje Y
                                                                                                                //y se le resta la posicion actual de la camara

        cameraSizeY.max = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - this.transform.position.y; //Se recupera la posicion final de la camara en el eje Y
                                                                                                                //y se le resta la posicion actual de la camara
    }

    //Metodo para actualizar la camara
    public void UpdatePosition(Vector2 position)
    {
        this.transform.position = (Vector3)position + new Vector3(0, 0, offsetZ);
    }

    //Metodo para activar el congelamiento de camara, por ejemplo al cargar la escena
    public void FreezeCamera()
    {
        freezeCamera = true;
    }

    //Metodo para que la camara vaya actualizando la posicion
    void Update()
    {
        if (targetGameObject&&!freezeCamera)  //Si el target no es nulo y la camara no esta congelada avanza
        {        
            var targetPosition = new Vector3(   //Variable para restringir los limites de la camara segun el size de la camara y la posicion del target al que sigue
                    Mathf.Clamp(targetGameObject.transform.position.x, boundaryX.min - cameraSizeX.min, boundaryX.max - cameraSizeX.max),
                    Mathf.Clamp(targetGameObject.transform.position.y, boundaryY.min - cameraSizeY.min, boundaryY.max - cameraSizeY.max),
                    offsetZ
                );

            this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPosition, ref vel, 0.3f);   //Indica la nueva posicion de la camara, recibiendo
                                                                                                                    //como parametro la posicion actual, como segundo la
                                                                                                                    //objetiva y como tercer parametro un vector de 3 posiciones
        }
    }

#if UNITY_EDITOR         //Para dibujar en el ide los limites en los que se puede mover la camara
    private void OnDrawGizmos()
    {
        var pointA = new Vector3(boundaryX.min, boundaryY.min);
        var pointB = new Vector3(boundaryX.min, boundaryY.max);
        Gizmos.DrawLine(pointA, pointB);       

        pointA = new Vector3(boundaryX.max, boundaryY.min);
        pointB = new Vector3(boundaryX.max, boundaryY.max);
        Gizmos.DrawLine(pointA, pointB);



        pointA = new Vector3(boundaryX.min, boundaryY.min);
        pointB = new Vector3(boundaryX.max, boundaryY.min);
        Gizmos.DrawLine(pointA, pointB);


        pointA = new Vector3(boundaryX.min, boundaryY.max);
        pointB = new Vector3(boundaryX.max, boundaryY.max);
        Gizmos.DrawLine(pointA, pointB);
    }

#endif

}
