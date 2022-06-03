using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [Header("Debug")]
    public bool save;   //Variable para ver si guarda
    public bool load;   //Variable para ver si carga

    [SerializeField] GameData gameData; //variable para debugear el gameData al cargar datos
    [SerializeField] HeroData heroData; //variable para debugear el heroData al cargar datos
    [SerializeField] LevelData levelData;   //variable para debugear el levelData al cargar datos

    private const string gameName = "partidaDefinitiva"; //variable con el nombre de la partida

    //Intanciar modelos
    private GameModel gameModel = new GameModel();
    private LevelModel levelModel = new LevelModel();
    private HeroModel heroModel = new HeroModel();

    private static SaveSystem _instance;

    public static SaveSystem instance   //Patron singleton, como he venido haciendo para evitar instancias repetidas
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SaveSystem>();

                GameObject gameO = null;
                if (_instance == null)
                {
                    gameO = new GameObject("SaveSystem");
                    gameO.AddComponent<SaveSystem>();
                    _instance = gameO.GetComponent<SaveSystem>();

                }
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    //Metodo para guardar los datos de tipo gameData
    public void Save(GameData data)
    {
        data.gameName = gameName;
        gameData = data;
        gameModel.Save(data);
    }

    //Metodo para guardar los datos de tipo levelData
    public void Save(LevelData data)
    {
        levelData = data;

        levelModel.Save(gameName, data);
    }

    //Metodo para guardar los datos de tipo heroData
    public void Save(HeroData data)
    {
        heroData = data;

        heroModel.Save(gameName, data);
    }

    //Logica para cargar datos por parametro
    public void Load(out GameData data)
    {
        data = new GameData();
        data.gameName = gameName;
        data = gameModel.Load(data);
        if (data.levelData == null && data.heroData == null)
        {
            data = null;
        }
        gameData = data;
    }

    public void Load(out LevelData data)
    {
        data = new LevelData();
        data = levelModel.Load(gameName);
        levelData = data;
    }
    public void Load(out HeroData data)
    {
        data = new HeroData();
        data = heroModel.Load(gameName);
        heroData = data;
    }

    //Si hay cambios comprueba que variable booleana esta chequeada para cargar o guardar
    private void Update()
    {
        if (save)
        {
            save = false;
            Save(gameData);
        }
        if (load)
        {
            load = false;
            Load(out gameData);
        }
    }

}
