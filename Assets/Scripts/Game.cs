using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using TMPro;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public const int NumLanes = 4;
    public const float LaneWidth = 1.333f;
    
#pragma warning disable 649 // 'field' is never assigned to
    [Tooltip("Settings")] [SerializeField]
    private Settings settings;
    
    [Tooltip("Car spawn position")] [SerializeField]
    private Transform playerSpawn;
    public Transform PlayerSpawn => playerSpawn;

    [Tooltip("Player Speed")] [SerializeField]
    private FloatVariable playerSpeed;

    public FloatVariable PlayerSpeed => playerSpeed;
    
    [Tooltip("Timer UI")]  [SerializeField]
    private TextMeshProUGUI timerText;
#pragma warning restore 649 // 'field' is never assigned to
    
    private float _gameTimer;
    private float _aiCarAppearTimer;
    private Camera _camera;
    
    public Settings Settings => settings;

    private GameObject _playerGo;
    private PlayerVehicle _playerVehicle;
    public float ScreenWidthPixels { get; private set; }
    public float ScreenHeightPixels { get; private set; }
    
    public float ScreenWidthUnits { get; private set; }
    public float ScreenHeightUnits { get; private set; }
    
    
    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Determine screen world space boundaries irrespective of screen size or aspect ratio
        _camera = Camera.main;
        ScreenWidthPixels = Screen.width;
        ScreenHeightPixels = Screen.height;

        var topleft = _camera.ScreenToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));
        var bottomRight = _camera.ScreenToWorldPoint(new Vector3(ScreenWidthPixels, ScreenHeightPixels, _camera.nearClipPlane));
        ScreenWidthUnits = bottomRight.x - topleft.x;
        ScreenHeightUnits = bottomRight.y - topleft.y;
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
            vehicle.EnableCollder();
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
