using System;
using UnityEngine;
using UnityEngine.UI;

public class BoostBar : MonoBehaviour
{
#pragma warning disable 649 // 'field' is never assigned to
    [Tooltip("Sliding bar image")] [SerializeField]
    private Image bar;

    [Tooltip("Reference to boost value")] [SerializeField]
    private FloatVariable boostValue;

    [Tooltip("Threshold for sufficient boost")] [SerializeField]
    private float threshold;

    [Tooltip("Threshold for critical boost")] [SerializeField]
    private float critical;
    
    [Tooltip("Normal colour")] [SerializeField]
    private Color normalColour;
    
    [Tooltip("Enough colour")] [SerializeField]
    private Color thresholdColour;

    [Tooltip("Critical flash colour")] [SerializeField]
    private Color criticalColour;

#pragma warning restore 649 // 'field' is never assigned to

    public enum State
    {
        None = 0,
        Normal,
        Threshold,
        Critical,
        Full
    }

    private State state;
    
    private int counter = 0;

    public State BoostState => state;
    
    private void Awake()
    {
        state = State.None;
    }

    private void Update()
    {
        ++counter;
        if (counter > 60)
        {
            counter = 0;
        }
        
        if (boostValue == null)
        {
            return;
        }

        var amount = Mathf.Clamp01(boostValue);
        bar.fillAmount = amount;

        if (amount < threshold)
        {
            bar.color = normalColour;
            state = State.Normal;
            return;
        }

        if (amount >= critical)
        {
            bar.color = counter  < 20 ? criticalColour : thresholdColour;
            state = Math.Abs(amount - 1f) < 0.0001f ? State.Full : State.Critical;
            return;
        }

        bar.color = thresholdColour;
        state = State.Threshold;
    }
}
