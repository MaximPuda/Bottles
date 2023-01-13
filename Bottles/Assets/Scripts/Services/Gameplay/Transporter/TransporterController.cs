using UnityEngine;
using UnityEngine.Events;

public class TransporterController : Controller
{
    [SerializeField] private ItemController _prefab;
    [SerializeField] private TransporterLine[] _lines;
    [SerializeField] private float _distanceBetween;
    [SerializeField] private float _speed;
    [SerializeField] private bool _enabled;

    public event UnityAction<int> ItemsLeftEvent;

    private ItemPool _itemPool;

    public override void Initialize(Service service)
    {
        base.Initialize(service);

        if (_lines != null)
        {
            Level level = ((GamePlayService)CurrentService).LevelCTRL.CurrentLevel;
            _itemPool = new ItemPool("MainItemPool", level.ItemsAmount, _prefab, transform);
            _itemPool.PoolChangeEvent += OnItmesLeft;
            _itemPool.PoolEmptyEvent += OnItemsOver;

            foreach (var line in _lines)
            {
                line.Spawner.Initialize(_itemPool, line.Capacity, line.ItemsContainer, level.ItemTypes, level.ColorPalettes);
            }
        }
    }

    private void OnDisable()
    {
        if(_itemPool != null)
        {
            _itemPool.PoolChangeEvent -= OnItmesLeft;
            _itemPool.PoolEmptyEvent -= OnItemsOver;
        }
    }

    private void Update()
    {
        if (_enabled)
            MoveAll();
    }

    private void MoveAll()
    {
        if(_lines != null)
        {
            foreach (var line in _lines)
            {
                TryToMove(line.TargetPoint, line.ItemsContainer);
            }
        }
    }

    private void TryToMove(Transform targetPoint, Transform itemsContainer)
    {
        if (targetPoint != null)
        {
            for (int i = 0; i < itemsContainer.childCount; i++)
            {
                var itemTransform =itemsContainer.GetChild(i);
                if(i > 0)
                {
                    float newX = targetPoint.position.x - (_distanceBetween * i);
                    var newPos = new Vector2(newX, targetPoint.position.y);
                    float dist = newPos.x - itemTransform.position.x;

                    if (dist > 0.01f)
                        itemTransform.position = Vector2.Lerp(itemTransform.position, newPos, _speed * Time.deltaTime);
                }
                else
                {
                    float dist = targetPoint.position.x - itemTransform.position.x;

                    if (dist > 0.01f)
                        itemTransform.position = Vector2.Lerp(itemTransform.position, targetPoint.position, _speed * Time.deltaTime);
                }
            }
        }
        else Debug.LogWarning("Target point doesnt set!");
    }

    public void Active(bool active) => _enabled = active;

    private void OnItmesLeft(int itemsLeft)
    {
        ItemsLeftEvent?.Invoke(itemsLeft);
    }

    private void OnItemsOver()
    {

    }
}
