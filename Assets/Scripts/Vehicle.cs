using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Vehicle : MonoBehaviour
{
#pragma warning disable 649 // 'field' is never assigned to
    [Tooltip("Steering Responsiveness")]  [SerializeField]
    protected float  _steeringResponse;

    [Tooltip("Maximum Speed")]  [SerializeField]
    protected float  _maxSpeed;
#pragma warning restore 649 // 'field' is never assigned to
    
    public float MaxSpeed => _maxSpeed;
    
    protected PolygonCollider2D _collider;
    protected Rigidbody2D _rigidbody2D;
    protected float _xVelocity;
    protected bool CentredSteering { get; private set; }

    protected Vector2 _targetPos;
    protected Vector2 _initialPos;

    protected virtual void Awake()
    {
        // Cache rigidbody and collider for later frequent access
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _collider = gameObject.GetComponent<PolygonCollider2D>(); 
        _collider.autoTiling = true;    // Testing to see if this does what we were doing clumsily
        CentredSteering = true;
    }

    protected virtual void Update()
    {
    }
    
    protected virtual void FixedUpdate()
    {
        _initialPos = _rigidbody2D.position;
        var velocity = Vector2.zero;
        velocity.x = _xVelocity;
        velocity.y = _maxSpeed - Game.Instance.PlayerSpeed;
        _targetPos = _initialPos + velocity * Time.fixedDeltaTime;
    }

    public virtual void ResetPosition()
    {
        //_rigidbody2D.angularDrag = 0.75f;
    }

    public void WakePhysics(bool wake = true)
    {
        if (wake)
        {
            _rigidbody2D.WakeUp();
        }
        else
        {
            _rigidbody2D.Sleep();
        }
    }
    
    public void SetPosition(Vector2 position)
    {
        _rigidbody2D.angularVelocity = 0;
        _rigidbody2D.SetRotation(Quaternion.identity);        
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.MovePosition(position);
    }

    public void SetMaxSpeed(float speed)
    {
        _maxSpeed = speed;
    }

    public void SetSteeringSpeed(float speed)
    {
        _steeringResponse = speed;
    }
    
    protected void CentreSteering(bool centred = true)
    {
        CentredSteering = centred;
    }

    public void EnableCollder(bool enable = true)
    {
        _collider.enabled = enable;
    }
}