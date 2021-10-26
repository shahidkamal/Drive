using UnityEngine;

[CreateAssetMenu(menuName="Drive/Settings")]
public class Settings : ScriptableObject
{
    [Header("AI")]
    [Tooltip("Maximum Active Cars")] [SerializeField]
    private int maxActiveCars;
    public int MaxActiveCars => maxActiveCars;

    [Tooltip("Vehicle sprites")] [SerializeField]
    private Sprite[] vehicleSprites;
    public Sprite[] VehicleSprites => vehicleSprites;

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
    
    [Header("Player")]
    [Tooltip("Player Sprite")] [SerializeField]
    private Sprite playerSprite;

    public Sprite PlayerSprite => playerSprite;

    [Tooltip("Player steering speed")] [SerializeField]
    private float playerSteerSpeed;
    public float PlayerSteerSpeed => playerSteerSpeed;

    [Tooltip("Left screen bound")] [SerializeField]
    private float playerLeftScreenBound;

    public float PlayerLeftScreenBound => playerLeftScreenBound;
    
    
    [Tooltip("Right screen bound")] [SerializeField]
    private float playerRightScreenBound;

    public float PlayerRightScreenBound => playerRightScreenBound;
    
    [Header("Keys")]
    [Tooltip("Right")] 
    [SerializeField]
    private KeyCode keyRight;

    public KeyCode KeyRight => keyRight;
    
    [Tooltip("Left")] 
    [SerializeField]
    private KeyCode keyLeft;

    public KeyCode KeyLeft => keyLeft;

}
