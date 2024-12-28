using UnityEngine;

public class FireBall : Projectile
{
	#region Protected Methods

	protected override void OnCollision(Collider2D _collider)
	{
		IceBlock _iceBlock = _collider.GetComponent<IceBlock>();

		if (_iceBlock)
		{
			_iceBlock.DestroyBlock();
		}
		else
		{
			ShakeEffect _camera = Camera.main.GetComponent<ShakeEffect>();
			_camera.Shake(0.05f, 0.2f);

			DestroyParticle();
		}
	}

	#endregion
}
