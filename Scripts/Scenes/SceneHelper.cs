using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHelper : MonoBehaviour
{
    private static SceneHelper _instance;   //variable para la instancia de la escena con el patron Singleton.

    public SceneId previousScene;

    //Metodo que crea la instancia de la escena
    public static SceneHelper instance
    {
        get
        {
            if (_instance == null)  //Si la instancia es null la encuentra en la escena actual
            {
                _instance = FindObjectOfType<SceneHelper>();

                if (_instance == null)  //Si aun asi no lo encuentra lo carga desde los recursos
                {
                    var go = new GameObject("SceneHelper");
                    go.AddComponent<SceneHelper>();

                    _instance = go.GetComponent<SceneHelper>();
                }
                DontDestroyOnLoad(_instance.gameObject);    //Evita destruir la escena al cargar
            }
            return _instance;
        }
    }

    public SceneId GetCurrentSceneId()
    {
        Enum.TryParse(SceneManager.GetActiveScene().name, out SceneId sceneId);
        return sceneId;
    }
    public void ReloadScene()
    {
        Enum.TryParse(SceneManager.GetActiveScene().name, out SceneId sceneId);
        StartCoroutine(_LoadScene(sceneId));
    }

    //Metodo para cargar la escena
    public void LoadScene(SceneId sceneId)
    {
        StartCoroutine(_LoadScene(sceneId));
    }

    //Corutina que carga la escena segun el sceneId que recibe de forma asincrona
    private IEnumerator _LoadScene(SceneId sceneId)
    {
        yield return LoadingScreen.instance._OnLoadScreen();    //Empieza a invocar la pantalla de carga con el fade
        Enum.TryParse(SceneManager.GetActiveScene().name, out previousScene);   //Nos da el valor de la escena que venimos, para recolocar el personaje

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneId.ToString()); //Carga la escena 

        if (Camera.main.GetComponent<CameraController>() != null)
        {
            Camera.main.GetComponent<CameraController>().FreezeCamera();    //Frezea la camara mientras carga la escena
        }
        if (HeroController.instance != null)
        {
            HeroController.instance.PutOutBoundaries();    //Posicion al jugador en la posicion inicial
        }

        while (!asyncLoad.isDone)   //Mientras no este lista la escena se queda en un loop
        {
            yield return null;
        }
        
        var list = FindObjectsOfType<PortalScene>().ToList();   //Busqueda de los portales en la escena y carga la posicion de spawn
        if (list != null)
        {
            try
            {
                var spawnPosition = list.Find(x => x.SceneToLoad() == previousScene).GetSpawnPosition();        //Recuperamos el portal que nos puede regresar al que nos ha traido               
                HeroController.instance.PutOnSpawnPosition(spawnPosition);         //Pone al heroe en la posicion inicial       
                Camera.main.GetComponent<CameraController>().UpdatePosition(spawnPosition); //Mueve la camara a la posicion inicial
            }
            catch (Exception ex)
            {
            }
        }
        yield return new WaitForSeconds(0.3f);
        yield return LoadingScreen.instance._OnLoadedScreen();    //Termina de quitar la pantalla de carga con el fade;
    }
}
