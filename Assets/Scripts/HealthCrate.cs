using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCrate : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private GameObject container;
    [SerializeField] private float rotationSpeed = 180f;

    [Header("Gameplay")]
    public int health = 50;

    // Update is called once per frame
    private void Update()
    {
        // Rotate the health crate based on rotationSpeed
        container.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
