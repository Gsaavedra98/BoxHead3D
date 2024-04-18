using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [Header("Objeto a seguir")]
    [SerializeField] private GameObject objectToFollow;
    [Header("Variables")]
    [SerializeField, Range(0f, 10f)]
    private float offSetX;
    [SerializeField, Range(0f, 10f)]
    private float offSetY;
    [SerializeField, Range(0f, 10f)]
    private float offSetZ;
    [SerializeField] private bool lookAtCamera = false;
    private Camera mainCam;

    private void Start() {
        mainCam = Camera.main;
    }
    
    void Update()
    {
        if(lookAtCamera)
        transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);
        transform.position = new Vector3(objectToFollow.transform.position.x + offSetX,
                                        objectToFollow.transform.position.y + offSetY,
                                        objectToFollow.transform.position.z + offSetZ);
    }
}
