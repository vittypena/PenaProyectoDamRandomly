using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<Sprite> powerUpSprites;
    private static GameManager _instance;

    private bool gameOver = false;

    private PowerUpId currentPowerUpId;
    private int powerUpAmount = 0;
    private int coin = 0;
    Sprite powerUpSprite;
    private int hearts = 0;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    var go = Resources.Load("Game/GameManager") as GameObject;
                    Instantiate(go, Vector3.zero, Quaternion.identity);
                    go.AddComponent<GameManager>();
                    _instance = go.GetComponent<GameManager>();
                }
                DontDestroyOnLoad(_instance.gameObject);
            }            
            return _instance;
        }
    }


    public void HideGameOver()
    {
        gameOver = false;
    }

    //Metodo para distriubuir los datos entre los distintos elementos
    public void UpdateHealth(int health)
    {
        hearts = health;
        HUDScreen.instance.UpdateHealth(health);
        if (health <= 0 && !gameOver)       //Si se muere se instancia GameOverScreen
        {
            gameOver = true;
            GameOverScreen.instance.ShowScreen();
        }
    }

    public void UpdateCoins(int coins)
    {
        coin = coins;
        HUDScreen.instance.UpdateCoins(coins);
    }

    public void UpdatePowerUp(int amount)
    {
        powerUpAmount = amount;
        HUDScreen.instance.UpdatePowerUp(amount);
    }

    public void UpdatePowerUp(PowerUpId powerUpId, int amount)
    {
        powerUpAmount = amount;
        currentPowerUpId = powerUpId;
        if ((int)powerUpId >= 0)
        {
            HUDScreen.instance.UpdatePowerUp(powerUpSprites[(int)powerUpId], amount);
        }
    }

    public void UpdatePowerUp(PowerUpId powerUpId)
    {

        currentPowerUpId = powerUpId;
        if ((int)powerUpId >= 0)
        {
            HUDScreen.instance.UpdatePowerUp(powerUpSprites[(int)powerUpId]);
        }
    }

    //Metodo para guardar la partida
    public void SaveGame()
    {
        SaveSystem.instance.Load(out GameData gameData);    //Instancia los datos ya guardados
        if (gameData == null)   //Si es null crea el gameData de cero
        {
            gameData = new GameData();
        }
        //Obtengo los datos del hero gracias a su get, estos datos los mandare en el gameData para guardarlos
        gameData.heroData.currentPowerUpId = currentPowerUpId; 
        gameData.heroData.powerUpAmount = powerUpAmount;
        gameData.heroData.coins = coin;
        gameData.heroData.heart = hearts;

        gameData.levelData.sceneId = SceneHelper.instance.GetCurrentSceneId();  //Nos da la escena actual el SceneHelper

        SaveSystem.instance.Save(gameData); //Guarda los datos
    }
}
