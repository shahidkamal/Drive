using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public bool underAIControl;
    public float steerSpeed;
    public float speed;
    public Sprite carSprite;

    private SpriteRenderer _spriteRenderer;
    private PolygonCollider2D _collider;
    private Rigidbody2D _rigidbody2D;
    private float _xVelocity;
    private bool _centredSteering;
    
    private void Awake()
    {
        _collider = gameObject.GetComponent<PolygonCollider2D>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
        
        if (carSprite != null)
        {
            SetSprite(carSprite);
        }

        _xVelocity = 0;
        _centredSteering = true;
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

    private void ResetCollider()
    {
        if (_collider != null)
        {
            DestroyImmediate(_collider);
        }
        _collider = gameObject.AddComponent<PolygonCollider2D>();
        if (underAIControl)
        {
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody2D.gravityScale = 0;
        }
        else
        {
            _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }
    }
    
    private void Update()
    {
        if (underAIControl)
        {
            
        }
        else
        {
            if (Input.GetKeyDown(Game.Instance.Settings.KeyLeft))
            {
                _xVelocity = -steerSpeed;
                _centredSteering = false;
            }
            
            if (Input.GetKeyDown(Game.Instance.Settings.KeyRight))
            {
                _xVelocity = steerSpeed;
                _centredSteering = false;
            }

            if (Input.GetKeyUp(Game.Instance.Settings.KeyLeft) || Input.GetKeyUp(Game.Instance.Settings.KeyRight))
            {
                _centredSteering = true;
            }

            if (_centredSteering)
            {
                _xVelocity *= 0.95f;
            }
        }
    }

    private void FixedUpdate()
    {
        var pos = _rigidbody2D.position;
        var velocity = Vector2.zero;
        velocity.x = _xVelocity;
        velocity.y = speed;
        var targetPos = pos + velocity * Time.fixedDeltaTime;
        if (!underAIControl)
        {
            if (targetPos.x >= Game.Instance.Settings.PlayerRightScreenBound ||
                targetPos.x <= Game.Instance.Settings.PlayerLeftScreenBound)
            {
                targetPos = pos;
            }
            _rigidbody2D.MovePosition(targetPos);
        }
        else
        {
            if (pos.y < -5.5f)
            {
                ResetPosition();
                ObjectPool.Pool.Release(gameObject);
            }
            else
            {
                _rigidbody2D.MovePosition(targetPos);
            }
        }
    }

    public void InitAI(float newSpeed, Sprite newSprite = null)
    {
        speed = newSpeed;
        underAIControl = true;

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
}
