using UnityEngine;
using UnityEngine.Rendering;
using Unity.Jobs;

public class BatchRendererGroupTest : MonoBehaviour
{
    // Allow us to set the mesh and material from the inspector
    public GameObject ObjectWeGetTheMeshFrom;
    public Material _material;

    // The batch. 
    BatchRendererGroup _brg;
    int _batch;

    void OnEnable()
    {
        _brg = new BatchRendererGroup(CullingCallback);

        var mesh = ObjectWeGetTheMeshFrom.GetComponent<MeshFilter>().sharedMesh;

        _batch = _brg.AddBatch(mesh, 0, _material, 0, castShadows: ShadowCastingMode.On,
                   receiveShadows: true, invertCulling: false,
                   bounds: new Bounds(Vector3.one * 10, Vector3.one * 20),
                   instanceCount: 1, customProps: null,
                   associatedSceneObject: null);
    }

    void OnDisable()
    {
        _brg.Dispose();
    }

    void Update()
    {
        // Spin the cube. Proves that the batch is reading the matrices just fine
        var transforms = _brg.GetBatchMatrices(_batch);
        transforms[0] = Matrix4x4.Rotate(Quaternion.Euler(Mathf.Sin(Time.time) * 180, Mathf.Sin(Time.time) * 180, Mathf.Cos(Time.time) * 180));

        // I thought this would set the color in the shader but it isn't working for me
        var instVals = _brg.GetBatchVectorArray(_batch, "_Color");
        instVals[0] = new Vector4(0, 1, 0, 1);
    }

    // You can safely ignore this. It is just a pass-throught that performs no culling
    JobHandle CullingCallback(BatchRendererGroup rendererGroup, BatchCullingContext cullingContext)
    {
        // dummy culling
        for (var batchIndex = 0; batchIndex < cullingContext.batchVisibility.Length; ++batchIndex)
        {
            var batchVisibility = cullingContext.batchVisibility[batchIndex];

            for (var i = 0; i < batchVisibility.instancesCount; ++i)
            {
                cullingContext.visibleIndices[batchVisibility.offset + i] = i;
            }

            batchVisibility.visibleCount = batchVisibility.instancesCount;
            cullingContext.batchVisibility[batchIndex] = batchVisibility;
        }
        return default;
    }
}
