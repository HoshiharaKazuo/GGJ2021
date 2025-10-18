using UnityEngine;

public class GuardEnemy : MonoBehaviour
{
    [Header("Movimentação de Patrulha")]
    [SerializeField] private float patrolDistance = 3f; // Distância que ele percorre
    [SerializeField] private float patrolSpeed = 2f;    // Velocidade de patrulha

    [Header("Detecção e Ataque")]
    [SerializeField] private float detectionRange = 6f; // Distância para começar a atirar
    [SerializeField] private float fireCooldown = 1.5f; // Intervalo entre tiros
    [SerializeField] private Transform bulletSpawnPoint; // Ponto de spawn da bala
    [SerializeField] private GameObject bulletPrefab;    // Prefab da bala
    [SerializeField] private float bulletSpeed = 6f;     // Velocidade da bala

    [Header("Sistema de Vida")]
    [Range(1, 10)] public int maxHealth = 3;
    [Range(0, 10)] public int currentHealth;

    [Header("Configurações Gerais")]
    public bool faceRightByDefault = true;

    private Transform player;
    public SpriteRenderer sr;
    public Animator anim;
    private bool isFacingRight;
    private float fireTimer;
    private float startX;
    private int direction = 1; // 1 = direita, -1 = esquerda
    private bool isPlayerNearby = false;

    void Start()
    {
        isFacingRight = faceRightByDefault;
        currentHealth = maxHealth;
        startX = transform.position.x;

        // Localiza o jogador automaticamente pela tag "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("⚠️ Nenhum objeto com a Tag 'Player' foi encontrado!");
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        fireTimer -= Time.deltaTime;

        // Verifica se o jogador está dentro do alcance de detecção
        isPlayerNearby = distance <= detectionRange;

        if (isPlayerNearby)
        {
            // Para de patrulhar e ataca
            UpdateFacing(player.position.x - transform.position.x);
            if (fireTimer <= 0)
            {
                anim.SetTrigger("attack"); // animação controla o disparo
                fireTimer = fireCooldown;
            }
        }
        else
        {
            // Continua patrulhando normalmente
            PatrolMovement();
        }
    }

    void PatrolMovement()
    {
        transform.Translate(Vector2.right * direction * patrolSpeed * Time.deltaTime);

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

    // Função pública chamada pela animação (Trigger "attack")
    public void Shoot()
    {
        if (bulletPrefab == null || bulletSpawnPoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

        float direction = isFacingRight ? 1f : -1f;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = new Vector2(direction * bulletSpeed, 0);

        Destroy(bullet, 5f);
    }

    // Sistema de dano
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

    // Visualização no editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
            new Vector3(transform.position.x - patrolDistance, transform.position.y, transform.position.z),
            new Vector3(transform.position.x + patrolDistance, transform.position.y, transform.position.z)
        );

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        if (bulletSpawnPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(bulletSpawnPoint.position, 0.1f);
        }
    }
}
