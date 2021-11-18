using UnityEngine;

[CreateAssetMenu(menuName="Drive/Settings")]
public class Settings : ScriptableObject
{
    [Header("AI")]
    [Tooltip("Maximum Active Cars")] [SerializeField]
    private int maxActiveCars;
    public int MaxActiveCars => maxActiveCars;
    
    [Tooltip("Bad guy car prefabs")] [SerializeField]
    private GameObject[] vehiclePrefabs;
    public GameObject[] VehiclePrefabs => vehiclePrefabs;

    [Tooltip("AI car minimum speed")] [SerializeField]
    private float aiCarMinSpeed;
    public float AICarMinSpeed => aiCarMinSpeed;
    
    [Tooltip("AI car maximum speed")] [SerializeField]
    private float aiCarMaxSpeed;
    public float AICarMaxSpeed => aiCarMaxSpeed;

    [Tooltip("AI Car Appearance Frequency Minimum")] [SerializeField]
    private float aiCarFrequencyMin;

    public float AICarFrequencyMin => aiCarFrequencyMin;
    
    [Tooltip("AI Car Appearance Chance")] [SerializeField]
    private float aiCarAppearChance;

    public float AICarAppearChance => aiCarAppearChance;

    [Tooltip("AI Car Initial Power")] [SerializeField]
    private float aiInitialPower;

    public float AIInitialPower => aiInitialPower;
    
    [Header("Player")]
    [Tooltip("Player Prefab")] [SerializeField]
    private GameObject playerPrefab;

    public GameObject PlayerPrefab => playerPrefab;

    [Tooltip("Player steering speed")] [SerializeField]
    private float playerSteerSpeed;
    public float PlayerSteerSpeed => playerSteerSpeed;

    [Header("Keys")]
        
    [Tooltip("Left")] 
    [SerializeField]
    private KeyCode keyLeft;

    public KeyCode KeyLeft => keyLeft;
    
    [Tooltip("Right")] 
    [SerializeField]
    private KeyCode keyRight;
    public KeyCode KeyRight => keyRight;
    
    [Tooltip("Boost")]
    [SerializeField]
    private KeyCode keyBoost;
    public KeyCode KeyBoost => keyBoost;


}
