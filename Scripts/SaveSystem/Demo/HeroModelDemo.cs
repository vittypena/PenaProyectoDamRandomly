using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroModelDemo : MonoBehaviour
{
    [SerializeField] List<GameData> ranuras;
    [SerializeField] GameData gameData;

    GameModel gameModel;
    public bool save;
    public bool load;
    void Start()
    {
        gameModel = new GameModel();
    }

    void Update()
    {
        if (save)
        {     //Si save esta activo guarda el heroData el cual se pasara por parametro en el ide.
            save = false;
            gameModel.Save(gameData);
        }

        if (load)       //Si no cargara el gameData
        {
            gameData = gameModel.Load(gameData);    //Seteo el heroData con los datos que reciba del load
            load = false;
        }
    }
}
