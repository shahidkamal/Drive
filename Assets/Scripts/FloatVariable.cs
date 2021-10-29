using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Float Variable", menuName = "Drive/Float Variable", order = 0)]
public class FloatVariable : ScriptableObject, ISerializationCallbackReceiver
{
    public float InitialValue;

    [HideInInspector]
    [NonSerialized]
    public float Value;

    public void SetValue(float value)
    {
        Value = value;
    }

    public void SetValue(FloatVariable value)
    {
        Value = value.Value;
    }

    // Shorthand for reading the Value from this object as a bool, use with caution
    public static implicit operator float(FloatVariable reference)
    {
        return reference.Value;
    }

    public static implicit operator double(FloatVariable reference)
    {
        return reference.Value;
    }
    
    public void OnAfterDeserialize()
    {
        Value = InitialValue;
    }

    public void OnBeforeSerialize() { }
}
