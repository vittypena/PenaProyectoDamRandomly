using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWayPoints : MonoBehaviour
{

    [SerializeField] float speed = 1;       //La velocidad a la que se movera
    [SerializeField] WayPointControllerManager wayPointsManager;     //Referencia al waypointsmanager.cs
    private Vector2 currentPosition;    //Posicion actual de la linea

    //Metodo para obtener la posicion actual
    private void Awake()
    {
        currentPosition = wayPointsManager.GetNextPoint();  
    }

    void Update()
    {
        if (Vector2.Distance(this.transform.position, currentPosition) > 0.1f)  //Si la distancia entre el currentPosition y la posicion actual es mayor a 0.1 no hemos llegado a nuestro destino,
                                                                                //por lo tanto avanza hacia la variable direccion, el cual sera el siguiente punto
        {
            var direction = currentPosition - (Vector2)this.transform.position;
            this.transform.Translate(direction.normalized * speed * Time.deltaTime);        //Indica con que rapidez se desplaza, el Time.deltaTime evita que si el pc es muy rapido
                                                                                            //se acelere el objet
        }
        else
        {
            currentPosition = wayPointsManager.GetNextPoint();      //Si esta en la posicion recupera el punto

        }
    }
}
