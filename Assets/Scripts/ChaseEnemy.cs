using UnityEngine;

public class ChaseEnemy : MonoBehaviour
{
    [Header("Configurações de perseguição")]
    public float speed = 2f;            // Velocidade de perseguição
    public float detectionRange = 5f;   // Distância para começar a perseguir
    public bool faceRightByDefault = true; // Ajuste conforme a direção do sprite

    [Header("Sistema de Vida")]
    [Range(1, 10)] public int maxHealth = 3; // Slider no Inspector
    [Range(0, 10)] public int currentHealth; // Mostra vida atual como slider

    private Transform player;           // Referência automática ao jogador
    public SpriteRenderer sr;
    private bool isFacingRight;
    private bool isChasing = false;

    void Start()
    {
        isFacingRight = faceRightByDefault;
        currentHealth = maxHealth;

        // Procura o jogador automaticamente pela Tag "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("⚠️ Nenhum objeto com a Tag 'Player' foi encontrado na cena!");
    }

    void Update()
    {
        if (player == null) return;

        // Calcula distância no eixo X
        float distanceX = Mathf.Abs(player.position.x - transform.position.x);

        // Ativa perseguição se o jogador estiver dentro do alcance
        isChasing = distanceX <= detectionRange;

        // Movimento apenas no eixo X
        if (isChasing)
        {
            Vector2 target = new Vector2(player.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            UpdateFacing(player.position.x - transform.position.x);
        }
    }

    void UpdateFacing(float directionX)
    {
        if (directionX > 0 && !isFacingRight)
            Flip();
        else if (directionX < 0 && isFacingRight)
            Flip();
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        sr.flipX = !sr.flipX;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Se colidir com objeto que tem a Tag "Bullet"
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
            Die();
    }

    System.Collections.IEnumerator DamageFlash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
