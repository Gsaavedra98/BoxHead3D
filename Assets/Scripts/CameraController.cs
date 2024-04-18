using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera mainCam;
    [SerializeField] private GameObject player;

    // Define los límites de desplazamiento de la cámara
    [SerializeField, Range(0f, 10f)]
    private float distanceOffSet = 3f;
    

    // Define la sensibilidad del movimiento de la cámara
    [SerializeField] private float sensitivity = 1f;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        // Obtén la posición del mouse en la pantalla
        Vector3 mousePosition = Input.mousePosition;

        // Calcula el desplazamiento de la cámara en los ejes X y Z
        float offsetX = Mathf.Clamp((mousePosition.x / Screen.width - 0.5f) * distanceOffSet * sensitivity, -distanceOffSet, distanceOffSet);
        float offsetZ = Mathf.Clamp((mousePosition.y / Screen.height - 0.5f) * distanceOffSet * sensitivity, -distanceOffSet, distanceOffSet);

        // Actualiza la posición de la cámara
        mainCam.transform.position = new Vector3(player.transform.position.x + offsetX,
                                                 player.transform.position.y + 10f,
                                                 player.transform.position.z - 10f  + offsetZ);
        
    }
}
