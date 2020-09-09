using UnityEngine;

public abstract class Spell : ScriptableObject
{
    public string spellName;
    public float castTime = 0.2f;

    public abstract void Cast(Player _player, Vector2 _castPoint);
}
