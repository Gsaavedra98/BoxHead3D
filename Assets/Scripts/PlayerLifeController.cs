using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeController : MonoBehaviour
{
    [Header("Vida del jugador")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField, Range(0f , 100f)] private float currentHealth = 100f;
    [Header("Referencia a barra de vida")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject damageWarning;
    private Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;
        StartCoroutine(AlphaAnimation());        
        if (currentHealth <= 0)
        {
            //StartCoroutine(Die());
            Debug.Log("Muricion");
        }
    }

    IEnumerator AlphaAnimation()
    {
        float duration = 0.5f; // Duración total de la animación
        float startAlpha = 0f; // Valor alfa inicial
        float targetAlpha = 0.4f; // Valor alfa objetivo

        // Aumentar el valor alfa
        float timer = 0f;
        while (timer < duration / 2f) // Dividir la duración entre 2 para subir la alpha durante la mitad del tiempo total
        {
            timer += Time.deltaTime;
            float progress = timer / (duration / 2f); // Progreso de 0 a 1
            damageWarning.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(startAlpha, targetAlpha, progress);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f); // Esperar un momento antes de disminuir el valor alfa

        // Disminuir el valor alfa
        timer = 0f;
        while (timer < duration / 2f) // Dividir la duración entre 2 para bajar la alpha durante la mitad del tiempo total
        {
            timer += Time.deltaTime;
            float progress = timer / (duration / 2f); // Progreso de 0 a 1
            damageWarning.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(targetAlpha, startAlpha, progress);
            yield return null;
        }
    }

    public void ApplyKnockback(Vector3 knockbackDirection, float force)
    {
        if (rb != null)
        {
            rb.AddForce(knockbackDirection * force, ForceMode.Impulse);
        }
    }
}
