using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ViewComponent : NetworkBehaviour
{
    Camera viewCamera;
    void Start() {
        viewCamera = Camera.main;
    }

    void Update() {
        if (!isLocalPlayer) {
            return;
        }
        Rotate();
    }

    public void Rotate() {
        Vector3 mousePos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewCamera.transform.position.y));
        transform.LookAt(mousePos + Vector3.up * transform.position.y);
    }
}
