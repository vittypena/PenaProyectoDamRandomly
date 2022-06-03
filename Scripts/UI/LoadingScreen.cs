using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    private static LoadingScreen _instance; //Variable para la instancia del singleton
    [SerializeField] CanvasFade canvasFade;     //Referencia al script del fade
    public static LoadingScreen instance    //Singleton
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LoadingScreen>();
                if (_instance == null)
                {
                    var go = Resources.Load("UI/LoadingScreen") as GameObject;
                    go = Instantiate(go, Vector3.zero, Quaternion.identity);
                    _instance = go.GetComponent<LoadingScreen>();

                }
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    //Recupero el canvas asignandole el sorting layer UI para que se vea delante del juego
    private void Awake()
    {
        GetComponent<Canvas>().sortingLayerName = "UI";
    }

    //Metodo para que aparezca el fade
    public void OnLoadScreen()
    {
        canvasFade.FadeIn();
    }
    //Corutina de OnLoad screen
    public IEnumerator _OnLoadScreen()
    {
        yield return canvasFade._FadeIn();
    }

    //Este metodo para que desaparezca el fade
    public void OnLoadedScreen()
    {
        canvasFade.FadeOut();

    }
    //Corutina de OnloadedScreen
    public IEnumerator _OnLoadedScreen()
    {
        yield return canvasFade._FadeOut();
        Destroy(this.gameObject);
    }

}
