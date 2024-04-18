using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodEffect : MonoBehaviour
{
    private CanvasGroup fade;

    private void Start() {
        fade = GetComponentInChildren<CanvasGroup>();
    }
    private void Update() {
        fade.alpha -= 0.3f * Time.deltaTime;
        if(fade.alpha == 0){
            Destroy(gameObject);
        }
    }
}
