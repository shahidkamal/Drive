using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIVehicle : Vehicle
{
    private static int _previousLane = -1;
    private static readonly List<Transform> Neighbours  = new List<Transform>(256);
    private static readonly Collider2D[] Results = new Collider2D[256];
    private const float AvoidanceRadius = 2f;

    private void Avoidance()
    {
        // Generate list of targets that overlap the circle collider for this vehicle
        Neighbours.Clear();
        var size = Physics2D.OverlapCircleNonAlloc(transform.position, AvoidanceRadius, Results, _layerMask);
        for (var i = 0; i < size; ++i)
        {
            var c = Results[i];
            if (c != _collider)
            {
                Neighbours.Add(c.transform);
            }
        }
        
        // Now sum a total force to reduce collisions
        if (Neighbours.Count == 0) return;    // early out if no candidates found

        var offset = Vector2.zero;
        var count = 0;

        var pos = transform.position;
        foreach (var t in Neighbours)
        {
            var neighbourPos = t.position;
            if (Vector2.SqrMagnitude(neighbourPos - pos) < AvoidanceRadius)
            {
                ++count;
                Vector2 d = (pos - neighbourPos).normalized;
                offset += d / Vector2.Distance(pos, neighbourPos);
            }
        }

        if (count > 0)
        {
            offset /= count;
        }

        offset = offset.normalized * _maxSpeed - _velocity;
        _velocity = Vector2.ClampMagnitude(offset, _power);
    }

    protected override void FixedUpdate()
    {
        _initialPos = _rigidbody2D.position;
        _velocity.x = _xVelocity;
        _velocity.y = _maxSpeed;
        Avoidance();
        _targetPos = _initialPos + _velocity * Time.fixedDeltaTime;

        var y = Game.Instance.TrackingCamera.transform.position.y;
        if (_initialPos.y < -Game.Instance.ScreenHeightUnits + y || _initialPos.y > Game.Instance.ScreenHeightUnits + y)
        {
            ResetPosition();
            ObjectPool.Pool.Release(gameObject);
        }
        else
        {
            //_rigidbody2D.AddForce((_targetPos - _initialPos) * _power);
            _rigidbody2D.AddForce(_velocity);
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
        SetPower(Game.Instance.Settings.AIInitialPower);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _power = 0;
        print("Boom");
    }
    
}