using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenController : MonoBehaviour
{
    public bool showOptions = false;        //Variable para mostrar las opciones

    public List<GameObject> optionButtons;      //Lista con los botones para pasar por parametro
    public GameObject pressEnterButton;     //Variable para el boton enter
    public GameObject continueButton;   //Variable para el boton continue


    [SerializeField] AudioClip enterSfx;
    [SerializeField] AudioClip buttonSfx;

    GameData gameData;  //Referencia a gameData para recuperar los datos
    private void Awake()//Carga la escena guardada
    {
        SaveSystem.instance.Load(out gameData);
    }

    private void Update()
    {
        if (!showOptions && Input.GetKeyDown(KeyCode.Return))
        {     //Si se preta la tecla y no se ha mostrado las opciones
            showOptions = true;
            pressEnterButton.SetActive(false);  //Se deshabilita el boton
            foreach (var button in optionButtons)
            {
                button.SetActive(true);
            }
            AudioManager.instance.PlaySfx(enterSfx);

            if (gameData != null)   //Desactiva el boton de continuo si hay datos guardados
            {
                continueButton.SetActive(true);
            }
            else
            {
                continueButton.SetActive(false);
                optionButtons[1].SetActive(false);
            }
        }
    }


    //Metodo para continuar el juego
    public void ContinueGame()
    {
        AudioManager.instance.PlaySfx(buttonSfx);
        SceneHelper.instance.LoadScene(gameData.levelData.sceneId);
    }

    //Metodo para salir del juego
    public void ExitGame()
    {
        AudioManager.instance.PlaySfx(buttonSfx);   

        Application.Quit(); //Cierra la aplicacion
    }

    //Metodo para empezar una nueva partida
    public void StartNewGame()
    {
        AudioManager.instance.PlaySfx(buttonSfx);

        SceneHelper.instance.LoadScene(SceneId.Level1_1);   //Instancia la escena1 ( Level1_1 )
    }
}
