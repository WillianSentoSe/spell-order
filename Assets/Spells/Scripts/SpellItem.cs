﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellItem : MonoBehaviour, IPickable
{
    public Spell spell;

    public void OnPickup()
    {
        PlayerBase.main.GetComponent<SpellCast>().AddSpell(spell);
        Destroy(gameObject);
    }
}
