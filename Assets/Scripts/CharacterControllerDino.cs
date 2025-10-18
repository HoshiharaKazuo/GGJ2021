using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerDino : MonoBehaviour
{
    [Header("Componentes Principais")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private CharacterAtack characterAtack;
    private Transform tf;

    [Header("Controle de Movimento")]
    private float move;

    [Header("Sistema de Vida")]
    [SerializeField] private int maxLife = 5;
    [SerializeField] private int currentLife;
    [SerializeField] private float invulnerabilityTime = 1.2f; // tempo de invulnerabilidade após dano
    private bool isInvulnerable = false;

    [Header("Feedback Visual")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color damageColor = Color.red;
    private Color originalColor;

    void Start()
    {
        // Inicializa referências automaticamente
        tf = GetComponent<Transform>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (characterMovement == null) characterMovement = GetComponent<CharacterMovement>();
        if (characterAtack == null) characterAtack = GetComponent<CharacterAtack>();
        if (sr == null) sr = GetComponent<SpriteRenderer>();

        originalColor = sr.color;
        currentLife = maxLife;
    }

    void Update()
    {
        // Inverte o personagem conforme direção do movimento
        if (characterMovement.right)
        {
            tf.rotation = new Quaternion(tf.rotation.x, 0, tf.rotation.z, tf.rotation.w);
        }
        else if (characterMovement.left)
        {
            tf.rotation = new Quaternion(tf.rotation.x, 180, tf.rotation.z, tf.rotation.w);
        }

        // Testes manuais de dano e cura
        if (Input.GetKeyDown(KeyCode.G)) TakeDamage(1);
        if (Input.GetKeyDown(KeyCode.H)) RegenLife(1);

        // Controles principais
        move = Input.GetAxis("Horizontal") * characterMovement.speed;
        characterMovement.Jump(move);
        characterMovement.Dash(move);
        characterMovement.CheckLookAt();
        characterAtack.Sword();
        characterAtack.Shoot();
    }

    private void FixedUpdate()
    {
        characterMovement.move(move, false, false);
    }

    // 🩸 Detecta colisão com inimigos e projéteis
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy") || collision.CompareTag("EnemyBullet"))
        {
            TakeDamage(1);

            // Se for bala, destrói após o impacto
            if (collision.CompareTag("EnemyBullet"))
                Destroy(collision.gameObject);
        }
    }

    // 🧠 Sistema de Dano
    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        currentLife -= damage;
        currentLife = Mathf.Clamp(currentLife, 0, maxLife);
        Debug.Log("🔥 Player tomou dano! Vida atual: " + currentLife);

        StartCoroutine(DamageFlash());
        StartCoroutine(Invulnerability());

        if (currentLife <= 0)
        {
            Die();
        }
    }

    // 💚 Regeneração de Vida
    public void RegenLife(int amount)
    {
        currentLife = Mathf.Min(currentLife + amount, maxLife);
        Debug.Log("💚 Vida regenerada! Vida atual: " + currentLife);
    }

    // ✨ Efeito visual de dano
    private IEnumerator DamageFlash()
    {
        sr.color = damageColor;
        yield return new WaitForSeconds(0.1f);
        sr.color = originalColor;
    }

    // 🛡️ Invulnerabilidade temporária após dano
    private IEnumerator Invulnerability()
    {
        isInvulnerable = true;
        float elapsed = 0f;

        while (elapsed < invulnerabilityTime)
        {
            sr.enabled = !sr.enabled; // pisca o sprite
            elapsed += 0.15f;
            yield return new WaitForSeconds(0.15f);
        }

        sr.enabled = true;
        isInvulnerable = false;
    }

    // 💀 Morte do personagem
    private void Die()
    {
        Debug.Log("💀 Player morreu!");
        // Aqui você pode chamar animação de morte, respawn, ou tela de Game Over
        gameObject.SetActive(false);
    }
}
