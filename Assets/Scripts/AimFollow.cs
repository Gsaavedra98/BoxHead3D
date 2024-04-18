using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimFollow : MonoBehaviour
{   
    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        gameObject.GetComponent<RectTransform>().transform.position = mousePosition;
    }
}
