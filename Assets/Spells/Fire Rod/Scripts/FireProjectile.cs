using UnityEngine;

public class FireProjectile : Projectile
{
	#region Protected Methods

	protected override void OnCollision(Collider2D _collider)
	{
		IceBlock _iceBlock = _collider.GetComponent<IceBlock>();

		ShakeEffect _camera = Camera.main.GetComponent<ShakeEffect>();
		_camera.Shake(0.05f, 0.2f);

		if (_iceBlock)
		{
			Destroy(_iceBlock.gameObject);
		}

		DestroyParticle();
	}

	#endregion
}
