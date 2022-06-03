using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class HeroModel
{
    //Metodo para guardar los datos del heroData en el gameName(Registro), se llamara a la hora de guardar los datos desde el controlador de GameManager, utilizando su metodo saveGame.
    public void Save(string gameName, HeroData heroData)
    {
        var binaryFormater = new BinaryFormatter();
        Debug.Log(Application.persistentDataPath + "/HeroData_" + gameName + ".data");
        var file = File.Create(Application.persistentDataPath + "/HeroData_" + gameName + ".data");     //Creo un File en la ruta de datos persistentes por defecto de unity dandole formato de nombre
        binaryFormater.Serialize(file, heroData);   //Utilizo la clase BinaryFormatter que he creado para serializar los datos del entity que hemos recibido como parametro en el file
        file.Close();   //Cierra el flujo del fichero
    }


    //Metodo para cargar los datos del heroData guardados previamente
    public HeroData Load(string gameName)
    {
        var heroData = new HeroData();      //Creo un hero data que seteare con los datos guardados en el fichero del path
        var binaryFormater = new BinaryFormatter();

        var path = Application.persistentDataPath + "/HeroData_" + gameName + ".data";  //La ruta del registro
        Debug.Log(path);

        if (File.Exists(path))  //Si el fichero existe continua
        {
            var file = File.OpenRead(path); //Abro el flujo de datos del fichero para poder leer el contenido
            heroData = binaryFormater.Deserialize(file) as HeroData;    //Deserializo los datos de la misma forma que lo serialize ( tipo HeroData )
            file.Close();   //Cierro el flujo
        }
        else
        {
            heroData = null;
        }
        return heroData;    //Retorna los datos
    }
}
