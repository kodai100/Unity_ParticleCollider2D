using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Explicit)]
public struct RectangleColliderData {
    [FieldOffset(0)]
    public bool active;
    [FieldOffset(4)]
    public Vector2 position;
    [FieldOffset(12)]
    public float rotation;
    [FieldOffset(16)]
    public Vector2 p0;
    [FieldOffset(24)]
    public Vector2 p1;
    [FieldOffset(32)]
    public Vector2 p2;
    [FieldOffset(40)]
    public Vector2 p3;
}


public class RectangleCollider : ColliderBase {

    public Vector2[] points = new Vector2[4];

    protected override void OnDrawGizmos() {
        if (onGizmo) {
            float rot = transform.localRotation.eulerAngles.z;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position + Rotate(rot, points[0]), transform.position + Rotate(rot, points[1]));
            Gizmos.DrawLine(transform.position + Rotate(rot, points[1]), transform.position + Rotate(rot, points[2]));
            Gizmos.DrawLine(transform.position + Rotate(rot, points[2]), transform.position + Rotate(rot, points[3]));
            Gizmos.DrawLine(transform.position + Rotate(rot, points[3]), transform.position + Rotate(rot, points[0]));
        }
        
    }

    private Vector3 Rotate(float theta, Vector2 input) {

        float x = Mathf.Cos(theta) * input.x - Mathf.Sin(theta) * input.y;
        float y = Mathf.Sin(theta) * input.x + Mathf.Cos(theta) * input.y;

        return new Vector3(x, y, 0);
    }
}
