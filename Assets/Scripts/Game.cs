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
    
    [Tooltip("Player vehicle")] [SerializeField]
    private PlayerVehicle _playerVehicle;
    
    [Tooltip("Player Speed")] [SerializeField]
    private FloatVariable playerSpeed;

    public FloatVariable PlayerSpeed => playerSpeed;
    
    [Tooltip("Timer UI")]  [SerializeField]
    private TextMeshProUGUI timerText;
    
#pragma warning restore 649 // 'field' is never assigned to
    
    private float _gameTimer;
    private float _aiCarAppearTimer;
    public Camera TrackingCamera { get; private set; }

    public Settings Settings => settings;

    private GameObject _playerGo;

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
        TrackingCamera = Camera.main;
        ScreenWidthPixels = Screen.width;
        ScreenHeightPixels = Screen.height;

        var topleft = TrackingCamera.ScreenToWorldPoint(new Vector3(0, 0, TrackingCamera.nearClipPlane));
        var bottomRight = TrackingCamera.ScreenToWorldPoint(new Vector3(ScreenWidthPixels, ScreenHeightPixels, TrackingCamera.nearClipPlane));
        ScreenWidthUnits = bottomRight.x - topleft.x;
        ScreenHeightUnits = bottomRight.y - topleft.y;
    }

    private void Start()
    {
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
    
    private static void SpawnVehicle()
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
            //vehicle.WakePhysics();
            //vehicle.EnableCollder();
        }
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
