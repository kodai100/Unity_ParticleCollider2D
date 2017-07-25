using UnityEngine;

public enum MoveType {
    Translate, Rotate, Both
}

public class AutoMover2D : MonoBehaviour {
    
    public MoveType moveType;

    public float translateStrength;
    public float translateSpeed;

    public float rotateStrength;

    private float randOffset;
    private float time = 0;
    
	void Start () {
        randOffset = Random.Range(0f, 2 * Mathf.PI);
        time = 0;
	}
	
	void Update () {
        time += Time.deltaTime;

		if(moveType == MoveType.Translate) {
            transform.SetPositionAndRotation(new Vector3(translateStrength * Mathf.PerlinNoise(randOffset + time * translateSpeed, randOffset + time * translateSpeed), translateStrength * Mathf.PerlinNoise(randOffset*2 + time * translateSpeed, randOffset*2 + time * translateSpeed), 0), Quaternion.identity);
        } else if(moveType == MoveType.Rotate) {
            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, randOffset + rotateStrength * time));
        } else {
            transform.SetPositionAndRotation(new Vector3(translateStrength * Mathf.PerlinNoise(randOffset + time * translateSpeed, randOffset + time * translateSpeed), translateStrength * Mathf.PerlinNoise(randOffset * 2 + time * translateSpeed, randOffset * 2 + time * translateSpeed), 0), Quaternion.Euler(0, 0, randOffset + rotateStrength * time));
        }
	}
}
