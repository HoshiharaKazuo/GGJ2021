using UnityEngine;

public class GuardEnemyAnimation : MonoBehaviour
{
    public GuardEnemy guardEnemy;


    public void CallShootEvent()
    {
        guardEnemy.Shoot();
    }
}
