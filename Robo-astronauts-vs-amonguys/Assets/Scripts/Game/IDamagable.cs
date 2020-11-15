public interface IDamagable
{
    int Max_health {get; }
    int Health {get; }
    void TakeDamage(int amount);
}
