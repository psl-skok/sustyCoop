using UnityEngine;

public class ScrollSpeed : MonoBehaviour
{
    public float speed;
    public bool isActive = false; // Flag to control when the animation starts

    void Update()
    {
        if (isActive && !GetComponent<Animation>().isPlaying)
        {
            // Start the animation when isActive is true and it's not already playing
            GetComponent<Animation>()["Run"].speed = speed;
            GetComponent<Animation>().Play("Run");
            Debug.Log("Generator animation started.");
        }
    }
}
