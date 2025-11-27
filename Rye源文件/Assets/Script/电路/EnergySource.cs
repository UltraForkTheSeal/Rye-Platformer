using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnergySource : MonoBehaviour {
    public bool isOn = true;
    public LayerMask contactLayer;
    // 新增：记录当前直接接触的触点
    [HideInInspector] public List<ContactPoint> connectedPoints = new List<ContactPoint>();

    private void OnTriggerEnter2D(Collider2D other) {
        if (!isOn) return;
        var cp = other.GetComponent<ContactPoint>();
        if (cp != null && ((1 << other.gameObject.layer) & contactLayer) != 0) {
            if (!connectedPoints.Contains(cp)) connectedPoints.Add(cp);
            cp.SetPowered(true, this);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        var cp = other.GetComponent<ContactPoint>();
        if (cp != null && connectedPoints.Remove(cp)) {
            cp.SetPowered(false, this);
        }
    }
}
