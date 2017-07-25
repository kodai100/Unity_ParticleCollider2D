using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Explicit)]
public struct CircleColliderData {
    [FieldOffset(0)]
    public bool active;
    [FieldOffset(4)]
    public Vector2 position;
    [FieldOffset(12)]
    public float radius;
}

public class CircleCollider : ColliderBase {
    public float radius;
    
    protected override void OnDrawGizmos() {
        if (onGizmo) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}