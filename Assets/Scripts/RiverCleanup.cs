using UnityEngine;

public class RiverCleanup : MonoBehaviour
{
    public Material waterMaterial; // Assign your water material here
    public Color dirtyColor = new Color(0.4f, 0.3f, 0.2f); // Brownish color
    public Color cleanColor = new Color(0.1f, 0.6f, 0.9f); // Bluish color
    [Range(0f, 1f)] public float cleanProgress = 0f; // 0 = dirty, 1 = clean

    private PlayerBucket playerBucket;

    void OnEnable()
    {
        PlayerBucket.OnPlayerBucketCreated += AssignPlayerBucket;
    }

    void OnDisable()
    {
        PlayerBucket.OnPlayerBucketCreated -= AssignPlayerBucket;
    }

    void AssignPlayerBucket(PlayerBucket bucket)
    {
        playerBucket = bucket;
    } 

    void Update()
    {
        if (playerBucket != null)
        {
            // Dynamically calculate clean progress
            cleanProgress = (float)playerBucket.wasteCollected / 9; // Assuming max waste is 9
        }

        // Blend between dirty and clean colors
        Color currentColor = Color.Lerp(dirtyColor, cleanColor, cleanProgress);
        waterMaterial.color = currentColor;
    }
}
