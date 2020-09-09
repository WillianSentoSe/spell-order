﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float velocity = 5f;
    public Vector2 direction;

    public void Start()
    {
        GetComponent<SpriteRenderer>().flipX = direction.x < 0f;
    }

    private void Update()
    {
        transform.position = (Vector2)transform.position + velocity * direction * Time.deltaTime;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}