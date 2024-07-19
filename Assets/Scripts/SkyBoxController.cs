using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxController : MonoBehaviour
{
    [Range(0.01f, 0.1f)]
    public float rotateSpeed = 0.01f;

    [SerializeField]
    private Material sky;
    private float rotationRepeatValue;

    // Update is called once per frame
    void Update()        
    {
        rotateSpeed = 0.01f;
        rotationRepeatValue = Mathf.Repeat(sky.GetFloat("_Rotation") + rotateSpeed, 360f);

        sky.SetFloat("_Rotation", rotationRepeatValue);
    }
}
