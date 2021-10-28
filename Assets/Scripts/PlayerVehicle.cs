using System;
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
    }

    protected override void ResetCollider()
    {
        base.ResetCollider();
            
        _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
            
        if (_targetPos.x >= Game.Instance.Settings.PlayerRightScreenBound ||
            _targetPos.x <= Game.Instance.Settings.PlayerLeftScreenBound)
        {
            _targetPos = _initialPos;
        }
        _rigidbody2D.MovePosition(_targetPos);
    }
}