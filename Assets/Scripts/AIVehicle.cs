using UnityEngine;

public class AIVehicle : Vehicle
{
    protected override void ResetCollider()
    {
        base.ResetCollider();
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        _rigidbody2D.gravityScale = 0;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
            
        if (_initialPos.y < -5.5f)
        {
            ResetPosition();
            ObjectPool.Pool.Release(gameObject);
        }
        else
        {
            _rigidbody2D.MovePosition(_targetPos);
        }
    }

    public override void Init(float newSpeed, Sprite newSprite = null)
    {
        base.Init(newSpeed, newSprite);
        underAIControl = true;
    }
}