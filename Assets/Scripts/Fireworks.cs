using UnityEngine;

public class Fireworks : MonoBehaviour
{
    public GameObject[] fireworksEffects;

    public void StartFireworks()
    {
        foreach (GameObject firework in fireworksEffects)
        {
            if (firework != null)
            {
                Instantiate(firework, firework.transform.position, firework.transform.rotation);
            }
        }
    }
}
