using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ranged Spell", menuName = "Spells/Ranged Spell")]
public class RangedSpell : Spell
{
    public Projectile projectile;

    public override void Cast(Player _player, Vector2 _castPoint)
    {
        Projectile _projectile = Instantiate(projectile, _castPoint, Quaternion.identity);
        _projectile.direction = _player.Direction;
    }
}
