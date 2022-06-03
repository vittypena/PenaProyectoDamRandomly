using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LevelData
{
    public SceneId sceneId; //Guardo el Id de la escena
    public List<string> defeatBosses = new List<string>();  //Guardo la lista con los bosses derrotados.
}