using UnityEngine;

public abstract class Spell : ScriptableObject
{
    public string spellName;
    public float castTime = 0.2f;

    public abstract void Cast(PlayerBase _player, Vector2 _castPoint);

    public abstract bool CanCast(PlayerBase _player);
}
