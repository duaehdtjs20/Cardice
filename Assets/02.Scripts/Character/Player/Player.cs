using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    private Tile _prevTile;
    private Tile _currentTile;
    private int _attackDamage = 2;
    private int _defense = 2;
    private int _maxHp = 10;
    private int _hp = 10;
    private int _gold = 10;

    public Tile PrevTile => _prevTile;
    public Tile CurrentTile => _currentTile;
    public int AttackDamage => _attackDamage;
    public int Defense => _defense;
    public int Hp => _hp;
    public int MaxHp => _maxHp;
    public int Gold => _gold;

    public void SpawnToTile(Tile spawnTile)
    {
        _currentTile = spawnTile;
        _prevTile = null;

        transform.position = _currentTile.transform.position;
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
    }
    public void RestoreHp(int amount)
    {
        _hp += Mathf.Max(0, amount);
        _hp = Mathf.Min(_hp, _maxHp);
    }
    public void AddGold(int amount)
    {
        _gold += amount;
    }
}
