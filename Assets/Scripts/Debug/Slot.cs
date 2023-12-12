using UnityEngine;

namespace Common.Debug
{
    public class Slot : MonoBehaviour
    {
        public GridPosition Position { get; private set; }
        public Item Item { get; private set; }

        public Slot SetItem(Item item)
        {
            Item = item; 
            item.transform.SetParent(transform, false);
            return this;
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
            Item?.SetActive(active);
        }

        public Slot Initialise(Vector3 position, GridPosition gridPosition)
        {
            transform.position = position;
            Position = gridPosition;
            return this;
        }
    }
}