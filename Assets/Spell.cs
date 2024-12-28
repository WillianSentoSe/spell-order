using UnityEngine;

public abstract class Spell : BaseItem
{
    public Sprite rodSprite;
    public string spellName;
    public float castTime = 0.2f;

    public abstract void Cast(BasePlayer _player, Vector2 _castPoint);

    public abstract bool CanCast(BasePlayer _player);
}
