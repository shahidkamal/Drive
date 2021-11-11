using UnityEngine;

public class PlayerVehicle : Vehicle
{
    protected override void Update()
    {
        if (Input.GetKeyDown(Game.Instance.Settings.KeyLeft))
        {
            _xVelocity = -_steeringResponse;
            CentreSteering(false);
        }
                
        if (Input.GetKeyDown(Game.Instance.Settings.KeyRight))
        {
            _xVelocity = _steeringResponse;
            CentreSteering(false);
        }

        if (Input.GetKeyUp(Game.Instance.Settings.KeyLeft) || Input.GetKeyUp(Game.Instance.Settings.KeyRight))
        {
            CentreSteering();
        }

        if (CentredSteering)
        {
            _xVelocity *= 0.95f;
        }
        _maxSpeed = Game.Instance.PlayerSpeed;
    }
    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        // Now add force in the vector we intend to move towards
        var targetVector = _targetPos - _initialPos;
        _rigidbody2D.AddForce(targetVector * _power);
        _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, _maxSpeed);
    }
}