using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetPositionComponent : NetworkBehaviour
{
    NetworkStartPosition[] startPositions;

    private void Start() {
        startPositions = FindObjectsOfType<NetworkStartPosition>();
        Spawn();
    }

    void Spawn() {
        if (isLocalPlayer) {
            Vector3 p = new Vector3(8.5f,0.751f,-1.1f);
            if (startPositions != null && startPositions.Length > 0) {
                p = startPositions[Random.Range(0, startPositions.Length)].transform.position;
            }
            transform.position = p;
        }
    }
}
