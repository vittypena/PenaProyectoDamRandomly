using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
public class GameModel : MonoBehaviour
{
    //Recibira un GameModel generico con un heroModel y levelModel en su interior y los guardara 
    public void Save(GameData gameData)
    {
        var heroModel = new HeroModel();
        var levelModel = new LevelModel();
        heroModel.Save(gameData.gameName, gameData.heroData);
        levelModel.Save(gameData.gameName, gameData.levelData);
    }

    //Aplica la misma logica pero con la carga
    public GameData Load(GameData gameData)
    {
        var heroModel = new HeroModel();
        var levelModel = new LevelModel();
        gameData.heroData = heroModel.Load(gameData.gameName);
        gameData.levelData = levelModel.Load(gameData.gameName);

        return gameData;
    }
}
