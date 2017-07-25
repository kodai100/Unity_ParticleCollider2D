using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : SingletonMonoBehaviour<ColliderManager> {

    public static readonly int maxNumRectangles = 500;  // For compute bufer pool
    public int rectInstantiateNum = 10;
    public GameObject rectangleColliderPrefab;

    public static readonly int maxNumCircles = 50;      // For compute bufer pool
    public int circleInstantiateNum = 10;
    public GameObject circleColliderPrefab;

    public static List<RectangleCollider> rectangleColliders = new List<RectangleCollider>();
    public static List<CircleCollider> circleColliders = new List<CircleCollider>();
    
    /// <summary>
    /// Initialization
    /// </summary>
    void Start () {
        InstantiateRandomColliders(rectangleColliderPrefab, rectInstantiateNum);
        InstantiateRandomColliders(circleColliderPrefab, circleInstantiateNum);
    }

    /// <summary>
    /// Specify the collider type and add to the specified collider list
    /// </summary>
    /// <param name="obj"></param>
    public static void AddToColliderList(GameObject obj) {
        ColliderBase colliderBase = obj.GetComponent<ColliderBase>();

        if (colliderBase.colliderType == ColliderType.Rectangle) {
            rectangleColliders.Add(obj.GetComponent<RectangleCollider>());
        } else if(colliderBase.colliderType == ColliderType.Circle) {
            circleColliders.Add(obj.GetComponent<CircleCollider>());
        } else {
            Debug.LogWarning("<color=red>Add : Illegal collider type.</color>");
        }
    }

    /// <summary>
    /// Specify the collider type and remove from the specified collider list
    /// </summary>
    /// <param name="obj">A GameObject instance</param>
    public static void RemoveFromColliderList(GameObject obj) {
        ColliderBase colliderBase = obj.GetComponent<ColliderBase>();

        if (colliderBase.colliderType == ColliderType.Rectangle) {
            rectangleColliders.Remove(obj.GetComponent<RectangleCollider>());
            Debug.Log("<color=cyan>Successfully removed from Rectangles List</color>");
        } else if (colliderBase.colliderType == ColliderType.Circle) {
            circleColliders.Remove(obj.GetComponent<CircleCollider>());
            Debug.Log("<color=cyan>Successfully removed from Circles List</color>");
        } else {
            Debug.LogWarning("<color=red>Remove : Illegal collider type.</color>");
        }
    }
    
    /// <summary>
    /// Instantiate random colliders (optional)
    /// </summary>
    private void InstantiateRandomColliders(GameObject collider, int num) {
        for (int i = 0; i < num; i++) {
            GameObject o = Instantiate(collider, new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), 0), Quaternion.identity, transform);
            AddToColliderList(o);
        }
    }
    
}
