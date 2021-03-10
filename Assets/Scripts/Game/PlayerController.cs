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
    [Header("Shot")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPosition;
    Camera viewCamera;

    void Start() {
        startPositions = FindObjectsOfType<NetworkStartPosition>();
        viewCamera = Camera.main;
        Spawn();
    }

    void Update() {
        if (!isLocalPlayer) {
            return;
        } else {
            Move();
            Rotate();
            if (Input.GetKeyDown(KeyCode.Space))
                CmdShot();
        }
    }

    public void Move() {
        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
        transform.Translate(velocity * Time.deltaTime);
    }

    public void Rotate() {
        Vector3 mousePos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewCamera.transform.position.y));
        if (Vector3.Magnitude(mousePos - transform.position) < 1.6f)
            return;
        transform.LookAt(mousePos + Vector3.up * transform.position.y);
    }

    [Command]
    void CmdShot() {
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawnPosition.position, bulletSpawnPosition.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 10f;
        NetworkServer.Spawn(bullet);
        Destroy(bullet, 2.5f);
    }

    public void Spawn() {
        if (isLocalPlayer) {
            Vector3 p = new Vector3(8.5f, 0.751f, -1.1f);
            if (startPositions != null && startPositions.Length > 0) {
                p = startPositions[Random.Range(0, startPositions.Length)].transform.position;
            }
            transform.position = p;
        }
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}
