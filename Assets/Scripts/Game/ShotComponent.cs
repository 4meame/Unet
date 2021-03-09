using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShotComponent : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    void Update() {
        if (!isLocalPlayer) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            CmdShot();
        }
    }

    [Command]
    void CmdShot() {
        var bullet = (GameObject)Instantiate(bulletPrefab,bulletSpawn.position, bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 10f;
        NetworkServer.Spawn(bullet);
        Destroy(bullet, 2.5f);
    }
}
