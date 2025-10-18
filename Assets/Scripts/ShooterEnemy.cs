using UnityEngine;

public class ShooterEnemy : MonoBehaviour
{
    [Header("Detecção e Ataque")]
    [SerializeField] private float detectionRange = 6f; // Distância para começar a atirar
    [SerializeField] private float fireCooldown = 1.5f; // Intervalo entre tiros
    [SerializeField] private Transform bulletSpawnPoint; // Local de spawn da bala
    [SerializeField] private GameObject bulletPrefab;    // Prefab da bala
    [SerializeField] private float bulletSpeed = 6f;     // Velocidade do projétil

    [Header("Sistema de Vida")]
    [Range(1, 10)] public int maxHealth = 3; // Vida máxima (slider)
    [Range(0, 10)] public int currentHealth; // Vida atual (slider)

    [Header("Configurações Gerais")]
    public bool faceRightByDefault = true; // Direção inicial do sprite

    private Transform player;
    public SpriteRenderer sr;
    public Animator anim;
    private bool isFacingRight;
    private float fireTimer;

    void Start()
    {
        isFacingRight = faceRightByDefault;
        currentHealth = maxHealth;

        // Encontra o player automaticamente pela tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("⚠️ Nenhum objeto com a Tag 'Player' foi encontrado na cena!");
    }

    void Update()
    {
        if (player == null) return;

        // Calcula distância horizontal até o jogador
        float distance = Vector2.Distance(transform.position, player.position);

        // Atualiza a direção que o inimigo está olhando
        UpdateFacing(player.position.x - transform.position.x);

        // Fica parado, mas atira se o jogador estiver próximo
        fireTimer -= Time.deltaTime;
        if (distance <= detectionRange && fireTimer <= 0)
        {
            anim.SetTrigger("attack"); // Dispara animação de ataque
            fireTimer = fireCooldown;  // Reinicia cooldown
        }
    }

    // --- Atualiza direção visual ---
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

    // --- Função pública chamada pela animação ---
    public void Shoot()
    {
        if (bulletPrefab == null || bulletSpawnPoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

        // Define direção do tiro
        float direction = isFacingRight ? 1f : -1f;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = new Vector2(direction * bulletSpeed, 0);

        Destroy(bullet, 5f); // Auto-destrói a bala após 5 segundos
    }

    // --- Sistema de dano ---
    void OnTriggerEnter2D(Collider2D other)
    {
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

    // --- Visualização no Editor ---
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        if (bulletSpawnPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(bulletSpawnPoint.position, 0.1f);
        }
    }
}
