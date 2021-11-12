using UnityEngine;

public class PlayerVehicle : Vehicle
{
    protected override void Update()
    {
        if (Input.GetKeyDown(Game.Instance.Settings.KeyLeft))
        {
            _acceleration.x = -_steeringResponse;
            CentreSteering(false);
        }
                
        if (Input.GetKeyDown(Game.Instance.Settings.KeyRight))
        {
            _acceleration.x = _steeringResponse;
            CentreSteering(false);
        }

        if (Input.GetKeyUp(Game.Instance.Settings.KeyLeft) || Input.GetKeyUp(Game.Instance.Settings.KeyRight))
        {
            CentreSteering();
        }

        if (CentredSteering)
        {
            _acceleration.x = 0;
        }
        _maxSpeed = Game.Instance.PlayerSpeed;
        _acceleration.y = _power;
    }
    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        // Now add force in the vector we intend to move towards
        _rigidbody2D.AddForce(_acceleration);
        _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, _maxSpeed);
    }
}