using UnityEngine;

public class PlayerEventController : BaseEventController
{
    public System.Action<SpellItem> OnCastSpell;
    public System.Action<IPickable> OnItemGet;
    public System.Action OnDeath;

    private Player player;

    public PlayerEventController(Player player) : base()
    {
        this.player = player;
    }

    public void HandleItemCollision(Body body, LayerMask pickableLayerMask)
    {
        foreach (var item in body.CheckCollidersOfType<IPickable>(pickableLayerMask))
        {
            item.OnPickup(player.Inventory);
            player.Inventory.Actions.OnDataChange?.Invoke(player.Inventory.Spells);
        }
    }

}
