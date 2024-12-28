using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazzard : MonoBehaviour
{
    #region Properties

    #endregion

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == Player.Layer)
        {
            Player _player = collider.gameObject.GetComponent<Player>();
            _player?.Kill();
        }
    }
}
