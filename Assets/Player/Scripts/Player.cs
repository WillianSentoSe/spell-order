using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : SidescrollerPlayer
{
    private SpellCast spellCast;

    public override void Start()
    {
        // Chamando Start do componente pai
        base.Start();

        // Definindo referências
        spellCast = GetComponent<SpellCast>();
        if (!spellCast) throw new UnityException("Adicione o componente SpellCast ao jogador.");
    }

    protected override void OnPrimaryItemPerformed(InputAction.CallbackContext _context)
    {
        base.OnPrimaryItemPerformed(_context);

        spellCast?.CastSpell();
    }
}
