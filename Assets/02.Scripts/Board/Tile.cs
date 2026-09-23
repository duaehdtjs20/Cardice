using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private ETileType _type;
    [SerializeField] private List<Tile> _nextTiles;
    [SerializeField] private GameObject _arrowPrefab;

    private bool _selectable = false;
    private GameObject _arrow;

    public int Id => _id;
    public ETileType Type => _type;
    public IReadOnlyList<Tile> NextTiles => _nextTiles;
    public bool Selectable => _selectable;

    public void SetArrow(bool flag)
    {
        if(_arrow == null)
        {
            _arrow = Instantiate(_arrowPrefab, transform.position, Quaternion.identity, transform);
        }
        _arrow.SetActive(flag);
        _selectable = flag;
    }
}
