using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedThunderController : MonoBehaviour
{
    Animator animator;  //Variable para instanciar el animator
    [SerializeField] GameObject feedback;       //Para instanciar el feedback
    [SerializeField] GameObject firePrefab;     //Para instanciar el prefab
    [SerializeField] AudioClip thunderSfx;

    [Header("Debug")]
    public bool launch; //Variable para el modo debug

    //Recupero el componente animador y ejecuto la animacion Idle
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        animator.Play("Idle");
    }

    //Metodo para lanzar el rayo
    public void LaunchThunder()
    {
        StartCoroutine(_LaunchThunder());
    }
    //Corutina para lanzar el rayo
    public IEnumerator _LaunchThunder()
    {
        feedback.SetActive(true);   //Activo el feedback
        yield return new WaitForSeconds(1);
        feedback.SetActive(false);  //Desactivo el feedback
        AudioManager.instance.PlaySfx(thunderSfx);
        animator.Play("thunder");   //Carga la animacion
        yield return new WaitForSeconds(0.4f);
        Instantiate(firePrefab, feedback.transform.position, Quaternion.identity);  //Instancia el rayo en la posicion del feedback (pivot)
        //animator.Play("idle");
        Destroy(this.gameObject);
    }

    //Metodo para que si esta el modo debug activado lance rayos
    private void Update()
    {
        if (launch)
        {
            LaunchThunder();
            launch = false;
        }
    }
}
