using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellLauncherController : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;   //Es el gameObject de un proyectil
    [SerializeField] float force;       //La fuerza con la que se mueve el proyectil

    //Este metodo manda el proyectil en la direccion que reciba como parametro
    public void Launch(Vector2 direction)
    {
        //GameObject go = Instantiate(projectilePrefab, this.transform.position, Quaternion.identity);
        GameObject go = Instantiate(projectilePrefab, this.transform.position, Quaternion.identity);    //Instancia el gameObject proyectil
        go.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);    //Aplica una fuerza hacia esa direccion
        go.GetComponent<ProyectilController>().SetDirection(direction);    //Indica la direccion del proyectil
    }
}
