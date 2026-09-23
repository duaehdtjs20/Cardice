using System;

using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IRefreshable
{
    public event Action OnChanged;

    private Tile _prevTile;
    private Tile _currentTile;
    private int _attackDamage = 1;
    private int _defense = 1;
    private int _hp = 20;

    public Tile PrevTile => _prevTile;
    public Tile CurrentTile => _currentTile;
    public int AttackDamage => _attackDamage;
    public int Defense => _defense;
    public int Hp => _hp;

    public void SpawnToTile(Tile spawnTile)
    {
        _currentTile = spawnTile;
        _prevTile = null;

        transform.position = _currentTile.transform.position;

        OnChanged?.Invoke();
    }

    // 다음 타일로 이동
    public void MoveToTile(Tile nextTile)
    {
        if (nextTile == null)
        {
            return;
        }
        if (_currentTile != null)
        {
            _prevTile = _currentTile;
        }
        _currentTile = nextTile;

        // 위치 이동(임시로 순간이동, 추후 이동으로 수정예정)
        transform.position = _currentTile.transform.position;
    }
    public void TakeDamage(int damage)
    {
        _hp -= Mathf.Max(0, damage);

        OnChanged?.Invoke();
    }
}
