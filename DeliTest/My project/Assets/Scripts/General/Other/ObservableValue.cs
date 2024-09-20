using System;
using UnityEngine;
[Serializable]
public class ObservableValue<T>
{
    [SerializeField]
    private T m_value;
    public delegate void OnValueChangeDelegate(T oldValue, T newValue);
    public event OnValueChangeDelegate OnValueChangeEvent;
    public ObservableValue(T value,OnValueChangeDelegate onValueChange)
    {
        this.m_value = value;
        this.OnValueChangeEvent += onValueChange;
    }
    public T Value
    {
        get => m_value;
        set
        {
            T oldValue = this.m_value;
            if (this.m_value != null && this.m_value.Equals(value))
                return;
            this.m_value = value;
            OnValueChangeEvent?.Invoke(oldValue, value);
        }
    }
}
