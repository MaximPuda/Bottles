using UnityEngine;

[RequireComponent (typeof(Animator))]
public abstract class BoxView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _highlight;
    protected BoxController Collector;
    protected Animator Animator;
    protected BoxCell[] Cells;

    public virtual void Initialize(BoxController collector)
    {
        Collector = collector;
        Collector.ItemAddedEvent += OnItemAdded;
        Collector.AllItemsCollectedEvent += OnAllItemsCollected;
        Collector.ClearItemsEvent += OnClearItems;

        Cells = GetComponentsInChildren<BoxCell>();

        Animator = GetComponent<Animator>();
    }

    public virtual void Highlight(bool active) => _highlight.enabled = active;

    private void OnDisable()
    {
        Collector.ItemAddedEvent -= OnItemAdded;
        Collector.AllItemsCollectedEvent -= OnAllItemsCollected;
        Collector.ClearItemsEvent -= OnClearItems;
    }

    protected virtual void OnItemAdded(ItemController item) {}

    protected virtual void OnAllItemsCollected(int combo)
    {
        Animator.SetTrigger("Close");
    }

    protected virtual void OnClearItems() {}
}
