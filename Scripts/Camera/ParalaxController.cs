using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxController : MonoBehaviour
{
    [SerializeField] Vector3 parallaxFactor;
    [SerializeField] float speed;   //Velocidad a la que se mueve
    [SerializeField] float offsetZ;


    Vector3 lastPosition;   //Ultima posicion del gameObject
    void Start()
    {
        lastPosition = Camera.main.transform.position;  //Recupera la posicion de la camara
        lastPosition.z = 0;
        this.transform.localPosition = new Vector3(0, 0, offsetZ);  //Evita que la camara se mueva en el eje Z (el del 3d)
    }

    //Actualiza la posicion del fondo respecto a la camara
    void Update()
    {
        var cameraParallax = new Vector3(Camera.main.transform.position.x * parallaxFactor.x, Camera.main.transform.position.y * parallaxFactor.y, 0);  //Posicion actual
        this.transform.position -= (lastPosition - cameraParallax) * speed;     //Resta la ultima posicion de la camara con la posicion actual y lo multiplica por la velocidad                                                                                

        lastPosition = cameraParallax;
    }
}
