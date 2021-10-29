using UnityEngine;

public class AIVehicle : Vehicle
{
    private static int _previousLane = -1;
    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
            
        if (_initialPos.y < -Game.Instance.ScreenHeightUnits * 0.5f)
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
        int lane;
        do {
            lane = Random.Range(0, Game.NumLanes);
        } while (lane == _previousLane);

        _previousLane = lane;
        var xPos = Game.LaneWidth / 2;    // Centre in lane
        xPos -= (Game.LaneWidth * Game.NumLanes / 2);
        xPos += lane * Game.LaneWidth;
        SetPosition(new Vector2(xPos, Game.Instance.ScreenHeightUnits / 2 + 1));
    }
}