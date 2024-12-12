using UnityEngine;

public class ScrollSpeed : MonoBehaviour
{
    public float speed;
    public bool isActive = false; // Flag to control when the animation starts

    public AudioSource audioSource;

    void Update()
    {
        if (isActive && !GetComponent<Animation>().isPlaying)
        {
            // Start the animation when isActive is true and it's not already playing
            GetComponent<Animation>()["Run"].speed = speed;
            GetComponent<Animation>().Play("Run");
            Debug.Log("Generator animation started.");

            PlaySound();
        }
    }
    void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.Play(); //Plays audio sound
        }
    }
}
