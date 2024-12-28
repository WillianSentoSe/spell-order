using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public float height = 2f;

    private Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Body _body = collision.gameObject.GetComponent<Body>();

        if (_body)
        {
            float _strenght = Body.HeightToJumpForce(height);
            _body.Velocity = new Vector2(0f, _strenght);
            animator.StopPlayback();
            animator.Play("Activated");
        }
    }
}
