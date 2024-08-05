using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using static Unity.Mathematics.math;

public class HashVisualization : MonoBehaviour {
    [SerializeField] Mesh instanceMesh;
    [SerializeField] Material material;
    [SerializeField, Range(1, 512)] int resolution = 16;
    [SerializeField] int seed;
    [SerializeField, Range(-2f, 2f)] float verticalOffset = 1f;
    NativeArray<uint> hashes;
    static int hashesId = Shader.PropertyToID("_Hashes");
    static int configId = Shader.PropertyToID("_Config");
    ComputeBuffer hashesBuffer;
    MaterialPropertyBlock propertyBlock;

    [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
    struct HashJob : IJobFor { // use a job to create hash values for all the cubes in the grid
        [WriteOnly] public NativeArray<uint> hashes;
        public int resolution;
        public float invResolution;
        public SmallXXHash hash;

        public void Execute(int i) {
            int v = (int)floor(invResolution * i + 0.00001f);
            int u = i - resolution * v - resolution / 2;
            v -= resolution / 2;

            hashes[i] = hash.Eat(u).Eat(v);
        }
    }

    void OnEnable() {
        // Because we're not going to animate the hashes
        // we can immediately run the job here and also configure the property block once
        // instead of doing it every update.

        // Store the resolution in the first two components of a configuration vector
        int length = resolution * resolution;
        hashes = new NativeArray<uint>(length, Allocator.Persistent);
        new HashJob {
            hashes = hashes,
            resolution = resolution,
            invResolution = 1f / resolution,
            hash = SmallXXHash.Seed(seed)
        }.ScheduleParallel(hashes.Length, resolution, default).Complete();
        
        hashesBuffer = new ComputeBuffer(length, 4); // int is 4 bytes
        hashesBuffer.SetData(hashes);

        propertyBlock ??= new MaterialPropertyBlock();
        propertyBlock.SetBuffer(hashesId, hashesBuffer);
        propertyBlock.SetVector(configId, new Vector4(resolution, 1f / resolution, verticalOffset / resolution));
    }

    void OnDisable() {
        hashes.Dispose();
        hashesBuffer.Release();
        hashesBuffer = null;
    }

    void OnValidate() {
        if (hashesBuffer != null && enabled) {
            OnDisable();
            OnEnable();
        }
    }

    void Update() {
        // Only need to update draw, keep our grid inside a unit cube at the origin
        Graphics.DrawMeshInstancedProcedural(
            instanceMesh, 0, material, new Bounds(Vector3.zero, Vector3.one),
            hashes.Length, propertyBlock
        );
    }
}