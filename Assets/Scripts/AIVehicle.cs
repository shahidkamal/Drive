using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIVehicle : Vehicle
{
    private static int _previousLane = -1;
    private static readonly List<Transform> Neighbours  = new List<Transform>(256);
    private static readonly Collider2D[] Results = new Collider2D[256];
    private const float AvoidanceRadius = 3f;

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
        if (Neighbours.Count == 0)
        {
            return;
        }
        if ( _power == 0)
        {
            return;    // early out if no candidates found or if already a ragdoll
        }
        
        var offset = Vector2.zero;
        var count = 0;

        var pos = transform.position;
        foreach (var t in Neighbours)
        {
            var neighbourPos = t.position;
            if (Vector2.SqrMagnitude(neighbourPos - pos) < AvoidanceRadius * AvoidanceRadius)
            {
                ++count;
                Vector2 d = (pos - neighbourPos).normalized;
                offset += d / (1 + Vector2.Distance(pos, neighbourPos));
            }
        }

        if (count > 0)
        {
            offset /= count;
        }

        offset = offset * _maxSpeed - _rigidbody2D.velocity;
        _acceleration.x += offset.x * 25;    // bias towards steering away
        _acceleration.y += offset.y;
    }

    protected override void FixedUpdate()
    {
        _initialPos = _rigidbody2D.position;
        _acceleration.x = 0;
        _acceleration.y = _power;
        
        Avoidance();

        var y = Game.Instance.TrackingCamera.transform.position.y;
        if (_initialPos.y < -Game.Instance.ScreenHeightUnits + y || _initialPos.y > Game.Instance.ScreenHeightUnits + y)
        {
            ResetPosition();
            ObjectPool.Pool.Release(gameObject);
        }
        else
        {
            _rigidbody2D.AddForce(_acceleration);
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
        if (other.gameObject.layer == this.gameObject.layer)
        {
            _power = 0;
        }
        else
        {
            _power *= 0.9f;
        }
    }
    
}