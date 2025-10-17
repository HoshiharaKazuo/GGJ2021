using UnityEngine;

public class PatrolEnemySimple : MonoBehaviour
{
    [Header("Configurações de Patrulha")]
    public float patrolDistance = 3f;   // Distância total que o inimigo vai percorrer
    public float speed = 2f;            // Velocidade de movimento

    private float startX;               // Posição inicial do inimigo
    private int direction = 1;          // 1 = direita, -1 = esquerda

    void Start()
    {
        startX = transform.position.x;
    }

    void Update()
    {
        // Move o inimigo na direção atual
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // Verifica se atingiu os limites da patrulha
        if (transform.position.x > startX + patrolDistance)
        {
            direction = -1; // Inverte direção (vai para a esquerda)
            Flip();
        }
        else if (transform.position.x < startX - patrolDistance)
        {
            direction = 1; // Inverte direção (vai para a direita)
            Flip();
        }
    }

    void Flip()
    {
        // Espelha o inimigo horizontalmente
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnDrawGizmosSelected()
    {
        // Mostra o limite da patrulha no editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(transform.position.x - patrolDistance, transform.position.y, transform.position.z),
                        new Vector3(transform.position.x + patrolDistance, transform.position.y, transform.position.z));
    }
}
