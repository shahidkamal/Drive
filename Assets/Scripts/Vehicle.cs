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

    [Tooltip("Sprite")]  [SerializeField]
    protected Sprite _sprite;
#pragma warning restore 649 // 'field' is never assigned to
    
    public float MaxSpeed => _maxSpeed;
    
    private SpriteRenderer _spriteRenderer;
    private PolygonCollider2D _collider;
    protected Rigidbody2D _rigidbody2D;
    protected float _xVelocity;
    protected bool CentredSteering { get; private set; }

    protected Vector2 _targetPos;
    protected Vector2 _initialPos;

    private void Awake()
    {
        _collider = gameObject.GetComponent<PolygonCollider2D>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
        _xVelocity = 0;
        CentredSteering = true;
    }
    
    public void SetSprite(Sprite sprite)
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        _spriteRenderer.sprite = sprite;
        ResetCollider();
    }
    
    protected virtual void ResetCollider()
    {
        if (_collider != null)
        {
            DestroyImmediate(_collider);
        }
        _collider = gameObject.AddComponent<PolygonCollider2D>();
    }
    protected virtual void Update()
    {
    }



    protected virtual void FixedUpdate()
    {
        _initialPos = _rigidbody2D.position;
        var velocity = Vector2.zero;
        velocity.x = _xVelocity;
        velocity.y = _maxSpeed;
        _targetPos = _initialPos + velocity * Time.fixedDeltaTime;

    }

    public virtual void Init(float newSpeed, Sprite newSprite = null)
    {
        _maxSpeed = newSpeed;

        if (newSprite != null)
        {
            SetSprite(newSprite);
        }
    }
        
    public void ResetPosition()
    {
        SetPosition(new Vector2(Random.Range(-2.5f, 2.5f), 6));
        _rigidbody2D.angularDrag = 0.75f;
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

    public void CentreSteering(bool centred = true)
    {
        CentredSteering = centred;
    }
}