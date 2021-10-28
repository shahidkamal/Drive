using UnityEngine;

public class AIVehicle : Vehicle
{
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

    public override void ResetPosition()
    {
        base.ResetPosition();
        SetPosition(new Vector2(Random.Range(-2.5f, 2.5f), 6));
    }
}