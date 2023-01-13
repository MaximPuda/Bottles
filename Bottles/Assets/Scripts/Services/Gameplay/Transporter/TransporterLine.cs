using UnityEngine;

[System.Serializable]
public class TransporterLine
{
    [SerializeField] private Spawner _spawner;
    [SerializeField] private Transform _itemsContainer;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private int _capacity;

    public Spawner Spawner => _spawner;
    public Transform ItemsContainer =>_itemsContainer;
    public Transform TargetPoint => _targetPoint;
    public int Capacity => _capacity;
}
