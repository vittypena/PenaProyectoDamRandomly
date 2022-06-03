using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    private static GameOverScreen _instance;    //Variable para el singleton
    [SerializeField] CanvasFade canvasFade;     //Referencia al fade
    public static GameOverScreen instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameOverScreen>();
                if (_instance == null)
                {
                    var go = Resources.Load("UI/GameOverScreen") as GameObject;
                    go = Instantiate(go, Vector3.zero, Quaternion.identity);
                    _instance = go.GetComponent<GameOverScreen>();

                }
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    // public static GameOverScreen instance;

    // private void Awake(){
    //     if(instance == null){
    //         DontDestroyOnLoad(gameObject);
    //         instance=this;
    //     }else{
    //         Destroy(gameObject);
    //     }
    // }

    private void Awake()
    {
        canvasFade.FadeIn();
    }

    //Metodo para mostrar la pantalla
    public void ShowScreen()
    {
        canvasFade.FadeIn();
    }

    //Metodo para el boton de continuar
    public void Continue()
    {
        SceneHelper.instance.ReloadScene();
        GameManager.instance.HideGameOver();
        Destroy(this.gameObject);
    }

    //Metodo para el boton de no continuar
    public void NoContinue()
    {
        AudioManager.instance.PlayMusic(null);
        GameManager.instance.HideGameOver();
        SceneHelper.instance.LoadScene(SceneId.TitleScreen);
        Destroy(this.gameObject);
    }

}
