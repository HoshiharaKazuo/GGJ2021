using UnityEngine;

public class PatrolEnemy: MonoBehaviour
{
    [Header("Configurações de Patrulha")]
    public float patrolDistance = 3f;   // Distância total que o inimigo vai percorrer
    public float speed = 2f;            // Velocidade de movimento

    [Header("Configurações de Vida")]
    [Range(1, 10)] public int maxHealth = 3;  // Vida máxima (slider)
    [Range(0, 10)] public int currentHealth;  // Vida atual (slider)

    private float startX;               // Posição inicial do inimigo
    private int direction = 1;          // 1 = direita, -1 = esquerda
    public SpriteRenderer sr;          // Referência para alterar a cor no dano

    void Start()
    {
        startX = transform.position.x;
        currentHealth = maxHealth;
    }

    void Update()
    {
        PatrolMovement();
    }

    void PatrolMovement()
    {
        // Move o inimigo na direção atual
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // Verifica se atingiu os limites da patrulha
        if (transform.position.x > startX + patrolDistance)
        {
            direction = -1;
            Flip();
        }
        else if (transform.position.x < startX - patrolDistance)
        {
            direction = 1;
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

    // Detecta colisão com projéteis
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject); // Destroi o projétil após o impacto
        }
    }

    void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    System.Collections.IEnumerator DamageFlash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }

    void Die()
    {
        // Destroi o inimigo quando a vida acaba
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
            new Vector3(transform.position.x - patrolDistance, transform.position.y, transform.position.z),
            new Vector3(transform.position.x + patrolDistance, transform.position.y, transform.position.z)
        );
    }
}
