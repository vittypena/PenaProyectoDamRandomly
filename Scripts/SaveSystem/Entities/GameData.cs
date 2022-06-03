using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameData
{
    public string gameName = "";    //Nombre del registro
    public LevelData levelData = new LevelData();   //Guardo los datos de la escena
    public HeroData heroData = new HeroData();  //Guardo los datos del heroe
}