using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCrate : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private GameObject container;
    [SerializeField] private float rotationSpeed = 180f;

    [Header("Gameplay")]
    public int ammo = 12;

    // Update is called once per frame
    private void Update()
    {
        // Rotate the ammo crate based on rotationSpeed
        container.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
