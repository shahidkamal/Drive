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

    [Tooltip("Power")] [SerializeField]
    protected float _initialPower;
#pragma warning restore 649 // 'field' is never assigned to
    protected float _power;
    
    public float MaxSpeed => _maxSpeed;
    
    protected PolygonCollider2D _collider;
    protected Rigidbody2D _rigidbody2D;
    protected Vector2 _velocity;
    protected Vector2 _acceleration;
    
    protected bool CentredSteering { get; private set; }

    protected Vector2 _targetPos;
    protected Vector2 _initialPos;
    protected int _layerMask;

    protected virtual void Awake()
    {
        // Cache rigidbody and collider for later frequent access
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _collider = gameObject.GetComponent<PolygonCollider2D>(); 
        _collider.autoTiling = true;    // Testing to see if this does what we were doing clumsily
        CentredSteering = true;
        _layerMask = 1 << LayerMask.NameToLayer("Vehicle");
        _power = _initialPower;
    }

    protected virtual void Update()
    {
    }
    
    protected virtual void FixedUpdate()
    {
        _initialPos = _rigidbody2D.position;
        _targetPos = _initialPos + _velocity * Time.fixedDeltaTime;
    }

    public virtual void ResetPosition()
    {
    }

    public void SetPosition(Vector2 position)
    {
        _rigidbody2D.angularVelocity = 0;
        _rigidbody2D.SetRotation(Quaternion.identity);        
        _rigidbody2D.velocity = Vector2.zero;
        if (_rigidbody2D.isKinematic)
        {
            _rigidbody2D.MovePosition(position);
        }
        else
        {
            _rigidbody2D.position = position;
        }
    }

    public void SetPower(float power)
    {
        _power = power;
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