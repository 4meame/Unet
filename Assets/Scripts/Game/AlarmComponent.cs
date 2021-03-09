using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AlarmComponent : NetworkBehaviour
{
    public Vector3 offset = new Vector3(0.1f,0.0f,0.2f);
    public float sand = 0.3f;
    public float strength = 0.03f;
    Camera camera;

    void Start() {
        camera = Camera.main;
    }

    void Update() {
        if (!isLocalPlayer) {
            return;
        }

        if (gameObject.GetComponent<FieldOfViewComponent>().visibleTargets.Count > 0) {
            Alarm();
        }
    }

    public void Alarm() {
        camera.transform.localPosition -= offset;
        offset = Random.insideUnitSphere / sand * strength;
        camera.transform.localPosition += offset;
    }
}
