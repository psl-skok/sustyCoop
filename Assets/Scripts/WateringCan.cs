using UnityEngine;
public class WateringCan : MonoBehaviour
{
    public GameObject waterPlane; // Reference to the water plane
    public GameObject wateringCan;
    public bool isFilled = false; // Tracks if the can is filled
    public AudioSource fillBucketSound;
    public AudioSource pourBucketSound;
    void Start()
    {
        if (waterPlane != null)
        {
            waterPlane.SetActive(false); // Ensure the water plane is hidden initially
        }
        else
        {
            Debug.LogWarning("Water plane not assigned to the watering can.");
        }
    }
    public void FillWithWater()
    {
        if (isFilled)
        {
            Debug.Log("The watering can is already filled!");
            return;
        }
        isFilled = true;
        if (waterPlane != null)
        {
            waterPlane.SetActive(true); // Show the water plane
            BucketGatherSound();
        }
    }
    public void PourCan()
    {
        if(isFilled)
        {
            Animator bucketAnimator = wateringCan.GetComponent<Animator>();
            if (bucketAnimator != null)
            {
                bucketAnimator.SetTrigger("Pour"); // Trigger the pour animation
            }
            BucketPourSound();
        }
    }
    void BucketGatherSound()
    {
        if (fillBucketSound != null)
        {
            fillBucketSound.Play(); // Play the audio
        }
    }
    void BucketPourSound()
    {
        if (pourBucketSound != null)
        {
            pourBucketSound.Play(); // Play the audio
        }
    }
}