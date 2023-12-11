using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Common
{
    public interface IInputSystem
    {
        event Action<Vector3> PointerDown;
        event Action<Vector3> PointerDrag;
        event Action<Vector3> PointerUp;
        bool Lock { get; set; }
    }
    public class InputSystem : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IInputSystem
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Image _inputImage;
        
        public event Action<Vector3> PointerDown;
        public event Action<Vector3> PointerDrag;
        public event Action<Vector3> PointerUp;

        public bool Lock
        {
            get => _inputImage.raycastTarget; 
            set => _inputImage.raycastTarget = !value;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            PointerDown?.Invoke(GetWorldPosition(eventData.position));
        }

        public void OnDrag(PointerEventData eventData)
        {
            PointerDrag?.Invoke(GetWorldPosition(eventData.position));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            PointerUp?.Invoke(GetWorldPosition(eventData.position));
        }

        private Vector3 GetWorldPosition(Vector3 uiPosition)
        {
            return _camera.ScreenToWorldPoint(uiPosition);
        }
    }
}