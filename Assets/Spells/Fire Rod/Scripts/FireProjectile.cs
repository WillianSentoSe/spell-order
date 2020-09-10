using UnityEngine;

public class FireProjectile : Projectile
{
	#region Protected Methods

	protected override void OnCollision(Collider2D _collider)
	{
		IceBlock _iceBlock = _collider.GetComponent<IceBlock>();

		if (_iceBlock)
		{
			Destroy(_iceBlock.gameObject);
		}

		DestroyParticle();
	}

	#endregion
}
