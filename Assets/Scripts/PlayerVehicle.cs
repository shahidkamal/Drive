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
            
        if (_targetPos.x >= Game.Instance.Settings.PlayerRightScreenBound ||
            _targetPos.x <= Game.Instance.Settings.PlayerLeftScreenBound)
        {
            //_targetPos = _initialPos;
        }
        // Now add force in the vector we intend to go
        _rigidbody2D.AddForce((_targetPos - _initialPos) * 125);
//        _rigidbody2D.MovePosition(_targetPos);
    }
}