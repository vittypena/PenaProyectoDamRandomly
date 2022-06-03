using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchParentPlatform : MonoBehaviour
{
    //Cuando detecta que hay una colision, si el tag es el de player lo emparenta con ese game object.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals(TagId.Player.ToString()))
        {
            collision.gameObject.transform.parent = this.transform;
        }
    }

    //Lo desemparenta cuando deja de haber una colision
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals(TagId.Player.ToString()))
        {
            collision.gameObject.transform.parent = null;
        }
    }
}
