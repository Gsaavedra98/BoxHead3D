using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, Range(1f, 10f)] private float moveSpeed = 5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Obtener la entrada del teclado
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // Normalizar la dirección para evitar movimientos diagonales más rápidos
        Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized;

        // Mover el jugador en la dirección deseada
        rb.velocity = movement * moveSpeed;

        // Obtener la posición del mouse en la ventana
        Vector3 mousePosition = Input.mousePosition;

        // Convertir la posición del mouse de pantalla a rayo en el mundo
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Obtener la posición del mouse en el mundo
            Vector3 targetPosition = hit.point;

            // Calcular la dirección hacia la posición del mouse
            Vector3 direction = targetPosition - transform.position;

            // Obtener el ángulo de rotación en el eje Y
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // Aplicar la rotación al jugador en el eje Y
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }
}
