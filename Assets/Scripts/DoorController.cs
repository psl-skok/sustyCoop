using UnityEngine;
using UnityEngine.UI;  // Add this to use the UI elements
using TMPro;

public class DoorController : MonoBehaviour
{
    public enum DoorState
    {
        Open,
        Closed
    }

    public DoorState InitialState = DoorState.Closed;
    public float AnimationSpeed = 1;
    public float openDistance = 3f;

    [SerializeField]
    private AnimationClip openAnimation;
    [SerializeField]
    private AnimationClip closeAnimation;

    private Animation animator;
    private DoorState currentState;
    private Collider doorCollider;

    private CharacterSwap characterSwap;  // Reference to the CharacterSwap script
    private Transform player;  // Reference to the active player's transform
    private bool isPlayerNear = false;

    // Reference to the UI Text for door interaction
    public TextMeshProUGUI interactionText;

    void Awake()
    {
        animator = GetComponent<Animation>();
        doorCollider = GetComponent<Collider>();

        animator.playAutomatically = false;
        openAnimation.legacy = true;
        closeAnimation.legacy = true;
        animator.AddClip(openAnimation, DoorState.Open.ToString());
        animator.AddClip(closeAnimation, DoorState.Closed.ToString());
    }

    void Start()
    {
        // Find the CharacterSwap script in the scene
        characterSwap = FindObjectOfType<CharacterSwap>();

        // Set the initial state
        currentState = InitialState;
        var clip = GetCurrentAnimation();
        animator[clip].speed = 9999;
        animator.Play(clip);
        UpdateColliderState();

        // Hide the interaction text at the start
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Get the current active player from CharacterSwap
        if (characterSwap != null)
        {
            player = characterSwap.GetActiveCharacter().transform;
        }

        if (player != null)
        {
            CheckPlayerDistance();

            if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
            {
                ToggleDoor();
            }
        }
    }

    void CheckPlayerDistance()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= openDistance)
        {
            isPlayerNear = true;

            // Show the interaction text
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(true);
            }
        }
        else
        {
            isPlayerNear = false;

            // Hide the interaction text when the player is out of range
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }
        }
    }

    public void ToggleDoor()
    {
        if (IsDoorOpen)
            CloseDoor();
        else
            OpenDoor();
    }

    public bool IsDoorOpen { get { return currentState == DoorState.Open; } }

    private void OpenDoor()
    {
        currentState = DoorState.Open;
        Animate();
    }

    private void CloseDoor()
    {
        currentState = DoorState.Closed;
        Animate();
    }

    private void Animate()
    {
        var clip = GetCurrentAnimation();
        animator[clip].speed = AnimationSpeed;
        animator.Play(clip);
        UpdateColliderState();
    }

    private string GetCurrentAnimation()
    {
        return currentState.ToString();
    }

    private void UpdateColliderState()
    {
        doorCollider.enabled = currentState == DoorState.Closed;
    }
}
