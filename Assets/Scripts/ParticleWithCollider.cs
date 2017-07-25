using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ParticleWithCollider : MonoBehaviour {

    struct ParticleData {
        public Vector2 initPos;
        public Vector2 pos;
        public Vector2 vel;
        public Color col;
    }

    public int numParticles = 10000;
    public Vector2 rangeMin = new Vector2(0, 0);
    public Vector2 rangeMax = new Vector2(10, 10);
    public Vector2 gravity;
    public ComputeShader cs;
    public Material mat;

    ComputeBuffer particlesBuffer;
    
    ComputeBuffer rectanglesBuffer;
    RectangleColliderData[] rectColliderDatas;  // Pool
    
    ComputeBuffer circlesBuffer;
    CircleColliderData[] circleColliderDatas;   // Pool

   
    void Start () {

        InitializeParticleBuffer();
        InitializeColliderBuffer();
        
    }

    void Update() {

        UpdateColliders();

        cs.SetInt("_NumRectangleCollider", ColliderManager.rectangleColliders.Count);
        cs.SetInt("_NumCircleCollider", ColliderManager.circleColliders.Count);
        cs.SetVector("_RangeMin", rangeMin);
        cs.SetVector("_RangeMax", rangeMax);
        cs.SetFloat("_DeltaTime", Time.deltaTime);
        cs.SetVector("_Gravity", gravity);

        int kernel = cs.FindKernel("Update");
        cs.SetBuffer(kernel, "_ParticlesBuffer", particlesBuffer);
        cs.SetBuffer(kernel, "_RectanglesBuffer", rectanglesBuffer);
        cs.SetBuffer(kernel, "_CirclesBuffer", circlesBuffer);
        cs.Dispatch(kernel, numParticles / 32, 1, 1);

    }

    private void OnRenderObject() {
        mat.SetPass(0);
        mat.SetBuffer("_Particles", particlesBuffer);
        Graphics.DrawProcedural(MeshTopology.Points, numParticles);
    }

    private void OnDestroy() {
        ReleaseBuffer(particlesBuffer);
        ReleaseBuffer(rectanglesBuffer);
        ReleaseBuffer(circlesBuffer);
    }

    void InitializeParticleBuffer() {
        particlesBuffer = new ComputeBuffer(numParticles, Marshal.SizeOf(typeof(ParticleData)));
        ParticleData[] p = new ParticleData[numParticles];
        for (int i = 0; i < numParticles; i++) {
            Vector2 rnd = new Vector2(Random.Range(0f, 10f), Random.Range(7f, 10f));
            p[i].initPos = rnd;
            p[i].pos = rnd;
            p[i].vel = Vector2.zero;
            p[i].col = Color.white;
        }
        particlesBuffer.SetData(p);
    }

    void InitializeColliderBuffer() {
        rectanglesBuffer = new ComputeBuffer(ColliderManager.maxNumRectangles, Marshal.SizeOf(typeof(RectangleColliderData)));
        rectColliderDatas = new RectangleColliderData[ColliderManager.maxNumRectangles];    // プール分先に作っておき、毎フレームnewが走るのを防ぐ

        circlesBuffer = new ComputeBuffer(ColliderManager.maxNumCircles, Marshal.SizeOf(typeof(CircleColliderData)));
        circleColliderDatas = new CircleColliderData[ColliderManager.maxNumRectangles];    // プール分先に作っておき、毎フレームnewが走るのを防ぐ
    }


    void UpdateColliders() {
        UpdateRectangleCollider();
        UpdateCircleCollider();
    }

    void UpdateRectangleCollider() {
        Debug.Log("Num Rectangles : " + ColliderManager.rectangleColliders.Count);
        for (int i = 0; i < ColliderManager.maxNumRectangles; i++) {
            if (i < ColliderManager.rectangleColliders.Count) {
                rectColliderDatas[i].active = ColliderManager.rectangleColliders[i].gameObject.activeInHierarchy;
                rectColliderDatas[i].position = ColliderManager.rectangleColliders[i].transform.position;   // 2D <- 3D
                rectColliderDatas[i].rotation = ColliderManager.rectangleColliders[i].transform.localRotation.eulerAngles.z;
                rectColliderDatas[i].p0 = ColliderManager.rectangleColliders[i].points[0];
                rectColliderDatas[i].p1 = ColliderManager.rectangleColliders[i].points[1];
                rectColliderDatas[i].p2 = ColliderManager.rectangleColliders[i].points[2];
                rectColliderDatas[i].p3 = ColliderManager.rectangleColliders[i].points[3];
            } else {
                rectColliderDatas[i].active = false; // 数が減る場合があるのでListにない要素分はfalseにしておく
            }
        }
        rectanglesBuffer.SetData(rectColliderDatas);
    }

    void UpdateCircleCollider() {
        Debug.Log("Num Circles : " + ColliderManager.circleColliders.Count);
        for (int i = 0; i < ColliderManager.maxNumCircles; i++) {
            if (i < ColliderManager.circleColliders.Count) {
                circleColliderDatas[i].active = ColliderManager.circleColliders[i].gameObject.activeInHierarchy;
                circleColliderDatas[i].position = ColliderManager.circleColliders[i].transform.position;    // 2D <- 3D
                circleColliderDatas[i].radius = ColliderManager.circleColliders[i].radius;
            } else {
                circleColliderDatas[i].active = false;  // 数が減る場合があるのでListにない要素分はfalseにしておく
            }
        }
        circlesBuffer.SetData(circleColliderDatas);
    }

    private void ReleaseBuffer(ComputeBuffer buffer) {
        if(buffer != null) {
            buffer.Release();
        }
    }
}
