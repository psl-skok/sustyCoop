using UnityEngine;
using System.Collections;

public class AN_Button : MonoBehaviour
{
    public GameObject[] fireworksObjects; // Fireworks objects to toggle
    public Fireworks fireworksScript; // Fireworks control script
    public WagonStorage wagonStorage; // Reference to the generator state
    private Animator anim; // Animator for the button
    public KeyCode buttonKey = KeyCode.E; // Keybinding for interaction
    private bool areFireworksActive = false; // Tracks whether fireworks are active
    private bool skyboxChanged = false; // Tracks whether the skybox has been changed

    public Material newSkybox; // The new skybox material to transition to
    public float fadeDuration = 2f; // Duration of the fade between skyboxes

    private Light sunLight; // Reference to the Directional Light (sun)

    [SerializeField] private GameObject wagonWheelSuccessCanvas; // Reference to the wagon wheel success canvas
    [SerializeField] private Transform playerTransform; // Reference to the player
    [SerializeField] private float interactionRange = 5f; // Distance within which the player can interact with the button
    [SerializeField] private GameObject farmer; // Reference to the farmer character
    [SerializeField] private GameObject fireworksCelebrationText; // Reference to the text to press the button

    void Start()
    {
        anim = GetComponent<Animator>();

        if (anim == null)
        {
            Debug.LogError("Animator component not found on the button.");
        }

        gameObject.SetActive(false); // Button is initially hidden

        if (wagonStorage != null)
        {
            wagonStorage.OnGeneratorStateChanged += HandleGeneratorStateChanged;
        }
        else
        {
            Debug.LogError("WagonStorage reference not assigned.");
        }

        if (fireworksObjects != null)
        {
            foreach (GameObject firework in fireworksObjects)
            {
                firework.SetActive(false); // Ensure fireworks are initially off
            }
        }
        else
        {
            Debug.LogError("No fireworks objects assigned.");
        }

        // Find and assign the directional light (sun)
        GameObject sunObject = GameObject.Find("Directional Light");
        if (sunObject != null)
        {
            sunLight = sunObject.GetComponent<Light>();
        }

        if (sunLight == null)
        {
            Debug.LogError("Directional Light not found or does not have a Light component.");
        }

        // Hide the success canvas initially
        if (wagonWheelSuccessCanvas != null)
        {
            wagonWheelSuccessCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError("WagonWheelSuccessCanvas reference is not assigned in the Inspector.");
        }

        // Hide the farmer initially
        if (farmer != null)
        {
            farmer.SetActive(false);
        }
        else
        {
            Debug.LogError("Farmer reference is not assigned in the Inspector.");
        }
    }

    void OnDestroy()
    {
        if (wagonStorage != null)
        {
            wagonStorage.OnGeneratorStateChanged -= HandleGeneratorStateChanged;
        }
    }

    void Update()
    {
        if (IsPlayerInRange() && Input.GetKeyDown(buttonKey))
        {
            ToggleFireworks();
        }
    }

    private bool IsPlayerInRange()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned.");
            return false;
        }

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        return distance <= interactionRange;
    }

    private void HandleGeneratorStateChanged(bool isActive)
    {
        gameObject.SetActive(isActive); // Show or hide the button based on generator state
        if (isActive)
        {
            Debug.Log("Generator is active. Showing success canvas and farmer.");
            if (wagonWheelSuccessCanvas != null)
            {
                wagonWheelSuccessCanvas.SetActive(true); // Show the success canvas when the generator is active
            }

            // Show the farmer
            if (farmer != null)
            {
                farmer.SetActive(true);
            }
        }
        else
        {
            Debug.Log("Generator is inactive. Hiding success canvas and farmer.");
            if (wagonWheelSuccessCanvas != null)
            {
                wagonWheelSuccessCanvas.SetActive(false); // Hide the success canvas when the generator is inactive
            }

            // Hide the farmer
            if (farmer != null)
            {
                farmer.SetActive(false);
            }
        }
    }

    private void ToggleFireworks()
    {
        areFireworksActive = !areFireworksActive; // Toggle the state
        PlayButtonAnimation();

        if (areFireworksActive)
        {
            StartFireworks();

            if (!skyboxChanged)
            {
                skyboxChanged = true; // Ensure the skybox only changes once
                StartCoroutine(FadeSkybox(RenderSettings.skybox, newSkybox, sunLight)); // Pass sunLight here
            }
        }
        else
        {
            StopFireworks();
        }
    }

    private void PlayButtonAnimation()
    {
        if (anim != null)
        {
            anim.SetTrigger("ButtonPress"); // Trigger button press animation
            Debug.Log("Button animation played.");
        }
        else
        {
            Debug.LogError("Animator not assigned or missing.");
        }
    }

    private void StartFireworks()
    {
        Debug.Log("Starting fireworks!");

        if (fireworksScript != null)
        {
            fireworksScript.StartFireworks();
            fireworksCelebrationText.SetActive(false);
        }

        foreach (GameObject firework in fireworksObjects)
        {
            firework.SetActive(true); // Activate fireworks objects
        }
    }

    private void StopFireworks()
    {
        Debug.Log("Stopping fireworks!");

        foreach (GameObject firework in fireworksObjects)
        {
            firework.SetActive(false); // Deactivate fireworks objects
        }
    }

    private IEnumerator FadeSkybox(Material currentSkybox, Material targetSkybox, Light sunLight)
    {
        float elapsedTime = 0f;

        // Use a temporary material to modify the properties of the current skybox
        Material tempSkybox = new Material(currentSkybox);
        RenderSettings.skybox = tempSkybox;

        // Cache original settings
        Color originalTint = currentSkybox.HasProperty("_Tint") ? currentSkybox.GetColor("_Tint") : Color.white;
        Color targetTint = targetSkybox.HasProperty("_Tint") ? new Color(0.1f, 0.12f, 0.15f, 1f) : new Color(0.1f, 0.12f, 0.15f, 1f); // Darker gray-blue

        float originalExposure = currentSkybox.HasProperty("_Exposure") ? currentSkybox.GetFloat("_Exposure") : 1f;
        float targetExposure = targetSkybox.HasProperty("_Exposure") ? Mathf.Max(0.5f, targetSkybox.GetFloat("_Exposure")) : 0.5f; // Slightly darker exposure

        float initialSunIntensity = sunLight != null ? sunLight.intensity : 0f;

        // Optional: Adjust ambient lighting
        Color originalAmbient = RenderSettings.ambientLight;
        Color targetAmbient = new Color(0.1f, 0.12f, 0.15f); // Darker gray-blue for ambient light

        float opacityFadeStart = fadeDuration * 0.3f; // Delay opacity fading until 30% through the transition

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float lerpFactor = elapsedTime / fadeDuration;

            // Faster darkening: Use an exponential curve to darken more quickly at the start
            float adjustedFactor = Mathf.Pow(lerpFactor, 0.5f); // Square root for faster initial transition

            // Smoothly transition exposure with a faster drop initially
            float blendedExposure = Mathf.Lerp(originalExposure, targetExposure, adjustedFactor);
            tempSkybox.SetFloat("_Exposure", blendedExposure);

            // Ensure tint transitions to darker gray-blue
            Color blendedTint = Color.Lerp(originalTint, targetTint, adjustedFactor);
            if (elapsedTime > opacityFadeStart)
            {
                // Gradually fade opacity slower than the color fade
                float opacityFactor = (elapsedTime - opacityFadeStart) / (fadeDuration - opacityFadeStart);
                blendedTint.a = Mathf.Lerp(1f, 0f, opacityFactor); // Fade alpha channel
            }
            tempSkybox.SetColor("_Tint", blendedTint);

            // Transition ambient light to darker gray-blue
            RenderSettings.ambientLight = Color.Lerp(originalAmbient, targetAmbient, adjustedFactor);

            // Gradually reduce the sun intensity to 0
            if (sunLight != null)
            {
                sunLight.intensity = Mathf.Lerp(initialSunIntensity, 0f, adjustedFactor);
            }

            yield return null;
        }

        // Set final skybox and lighting
        RenderSettings.skybox = targetSkybox;
        targetSkybox.SetFloat("_Exposure", targetExposure);
        RenderSettings.ambientLight = targetAmbient;

        // Ensure the sun is completely off
        if (sunLight != null)
        {
            sunLight.intensity = 0f;
            sunLight.enabled = false;
        }

        DynamicGI.UpdateEnvironment(); // Update the lighting
    }
}
