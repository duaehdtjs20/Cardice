public interface IDamageable
{
    int AttackDamage { get; }
    int Defense { get; }
    int Hp { get; }
    void TakeDamage(int damage);
}
