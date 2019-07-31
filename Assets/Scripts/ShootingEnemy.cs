using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootingEnemy : Enemy
{
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private float shootingInterval = 4f; // How quickly enemies shoot
    [SerializeField] private float shootingDistance = 3f; // How far enemy will shoot from
    [SerializeField] private float chasingInterval = 2f;
    [SerializeField] private float chasingDistance = 12f;

    private Player player;
    private float shootingTimer; // Enemy shoots once timer hits 0
    private float chasingTimer;
    private NavMeshAgent agent;
    private Vector3 initialPos;

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>(); // Get reference to player
        agent = GetComponent<NavMeshAgent>();
        shootingTimer = Random.Range(0, shootingInterval); // sets enemy's shootingTimer to a random number between 0 and shootingInterval
        initialPos = transform.position;

        agent.SetDestination(player.transform.position);
    }

    // Update is called once per frame
    private void Update()
    {
        // If player is dead, stop enemy movement
        if (player.Killed)
        {
            agent.enabled = false;
            this.enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
        }
        shootingTimer -= Time.deltaTime;
        // Check if enemy's shooting timer is 0 and enemy is within shootingDistance from player
        if (shootingTimer <= 0 && Vector3.Distance(transform.position, player.transform.position) <= shootingDistance)
        {
            shootingTimer = shootingInterval;

            GameObject bullet = PoolingManager.Instance.GetBullet(false);
            bullet.transform.position = transform.position;
            bullet.transform.forward = (player.transform.position - transform.position).normalized;
        }

        // Chasing logic
        chasingTimer -= Time.deltaTime;
        if (chasingTimer <= 0 && Vector3.Distance(transform.position, player.transform.position) <= chasingDistance)
        {
            chasingTimer = chasingInterval;
            agent.SetDestination(player.transform.position);
        }
    }

    protected override void OnKill()
    {
        // When killed, disable enemy, play death sound, and have them fall to the ground
        base.OnKill();
        deathSound.Play();
        agent.enabled = false;
        this.enabled = false;
        transform.localEulerAngles = new Vector3(10, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
}
