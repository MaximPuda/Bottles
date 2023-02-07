using UnityEngine;

[RequireComponent (typeof(Animator))]
public abstract class ItemsCollectorView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _highlight;
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

    public virtual void Highlight(bool active) => _highlight.enabled = active;

    private void OnDisable()
    {
        Collector.ItemAddedEvent -= OnItemAdded;
        Collector.AllItemsCollectedEvent -= OnAllItemsCollected;
        Collector.ClearItemsEvent -= OnClearItems;
    }

    protected abstract void OnItemAdded(ItemController item);
    protected abstract void OnAllItemsCollected(int combo);
    protected abstract void OnClearItems();
}
