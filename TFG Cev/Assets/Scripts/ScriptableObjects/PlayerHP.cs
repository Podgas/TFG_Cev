using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHP")]
public class PlayerHP : ScriptableObject
{
    public float maxValue;
    float minValue = 0;


    private int _value;
    public int value
    {
        get { return _value; }
        set
        {
            if (_value == value) return;
            _value = value;
            if (OnVariableChange != null)
                OnVariableChange(_value);
        }
    }
    public delegate void OnVariableChangeDelegate(int newVal);
    public event OnVariableChangeDelegate OnVariableChange;

    private void Start()
    {
        componentWithEvent.OnVariableChange += VariableChangeHandler;
    }

    private void VariableChangeHandler(int newVal)
    {
        Debug.Log("Hola");
    }

}
