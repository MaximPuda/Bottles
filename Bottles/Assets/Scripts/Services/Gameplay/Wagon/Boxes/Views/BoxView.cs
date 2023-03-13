using UnityEngine;

[RequireComponent (typeof(Animator))]
public abstract class BoxView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _highlight;
    protected BoxController Controller;
    protected Animator Animator;
    protected BoxCell[] Cells;

    public virtual void Initialize(BoxController controller)
    {
        Controller = controller;
        Controller.ItemAddedEvent += OnItemAdded;
        Controller.AllItemsCollectedEvent += OnAllItemsCollected;
        Controller.ClearItemsEvent += OnClearItems;

        Cells = GetComponentsInChildren<BoxCell>();

        Animator = GetComponent<Animator>();
    }

    public virtual void Highlight(bool active) => _highlight.enabled = active;

    private void OnDisable()
    {
        Controller.ItemAddedEvent -= OnItemAdded;
        Controller.AllItemsCollectedEvent -= OnAllItemsCollected;
        Controller.ClearItemsEvent -= OnClearItems;
    }

    protected virtual void OnItemAdded(ItemController item) {}

    protected virtual void OnAllItemsCollected(int combo)
    {
        Animator.SetTrigger("Close");
    }

    protected virtual void OnClearItems() {}
}
