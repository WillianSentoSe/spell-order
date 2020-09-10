using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeEffect : MonoBehaviour
{
    #region Properties

    public Transform target;

    protected float strenght;
    protected float time;
    protected Vector2 startPosition;
    protected bool isShaking;

    #endregion

    #region Getters and Setters

    public bool IsShaking { get { return isShaking; } }

    #endregion

    #region Execution

    public void Start()
    {
        if (!target) target = transform;
    }

    public void Update()
    {
        if (isShaking)
        {
            float _x = Random.Range(-strenght, strenght);
            float _y = Random.Range(-strenght, strenght);

            target.position = startPosition + new Vector2(_x, _y);
        }
    }

    #endregion

    #region Public Methods

    public void Shake(float _strenght, float _duration)
    {
        strenght = _strenght;
        time = _duration;

        isShaking = true;
    }

    public void Stop()
    {
        isShaking = false;
    }

    #endregion
}
