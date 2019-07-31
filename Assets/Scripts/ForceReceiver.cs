using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    public float deceleration = 5;
    public float mass = 3;

    private Vector3 intensity; // determines how quickly the knockback effect intensity goes back down to zero
    private CharacterController character;

    // Start is called before the first frame update
    void Start()
    {
        // Set intensity to zero and get reference to character controller
        intensity = Vector3.zero;
        character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // If magnitude of intensity is big enough, apply knockback
        if (intensity.magnitude > 0.2f)
        {
            character.Move(intensity * Time.deltaTime);
        }

        // Calculate intensity value based on deceleration rate
        intensity = Vector3.Lerp(intensity, Vector3.zero, deceleration * Time.deltaTime);
    }

    // Adjust intensity depending on the given direction and force and the object's mass
    public void AddForce (Vector3 direction, float force)
    {
        intensity += direction.normalized * force / mass;
    }
}
