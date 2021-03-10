using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class AlarmComponent : NetworkBehaviour
{
    [Header("Exposed Index")]
    public const float exposeThresold = 4.0f;
    [SyncVar(hook = "OnExposeChange")]
    public float currentExpose = exposeThresold;
    public float epRecoverTime = 1.0f;
    public Slider exposeBar;
    [Header("Quake Effect")]
    public bool isQuake;
    public Vector3 offset = new Vector3(0.1f,0.0f,0.2f);
    public float sand = 0.3f;
    public float strength = 0.03f;
    Camera camera;

    void Start() {
        camera = Camera.main;
        exposeBar.maxValue = exposeThresold;
    }

    void Update() {
        if (!isServer) {
            return;
        }

        OnExposeChange(currentExpose);

        if (gameObject.GetComponent<FieldOfViewComponent>().visibleTargets.Count > 0) {
            Exposed();
            if (isQuake)
            Quake();
        } else {
            StartCoroutine(ExposeRecoverRoutine());
        }
    }

    public void Exposed() {
        currentExpose = Mathf.Max(0, currentExpose -= Time.deltaTime / 2);
        if (currentExpose <= 0)
            Destroy(gameObject);
            return;
    }

    void OnExposeChange(float currentExpose)
    {
        exposeBar.value = currentExpose;
    }

    IEnumerator ExposeRecoverRoutine() {    
        yield return new WaitForSeconds(epRecoverTime);
        currentExpose = Mathf.Min(exposeThresold, currentExpose += Time.deltaTime / 2);
    }

    public void Quake() {
        camera.transform.localPosition -= offset;
        offset = Random.insideUnitSphere / sand * strength;
        camera.transform.localPosition += offset;
    }
}
