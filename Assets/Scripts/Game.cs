using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using TMPro;

public class Game : MonoBehaviour
{ 
    private static Game _instance;

    public static Game Instance => _instance;
    
#pragma warning disable 649 // 'field' is never assigned to
    [Tooltip("Settings")] [SerializeField]
    private Settings settings;
    
    [Tooltip("Car spawn position")] [SerializeField]
    private Transform playerSpawn;
    public Transform PlayerSpawn => playerSpawn;

    [Tooltip("Timer UI")]  [SerializeField]
    private TextMeshProUGUI timerText;
#pragma warning restore 649 // 'field' is never assigned to
    
    private float _gameTimer;
    private float _aiCarAppearTimer;
    
    public Settings Settings => settings;

    private GameObject _playerGo;
    private PlayerVehicle _playerVehicle = null;
    
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
                vehicle.SetMaxSpeed(Random.Range(settings.AICarMinSpeed, settings.AICarMaxSpeed));
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
        _playerGo = Instantiate(Settings.PlayerPrefab, PlayerSpawn.position, Quaternion.identity);
        _playerVehicle = _playerGo.GetComponent<PlayerVehicle>();
        _playerGo.SetActive(true);
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
        timerText.SetText(((int)_gameTimer).ToString());
        if (_playerVehicle != null)
        {
            _playerVehicle.SetSteeringSpeed(settings.PlayerSteerSpeed);
        }
    }
}
