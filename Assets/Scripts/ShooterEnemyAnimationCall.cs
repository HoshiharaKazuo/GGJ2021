using UnityEngine;

public class ShooterEnemyAnimationCall : MonoBehaviour
{
    public ShooterEnemy shooterEnemy;
    
    public void CallShootEvent()
    {
        shooterEnemy.Shoot();
    }
}
