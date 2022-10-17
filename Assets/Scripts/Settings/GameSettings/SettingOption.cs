using System;

namespace Settings.GameSettings
{
    public class SettingOption<T>
    {
        public static implicit operator T(SettingOption<T> settingOption) => settingOption.Value;
        
        public string Name { get; private set; }
        public T Value { get; private set; }

        public event Action OnChangedEvent;

        public SettingOption(string name, T value)
        {
            Name = name;
            Value = value;
            OnChangedEvent = null;
        }

        public void SetValue(T value)
        {
            Value = value;
            OnChangedEvent?.Invoke();
        }
    }
}