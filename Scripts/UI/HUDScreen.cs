using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDScreen : MonoBehaviour
{
    [SerializeField] List<Image> heartImages;   //Lista de los corazones
    [SerializeField] List<Sprite> powerUpSprites;   //Lista de sprite de powerups
    [SerializeField] TextMeshProUGUI coinsText;     //Referencia al texto de las monedas
    [SerializeField] Image powerUpIconImage;    //Referencia a la magia
    [SerializeField] TextMeshProUGUI powerUpAmountText; //Referencia al texto de la magia

    // private static HUDScreen _instance; //Patron singleton

    //Variable para el patron singleton, la instancia
    // public static HUDScreen instance    
    // {
    //     get
    //     {
    //         if (_instance == null)
    //         {
    //             _instance = FindObjectOfType<HUDScreen>();  //Busca en la escena
    //             if (_instance == null)  //Si sigue siendo nula lo carga desde los recursos.
    //             {
    //                 var go = Resources.Load("UI/HUD") as GameObject;
    //                 go = Instantiate(go, Vector3.zero, Quaternion.identity);
    //                 _instance = go.GetComponent<HUDScreen>();
    //             }
    //             DontDestroyOnLoad(_instance.gameObject);    //Que no destruya la instancia entre estancias
    //         }
    //         return _instance;
    //     }
    // }

    public static HUDScreen instance;

    private void Awake(){
        if(instance == null){
            DontDestroyOnLoad(gameObject);
            instance=this;
        }else{
            Destroy(gameObject);
        }
    }

    public void UpdateHealth(int health)
    {
        for (int i = 0; i < heartImages.Count; i++) //Recorre la lista de los corazones (las imagenes)
        {
            if ((i + 1) <= health)  //Si la cumple tiene color si no desaparece la imagen
            {
                heartImages[i].color = new Color(1, 1, 1, 1);
            }
            else
            {
                heartImages[i].color = new Color(1, 1, 1, 0);
            }
        }
    }

    //Metodo para upgradear las monedas
    public void UpdateCoins(int coins)
    {
        coinsText.text = "x " + coins.ToString();
    }

    //Metodo para upgradear las magias
    public void UpdatePowerUp(int amount)
    {

        if (amount <= 0)
        {  //Si es menor que 0 no lo pone
            powerUpIconImage.color = new Color(1, 1, 1, 0);
        }
        powerUpAmountText.text = "x " + amount;
    }

    //Metodo para upgradear las magias con dos parametros
    public void UpdatePowerUp(Sprite icon, int amount)
    {           
        powerUpIconImage.color = new Color(1, 1, 1, 1); //Si hay magias actualiza el color para que se vea

        powerUpIconImage.sprite = icon;  //Actualiza el icono que se le pase
        powerUpAmountText.text = "x " + amount; //Actualiza el texto
    }

    //Metodo para upgradear las magias con solo el parametro del sprite 
    public void UpdatePowerUp(Sprite icon)
    {
        powerUpIconImage.color = new Color(1, 1, 1, 1);

        powerUpIconImage.sprite = icon; //Actualiza el icono que se le pase
    }
}
