using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PortalScene : MonoBehaviour
{
    [SerializeField] SceneId levelToLoad;   //Referencia al SceneId con los enum con las distintas sceneas
    [SerializeField] Transform spawnPosition;   //Posicion de spawn, para posicionar correctamente al jugador

    //Metodo para comprobar si detecta una colision contra escena para cargarla
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals(TagId.Player.ToString()))
        {
            SceneHelper.instance.LoadScene(levelToLoad);    //Lleva al jugador a otra escena
        }
    }
    public SceneId SceneToLoad()    //Getter para saber que escena puede cargar este portal
    {
        return levelToLoad;
    }

    public Vector2 GetSpawnPosition()       //Necesitamos la variable de span position para la siguiente escena
    {
        return spawnPosition.position;
    }

    //Metodo para dar visibilidad al portal en el editor
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int i = 0; i < 10; i++) {
            Gizmos.DrawWireCube(this.transform.position, new Vector3(i * 0.3f, i * 0.3f, 0));
        }
        Handles.Label((Vector2)this.transform.position + Vector2.up * 2, "LEVEL: " + levelToLoad.ToString());

        Gizmos.DrawWireSphere(spawnPosition.position, 0.3f);
    }
#endif
}
