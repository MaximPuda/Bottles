using UnityEngine;

[RequireComponent (typeof(Animator))]
public abstract class ItemsCollectorView : MonoBehaviour
{
    protected ItemsCollector Collector;
    protected Animator Animator;

    public virtual void Initialize(ItemsCollector collector)
    {
        Collector = collector;
        Collector.ItemAddedEvent += OnItemAdded;
        Collector.AllItemsCollectedEvent += OnAllItemsCollected;
        Collector.ClearItemsEvent += OnClearItems;

        Animator = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        Collector.ItemAddedEvent -= OnItemAdded;
        Collector.AllItemsCollectedEvent -= OnAllItemsCollected;
        Collector.ClearItemsEvent -= OnClearItems;
    }

    protected abstract void OnItemAdded(Item item);
    protected abstract void OnAllItemsCollected(int combo);
    protected abstract void OnClearItems();
}
