using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewComponent : MonoBehaviour
{
    [Range(0,55)]
    public float viewRaidus;
    [Range(0,360)]
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstractMask;
    public List<Transform> visibleTargets = new List<Transform>();

    void Start() {
        StartCoroutine(FindTargetDelay(0.2f));
    }

    IEnumerator FindTargetDelay(float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            FindVisibleTarget();
        }
    }
    /// <summary>
    /// 找到角色视野内的可见目标
    /// </summary>
    void FindVisibleTarget() {
        visibleTargets.Clear();
        Collider[] targetsInVisibleRadius = Physics.OverlapSphere(transform.position, viewRaidus, targetMask);
        for (int i = 0; i < targetsInVisibleRadius.Length; i++) {
            Transform target = targetsInVisibleRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstractMask)) {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    /// <summary>
    /// 计算角色正面方向的视野角度
    /// </summary>
    /// <param name="angleInDegrees"></param>
    /// <param name="isGlobalAngle"></param>
    /// <returns></returns>
    public Vector3 DirFromAngle(float angleInDegrees, bool isGlobalAngle) {
        if (!isGlobalAngle) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
