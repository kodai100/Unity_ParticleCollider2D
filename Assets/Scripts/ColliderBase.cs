using UnityEngine;

public enum ColliderType {
    Circle, Rectangle
}

public abstract class ColliderBase : MonoBehaviour {

    public ColliderType colliderType;
    public bool onGizmo;

    protected virtual void Awake() { }

    protected abstract void OnDrawGizmos();

    protected void OnDestroy() {
        ColliderManager.RemoveFromColliderList(gameObject);
    }
}
