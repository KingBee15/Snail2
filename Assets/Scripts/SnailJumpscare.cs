using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class SnailInYourFace : MonoBehaviour
{
    [Header("Player References")]
    [Tooltip("Drag the Player's Main Camera here.")]
    [SerializeField] private Transform playerCamera;

    [Tooltip("Drag the Player object here to disable their movement.")]
    [SerializeField] private MonoBehaviour playerMovementScript; 
    
    [Tooltip("Drag the Main Camera here to disable MouseLook.")]
    [SerializeField] private MonoBehaviour mouseLookScript;

    [Header("Jumpscare Settings")]
    [Tooltip("How far away the snail teleports so it doesn't swallow the camera.")]
    [SerializeField] private float distanceInFrontOfCamera = 2.5f;

    [Tooltip("How big the snail instantly becomes.")]
    [SerializeField] private Vector3 jumpscareScale = new Vector3(3f, 3f, 3f);

    [Tooltip("The silly/scary sound effect to play.")]
    [SerializeField] private AudioClip jumpscareSound;

    [Tooltip("The UI Canvas that contains your 'Game Over' text.")]
    [SerializeField] private GameObject gameOverUI;

    [Tooltip("How long the snail stares at you before the Game Over screen.")]
    [SerializeField] private float timeBeforeGameOver = 2.0f;

    private AudioSource audioSource;
    private bool hasCaughtPlayer = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); 
        
        if (gameOverUI != null) 
        {
            gameOverUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasCaughtPlayer)
        {
            hasCaughtPlayer = true; 
            StartCoroutine(ExecuteJumpscare()); 
        }
    }

    private IEnumerator ExecuteJumpscare()
    {
        // 1. FREEZE THE PLAYER
        if (playerMovementScript != null) playerMovementScript.enabled = false;
        if (mouseLookScript != null) mouseLookScript.enabled = false;

        Rigidbody playerRb = playerMovementScript.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector3.zero;
        }

        // 2. TURN OFF THE SNAIL'S MOVEMENT
        // This stops the snail from trying to slide back to the floor while it's in your face!
        SnailMovement movementScript = GetComponent<SnailMovement>();
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }

        // 3. TELEPORT AND SCALE THE SNAIL
        Vector3 inFrontOfFace = playerCamera.position + (playerCamera.forward * distanceInFrontOfCamera);
        transform.position = inFrontOfFace;
        transform.LookAt(playerCamera.position);
        transform.localScale = jumpscareScale;

        // 4. PLAY THE SOUND
        if (jumpscareSound != null)
        {
            audioSource.PlayOneShot(jumpscareSound);
        }

        // 5. THE AWKWARD STARE
        yield return new WaitForSeconds(timeBeforeGameOver);

        // 6. SHOW GAME OVER
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true); 
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}