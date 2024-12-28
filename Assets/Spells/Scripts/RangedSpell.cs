using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ranged Spell", menuName = "Spells/Ranged Spell")]
public class RangedSpell : Spell
{
    private const float minDistanceWall = 1.2f;

    public Projectile projectile;
    public bool checkForWall = false;

    public override void Cast(BasePlayer _player, Vector2 _castPoint)
    {
        if (checkForWall)
        {
            Body _body = _player.GetComponent<Body>();
            float _distance = minDistanceWall;

            _body.CheckDistance(_player.Direction, ref _distance);

            if (_distance < minDistanceWall) return;
        }

         Projectile _projectile = Instantiate(projectile, _castPoint, Quaternion.identity);
        _projectile.direction = _player.Direction;
    }

    public override bool CanCast(BasePlayer _player)
    {
        bool _canCast = true;

        if (checkForWall)
        {
            Body _body = _player.GetComponent<Body>();
            float _distance = minDistanceWall;

            _body.CheckDistance(_player.Direction, ref _distance);

            if (_distance < minDistanceWall) _canCast = false;
        }
        else
        {
            _canCast = true;
        }

        return _canCast;
    }
}
