using System;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Screen
{
    public class GameplayScreen : MonoBehaviour
    {
        [SerializeField] private Button _endButton;

        public event Action EndButtonPressedEvent;

        private void Awake()
        {
            _endButton.onClick.AddListener(OnButtonPressed);
            Hide();
        }

        private void OnDestroy()
        {
            _endButton.onClick.RemoveListener(OnButtonPressed);
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        private void OnButtonPressed()
        {
            EndButtonPressedEvent?.Invoke();
            Hide();
        }
    }
}