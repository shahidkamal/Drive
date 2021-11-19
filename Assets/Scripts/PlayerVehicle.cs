using UnityEngine;

public class PlayerVehicle : Vehicle
{
#pragma warning disable 649 // 'field' is never assigned to
    [Tooltip("Player boost")] [SerializeField]
    private FloatVariable playerBoost;
#pragma warning restore 649 // 'field' is never assigned to

    private bool boosting;
    private float boostFactor;
    
    protected override void Awake()
    {
        base.Awake();
        boosting = false;
        boostFactor = 1;
    }

    private void SetBoost(bool boost = true)
    {
        if (boost && !boosting)
        {
            boosting = true;
            _power *= 1.5f;
            boostFactor = 2;
            if (_audioSource != null)
            {
                _audioSource.Play();
            }
        }
        if (!boost && boosting)
        {
            boosting = false;
            _power = _initialPower;
            boostFactor = 1;
            if (_audioSource != null)
            {
                _audioSource.Stop();
            }

        }
    }
    
    private void Boost()
    {
        if (boosting)
        {
            var total = Mathf.Clamp01(playerBoost - Time.deltaTime / 5f);
            if (total < 0.01f)
            {
                total = 0;
                SetBoost(false);
            }
            playerBoost.SetValue(total);
        }
        else
        {
            if (playerBoost > 0.9f)
            {
                if (Input.GetKeyDown(Game.Instance.Settings.KeyBoost))
                {
                    SetBoost(true);
                }
            }
        }
    }
    
    protected override void Update()
    {
        base.Update();
        Boost();
        
        if (Input.GetKeyDown(Game.Instance.Settings.KeyLeft))
        {
            _acceleration.x = -_steeringResponse;
            CentreSteering(false);
        }
                
        if (Input.GetKeyDown(Game.Instance.Settings.KeyRight))
        {
            _acceleration.x = _steeringResponse;
            CentreSteering(false);
        }

        if (Input.GetKeyUp(Game.Instance.Settings.KeyLeft) || Input.GetKeyUp(Game.Instance.Settings.KeyRight))
        {
            CentreSteering();
        }

        if (CentredSteering)
        {
            _acceleration.x = 0;
        }
        _maxSpeed = Game.Instance.PlayerSpeed * boostFactor;
        _acceleration.y = _power;
    }
    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        // Now add force in the vector we intend to move towards
        _rigidbody2D.AddForce(_acceleration);
        _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, _maxSpeed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == gameObject.layer)
        {
            var boost = 0f;
            foreach (var hitPos in other.contacts)
            {
                var normal = hitPos.normal;

                if (normal.x > 0.99f || normal.x <  -0.99f)
                {
                    boost += 0.1f;
                }

                if (normal.y < -0.99f)
                {
                    boost -= 0.1f;
                    SetBoost(false);
                }

                if (normal.y > 0.99f)
                {
                    // player death maybe?
                    boost -= 1f;
                    SetBoost(false);
                }
            }

            playerBoost.SetValue(Mathf.Clamp01(playerBoost + boost));
        }
    }
}