using System.Collections;
using TMPro;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [Header("Variables para el efecto del rayo")]
    [SerializeField] private LineRenderer lineRenderer;
    [Header("Variables de munición y recarga")]
    [SerializeField, Range(1, 100)]
    public int maxAmmo = 30;
    [SerializeField, Range(1.5f, 5f, order = 1)]
    public float reloadTime = 1.5f;
    [SerializeField] private int currentAmmo;
    private bool isReloading = false;
    [Header("Variables de velocidad de disparo (firerate)")]
    [SerializeField, Range(0.1f, 2f)]
    private float fireRate = 0.2f;
    private float nextFire = 0f;
    [Header("Cantidad de daño del arma")]
    [SerializeField, Range(1, 100)] private int damage = 5;
    [SerializeField, Range(1f, 100f)] private float knockbackForce = 100f;
    
    [Header("Referencias")]
    [SerializeField] private TextMeshPro ammoText;
    [SerializeField] private ParticleSystem shootEffect;
    [SerializeField] private Animator weaponAnimator;   

    void Start()
    {
        currentAmmo = maxAmmo;                
    }

    void Update()
    {
        ammoText.text = currentAmmo.ToString();
        // Verifica si se está recargando
        if (isReloading)
            return;

        // Verifica si se ha hecho clic izquierdo y si hay suficiente munición
        if (Input.GetMouseButton(0) && Time.time >= nextFire && currentAmmo > 0)
        {
            nextFire = Time.time + fireRate;
            Shoot();
        }

        // Verifica si se ha hecho clic derecho para recargar
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        // Resta 1 a la munición actual
        currentAmmo--;
         
        // Reproducir la animación de disparo si hay un Animator asignado
        if (weaponAnimator != null) weaponAnimator.Play("Shoot");        
        // Reproducir el efecto de partículas si hay un ParticleSystem asignado
        if (shootEffect != null) shootEffect.Play();
        Vector3 mousePos = Input.mousePosition;
        mousePos += new Vector3(0f,2f,0f);
        // Dispara el rayo con precisión ajustada
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hitEnemy;
        RaycastHit hitObstacle;        
        if (Physics.Raycast(ray, out hitEnemy))
        {
            if(Physics.Raycast(ray, out hitObstacle, 100f, 8)){
                
                DrawRay(transform.position, hitObstacle.point);
                Debug.Log(hitObstacle.point);
                return;
            }
            EnemyController enemy = hitEnemy.collider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                // Calcula la dirección del knockback
                Vector3 knockbackDirection = (enemy.transform.position - transform.position).normalized;
                    
                // Aplica knockback al enemigo
                enemy.ApplyKnockback(knockbackDirection, knockbackForce);
                enemy.TakeDamage(damage);
                ScoreController.Instance.IncreaseMultiplier(10);
                ScoreController.Instance.AddScore(10);
            }
            DrawRay(transform.position, hitEnemy.point);
        }
        else
        {
            Vector3 endPosition = ray.GetPoint(1000);
            DrawRay(transform.position, endPosition);
        }        
    }

    // Método para dibujar el rayo
    void DrawRay(Vector3 startPosition, Vector3 endPosition)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);
        lineRenderer.material.color = Color.white;
        StartCoroutine(FadeOutRay());
    }

    // Método para desvanecer gradualmente el rayo
    IEnumerator FadeOutRay()
    {
        yield return new WaitForSeconds(0.1f);
        lineRenderer.enabled = false;
    }

    // Método para recargar la munición
    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
