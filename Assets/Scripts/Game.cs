using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using TMPro;

public class Game : MonoBehaviour
{ 
    private static Game _instance;

    public static Game Instance => _instance;
    
    [Tooltip("Settings")] 
#pragma warning disable 649 // 'field' is never assigned to
    [SerializeField]
    private Settings settings;
    
    [Tooltip("Car spawn position")] [SerializeField]
    private Transform _playerSpawn;
#pragma warning restore 649 // 'field' is never assigned to

    [Tooltip("Timer")] 
#pragma warning disable 649 // 'field' is never assigned to
    [SerializeField]
    private TextMeshProUGUI _timerText;
#pragma warning restore 649 // 'field' is never assigned to
    private float _gameTimer;
    private float _aiCarAppearTimer;
    
    public Settings Settings => settings;

    private Vehicle _playerVehicle = null;
    
    private void Awake() 
    { 
        if (_instance != null && _instance != this) 
        { 
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SpawnPlayer();

        foreach (var go in ObjectPool.Pool.Iterator())
        {
            if (!go.activeSelf)
            {
                var vehicle = go.GetComponent<Vehicle>();
                vehicle.InitAI(Random.Range(settings.AICarMinSpeed, settings.AICarMaxSpeed), settings.VehicleSprites[Random.Range(0, settings.VehicleSprites.Length)]);
                vehicle.ResetPosition();
            }
        }
        _gameTimer = 0;
        _aiCarAppearTimer = 0;
    }
    
    private void SpawnVehicle()
    {
        var go = ObjectPool.Pool.Get();
        if (go == null)
        {
            Debug.Log("Pool has run dry");
        }
        else
        {
            // Now set some values on the vehicle
            go.SetActive(true);
            var vehicle = go.GetComponent<Vehicle>();
            vehicle.ResetPosition();
        }
    }

    private void SpawnPlayer()
    {
        var go = ObjectPool.Pool.Get();
        go.SetActive(true);
        var vehicle = go.GetComponent<Vehicle>();
        _playerVehicle = vehicle;
        vehicle.underAIControl = false;
        vehicle.SetSprite(settings.PlayerSprite);
        vehicle.SetPosition(new Vector2(_playerSpawn.position.x, _playerSpawn.position.y));
    }

    private void AICarSpawnCheck()
    {
        _aiCarAppearTimer += Time.fixedDeltaTime;
        if (_aiCarAppearTimer > settings.AICarFrequencyMin)
        {
            _aiCarAppearTimer = 0;
            if (Random.value < settings.AICarAppearChance)
            {
                SpawnVehicle();
            }
        }
    }

    private void FixedUpdate()
    {
        AICarSpawnCheck();
    }

    private void Update()
    {
        _gameTimer += Time.deltaTime;
        _timerText.SetText(((int)_gameTimer).ToString());
        if (_playerVehicle != null)
        {
            _playerVehicle.steerSpeed = settings.PlayerSteerSpeed;
        }
    }
}
