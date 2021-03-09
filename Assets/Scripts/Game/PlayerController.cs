using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    [Header("Transform")]
    public float moveSpeed;
    Vector3 velocity;
    [SerializeField]
    NetworkStartPosition[] startPositions;
    [Header("Health")]
    public int currentHealth;
    public const int maxHealth = 40;
    public Slider healthBar;
    [Header("Shot")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPosition;
    Camera viewCamera;

    void Start() {
        startPositions = FindObjectsOfType<NetworkStartPosition>();
        healthBar.maxValue = maxHealth;
        viewCamera = Camera.main;
        RpcSpawn();
    }

    void Update() {
        
    }

    public void Move() {
        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
        transform.Translate(velocity * Time.deltaTime);
    }

    public void Rotate() {
        Vector3 mousePos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewCamera.transform.position.y));
        transform.LookAt(mousePos + Vector3.up * transform.position.y);
    }

    [Command]
    public void CmdShot() {
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawnPosition.position, bulletSpawnPosition.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 10f;
        NetworkServer.Spawn(bullet);
        Destroy(bullet, 2.5f);
    }

    public void TakenDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            currentHealth = 0;
            Destroy(gameObject);
        }
    }

    [ClientRpc]
    public void RpcSpawn() {
        if (isLocalPlayer) {
            Vector3 p = new Vector3(8.5f, 0.751f, -1.1f);
            if (startPositions != null && startPositions.Length > 0) {
                p = startPositions[Random.Range(0, startPositions.Length)].transform.position;
            }
            transform.position = p;
        }
    }

    public override void OnStartLocalPlayer() {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}
