using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeEffect : MonoBehaviour
{
    #region Properties

    public Transform target;

    protected float strenght;
    protected float duration;
    protected float time;
    protected Vector3 startPosition;
    protected bool isShaking;
    protected bool decayOverTime;

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
            if (time > 0)
            {
                time -= Time.deltaTime;

                float _delta = time / duration;
                float _maxDistance = decayOverTime ? strenght * _delta : strenght;

                float _x = Random.Range(-_maxDistance, _maxDistance);
                float _y = Random.Range(-_maxDistance, _maxDistance);

                target.position = startPosition + new Vector3(_x, _y, startPosition.z);
            }
            else
            {
                Stop();
            }
        }
    }

    #endregion

    #region Public Methods

    public void Shake(float _strenght, float _duration, bool _decayOverTime = true)
    {
        if (isShaking) Stop();

        strenght = _strenght;
        duration = _duration;
        decayOverTime = _decayOverTime;

        startPosition = target.transform.position;
        time = duration;
        isShaking = true;
    }

    public void Stop()
    {
        target.position = startPosition;

        isShaking = false;
    }

    #endregion
}
