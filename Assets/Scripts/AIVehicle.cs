using UnityEngine;

public class AIVehicle : Vehicle
{
    private static int _previousLane = -1;
    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
            
        if (_initialPos.y < -Game.Instance.ScreenHeightUnits * 0.5f - _collider.bounds.extents.y)
        {
            ResetPosition();
            ObjectPool.Pool.Release(gameObject);
        }
        else
        {
            _rigidbody2D.AddForce((_targetPos - _initialPos) * _power);
            _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, _maxSpeed);
        }
    }

    public override void ResetPosition()
    {
        base.ResetPosition();
        int lane;
        do {
            lane = Random.Range(0, Game.NumLanes);
        } while (lane == _previousLane);
        _previousLane = lane;
        
        var xPos = Game.LaneWidth / 2;    // Centre in lane
        xPos -= (Game.LaneWidth * Game.NumLanes / 2);
        xPos += lane * Game.LaneWidth;
        var yPos = Game.Instance.ScreenHeightUnits * 0.5f + 1 + Game.Instance.TrackingCamera.transform.position.y;
        // If this car is going faster than the player, spawn below
        if (_maxSpeed > Game.Instance.PlayerSpeed)
        {
            yPos = Game.Instance.TrackingCamera.transform.position.y - Game.Instance.ScreenHeightUnits * 0.5f - 1;
        }
        SetPosition(new Vector2(xPos, yPos));
    }
}