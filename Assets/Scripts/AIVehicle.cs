using UnityEngine;

public class AIVehicle : Vehicle
{
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
        var lane = Random.Range(0, Game.NumLanes);
        var xPos = Game.LaneWidth / 2;    // Centre in lane
        xPos -= (Game.LaneWidth * Game.NumLanes / 2);
        xPos += lane * Game.LaneWidth;
        SetPosition(new Vector2(xPos, Game.Instance.ScreenHeightUnits / 2));
    }
}