using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]  //Hay que hacer la clase serializable para poder usar el formato JSON o RawData.
public class HeroData
{
    public PowerUpId currentPowerUpId; //Guardo el tipo de magia actual.
    public int powerUpAmount;   //Guardo la cantidad de magia actual.
    public int coins;   //Guardo las monedas.
    public int heart; // Guardo los corazones
}
