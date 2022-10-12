using System;
using System.Collections.Generic;
using AssetManaging;
using UnityEngine;

namespace Ui
{
    public interface IUiPanel
    {
        MainCanvas Main { get; set; }
        GameObject gameObject { get; }
        void OnFocused(bool isFocused);
        void OnRemoved();
    }
    
    public class MainCanvas : MonoBehaviour
    {
        public bool KeepBottomPanel;
        public bool CanExitCurrent = true;
        
        private List<IUiPanel> _registeredPanels;
        private Stack<IUiPanel> _panels;

        private void Awake()
        {
            _registeredPanels = new();
            _panels = new();
        }

        private void Start()
        {
            RegisterAllPrefabPanels();
        }
        
        private void RegisterAllPrefabPanels()
        {
            var uiPanels = PrefabManager.GetPrefabs("Ui/Panels/");
            for (int i = 0; i < uiPanels.Length; i++)
            {
                var panelGo = uiPanels[i];
                var go = Instantiate(panelGo, transform);
                var uiPanel = go.GetComponent<IUiPanel>();
                if (uiPanel == null)
                {
                    Debug.LogWarning($"UiPanel does not inherit IUiPanel: {panelGo.name}");
                    continue;
                }

                _registeredPanels.Add(uiPanel);
                go.SetActive(false);
            }
        }
        
        public T PushPanel<T>() where T : IUiPanel
        {
            for (int i = 0; i < _registeredPanels.Count; i++)
                if (_registeredPanels[i] is T ret)
                {
                    PushPanel(ret);
                    return ret;
                }

            Debug.LogError($"Could not find panel type: {typeof(T).Name}");
            return default;
        }
        
        public void PushPanel(Type type)
        {
            if (type == null)
            {
                Debug.LogError("Panel type was null");
                return;
            }

            for (int i = 0; i < _registeredPanels.Count; i++)
            {
                var regType = _registeredPanels[i].GetType();
                if (regType == type)
                {
                    PushPanel(_registeredPanels[i]);
                    return;
                }
            }

            Debug.LogError($"Could not find panel type: {type.Name}");
        }
        
        public void PushPanel(IUiPanel panel)
        {
            if (_panels.Count > 0)
            {
                SetCurrentIsEnabled(false);
            }

            panel.Main = this;
            panel.gameObject.SetActive(true);
            panel.OnFocused(true);
            _panels.Push(panel);
        }
        
        private void SetCurrentIsEnabled(bool isEnabled)
        {
            if (_panels.Count <= 0)
                return;

            var currentPanel = _panels.Peek();
            currentPanel.gameObject.SetActive(isEnabled);
            currentPanel.OnFocused(isEnabled);
        }

        public void ExitPanel()
        {
            var current = _panels.Pop();
            current.gameObject.SetActive(false);
            current.OnFocused(false);
            current.OnRemoved();
            SetCurrentIsEnabled(true);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // if (_panels.Count <= 0)
                // {
                    // PushPanel(DefaultPanelPrefab.GetType());
                // }
                // else if (CanExitCurrent)
                if (CanExitCurrent)
                {
                    if (_panels.Count == 1)
                    {
                        if (!KeepBottomPanel)
                            ExitPanel();
                    }
                    else
                    {
                        ExitPanel();
                    }
                }
            }
        }
        
    }
}