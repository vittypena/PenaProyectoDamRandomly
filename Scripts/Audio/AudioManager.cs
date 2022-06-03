using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private float _musicVolume;
    [Range(0, 1)]       //Pone un rango al [SerializeField] para que sea visible en el ide
    [SerializeField] float musicVolume;     //Variable para el volumen de la musica

    private float _sfxVolume;
    [Range(0, 1)]
    [SerializeField] float sfxVolume;       //Variable para el volumen de los efectos de sonido    
    

    private static AudioSource musicAudioSource;        //Variable para la musica del juego
    private static AudioSource sfxAudioSource;          //Variable para los efectos de sonido del juego

    //Implementa el patron de singleton, el cual evita que haya más de una instancia de una clase, definiendo una variable estatica
    private static AudioManager _instance;

    public static AudioManager instance
    {        
        get
        {
            if (_instance == null) //Si no hay instancia define el gameObject
            {
                _instance = FindObjectOfType<AudioManager>();   //Antes de instanciar, compruebo si existe en la escena ya una instancia del objeto

                GameObject gameO;
                if( _instance == null) 
                { 
                gameO = new GameObject("AudioManager"); //Instancia el AudioManager
                gameO.AddComponent<AudioManager>(); //Añade el componente AudioManager
                _instance = gameO.GetComponent<AudioManager>(); //Obtiene el componente                
                }

                if (_instance != null)  //Si ya estaba creado, que instancie la musica solo
                {
                    var gameMusic = new GameObject("Music"); //Instancia la musica
                    gameMusic.AddComponent<AudioSource>();
                    musicAudioSource = gameMusic.GetComponent<AudioSource>();
                    gameMusic.transform.parent = _instance.gameObject.transform;    //Los hago hijos de game0 para que salgan como sus hijos en el ide al crearse

                    var gameSfx = new GameObject("Sfx"); //Instancia los efectos de sonido
                    gameSfx.AddComponent<AudioSource>();
                    sfxAudioSource = gameSfx.GetComponent<AudioSource>();
                    gameSfx.transform.parent = _instance.gameObject.transform;    //Los hago hijos de game0 para que salgan como sus hijos en el ide al crearse

                    DontDestroyOnLoad(_instance.gameObject);    //Para compartir este gameObject entre escenas
                }
            }
            return _instance;
        }
    }

    //Metodo que recibe un parametro de tipo audioClip para los efectos de sonido y lo ejecuta
    public void PlaySfx(AudioClip audioClip)
    {
        sfxAudioSource.PlayOneShot(audioClip);  //Reproduce con playOneShot una sola vez
    }

    //Metodo que recibe un parametro de tipo audioClip para la musica y lo ejecuta en un loop
    public void PlayMusic(AudioClip audioClip)
    {
        if(audioClip == null)
        {
            musicAudioSource.Stop();
        }
        if(musicAudioSource != audioClip)  //Para evitar que se reproduzca varias veces el mismo clip
        { 
        musicAudioSource.clip= audioClip;       //Le introducimos el clip de musica
        musicAudioSource.loop = true;           //Le indicamos que va a estar en loop ( que se repita )
        musicAudioSource.Play();                //Lo reproducimos
        }
    }

    //Metodo por si el usuario modifica el volumen que lo modifique
    private void Update()
    {
        if(musicVolume != _musicVolume) //Si el volumen es distinto que lo cambie
        {
            _musicVolume = musicVolume;
            if (musicAudioSource != null)
                musicAudioSource.volume = musicVolume;
        }

        if (sfxVolume != _sfxVolume) //Si el volumen de los efectos es distinto que lo cambie
        {
            _sfxVolume = sfxVolume;
            if (sfxAudioSource != null)
                sfxAudioSource.volume = sfxVolume;
        }
    }
}

