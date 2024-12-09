using UnityEngine;

public class RiverCleanup : MonoBehaviour
{
    public Material dirtyMaterial;  // Material for the dirty water
    public Material cleanMaterial;  // Material for the clean water
    public MeshRenderer riverRenderer;  // Assign the MeshRenderer of the river object
    [Range(0f, 1f)] private float cleanProgress = 0f; // 0 = dirty, 1 = clean

    private PlayerCollector playerBucket;

    void OnEnable()
    {
        PlayerCollector.OnPlayerCollectorCreated += AssignPlayerBucket;
    }

    void OnDisable()
    {
        PlayerCollector.OnPlayerCollectorCreated -= AssignPlayerBucket;
    }

    void AssignPlayerBucket(PlayerCollector bucket)
    {
        playerBucket = bucket;
    }

    void Update()
    {
        if (playerBucket != null)
        {
            // Dynamically calculate clean progress
            cleanProgress = Mathf.Clamp01((float)playerBucket.wasteCollected / 9f); // Assuming max waste is 9
        }

        // Blend between dirty and clean materials
        BlendMaterials(cleanProgress);
    }

    private void BlendMaterials(float blendFactor)
    {
        // Create a blended material dynamically
        Material blendedMaterial = new Material(dirtyMaterial);

        // Lerp shader properties between dirty and clean materials
        blendedMaterial.Lerp(dirtyMaterial, cleanMaterial, blendFactor);

        // Assign the blended material to the river renderer
        riverRenderer.material = blendedMaterial;
    }
}
