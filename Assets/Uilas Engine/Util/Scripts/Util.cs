using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{

    #region Components

    public static void DisableChildrenParticles(Transform _transform)
    {
        ParticleSystem[] _particles = _transform.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < _particles.Length; i++)
        {
            _particles[i].Stop();
        }
    }

    #endregion

}
