using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [Header("Vida del enemigo")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth;
    [Header("Objetivo a pergseguir")]
    [SerializeField] private Transform player; // Referencia al transform del jugador
    [SerializeField] private NavMeshAgent navAgent; // Referencia al NavMeshAgent
    [Header("Daño del enemigo")]
    [SerializeField, Range(1f , 100f)] private float damage = 1f;
    [SerializeField] private float attackTime = 1f;
    [Header("Efectos")]
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject blood;
    private Rigidbody rb;
    private bool attacking = false;

    void Start()
    {       
        rb = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
        healthBar.GetComponent<Slider>().maxValue = maxHealth;
        healthBar.GetComponent<Slider>().minValue = 0;
        healthBar.GetComponent<Slider>().value = currentHealth;
        healthBar.SetActive(false);
    }

    private void Update() {
        // Verificar si el jugador y el NavMeshAgent están configurados
        if (player != null && navAgent != null)
        {
            // Configurar el destino del NavMeshAgent como la posición del jugador
            navAgent.SetDestination(player.position);
        }
        
    }

    // Método para recibir daño
    public void TakeDamage(int damage)
    {
        StartCoroutine(ShowLifeBar());
        currentHealth -= damage;        
        Debug.Log("Enemy health: " + currentHealth);       
        GameObject spawnBlood = Instantiate(blood, transform.position, Quaternion.identity);
        spawnBlood.transform.Rotate(new Vector3(0f, Random.Range(0f, 270f), 0f));
        spawnBlood.transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);        
        healthBar.GetComponent<Slider>().value = currentHealth;
        // Verifica si el enemigo ha sido derrotado
        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }        
        
    }

    IEnumerator ShowLifeBar(){
        healthBar.SetActive(true);
        yield return new WaitForSeconds(1f);
        healthBar.SetActive(false);
    }

    // Método para el comportamiento de muerte del enemigo
    IEnumerator Die()
    {
        Debug.Log("Enemy has been defeated!");
        navAgent.isStopped = true;
        Material alpha = GetComponent<MeshRenderer>().material;
        float duration = 1f;
        float elapsedTime = 0f;
        while(elapsedTime < duration){
            alpha.color -= new Color(alpha.color.r, alpha.color.g, alpha.color.b, 10f);
            elapsedTime += Time.deltaTime;
        }
        yield return new WaitForSeconds(1f);
        Destroy(gameObject); // Destruye el GameObject del enemigo
    }

    public void ApplyKnockback(Vector3 knockbackDirection, float force)
    {
        if (rb != null)
        {
            rb.AddForce(knockbackDirection * force, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){                   
            PlayerLifeController player = other.GetComponent<PlayerLifeController>();
            if(player != null){
                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
                if(!attacking){
                    StartCoroutine(AttackRate(player, knockbackDirection, other));                    
                }
            }
        }
    }

    IEnumerator AttackRate(PlayerLifeController player, Vector3 enemyDir, Collider other){  
        attacking = true;      
        yield return new WaitForSeconds(attackTime);
        other.GetComponent<PlayerController>().enabled = false;
        player.ApplyKnockback(enemyDir, 10f);
        player.TakeDamage(damage);
        yield return new WaitForSeconds(attackTime - 0.5f);
        other.GetComponent<PlayerController>().enabled = true;
        attacking = false;
    }
}
