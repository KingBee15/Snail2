using UnityEngine; // Required for Unity's core components like MonoBehaviour and Transform.

public class MouseLook : MonoBehaviour
{
    #region Serialized Fields
    [Header("Look Settings")]
    
    [Tooltip("How fast the camera rotates when you move the mouse. Higher means more sensitive.")]
    [SerializeField] private float mouseSensitivity = 100f; 

    [Tooltip("Drag the main Player object (the capsule/cube) here. The camera will rotate this body left and right.")]
    [SerializeField] private Transform playerBody; 
    #endregion

    #region Private Variables
    // Keeps track of our current up/down rotation so we can clamp it (preventing the player from doing backflips with their neck).
    private float xRotation = 0f; 
    #endregion

    #region Unity Lifecycle Methods
    private void Start()
    {
        // Locks the mouse cursor to the center of the screen and hides it. 
        // (Press 'Escape' in the Unity editor to get your mouse back while playing!)
        Cursor.lockState = CursorLockMode.Locked; 
    }

    private void Update()
    {
        ProcessMouseLook(); // Calls our custom method to handle camera rotation every frame.
    }
    #endregion

    #region Custom Methods
    private void ProcessMouseLook()
    {
        // Get the mouse movement for this frame. 
        // We multiply by mouseSensitivity and Time.deltaTime to ensure rotation speed is tied to time, not framerate.
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Subtract the mouseY movement from our xRotation. 
        // (If we added it, pushing the mouse forward would make you look down, like airplane controls).
        xRotation -= mouseY;

        // Clamps the up/down rotation between -90 degrees (straight up) and 90 degrees (straight down).
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Applies the up/down rotation to the Camera (the object this script is attached to).
        // Quaternion.Euler translates standard degree angles (X, Y, Z) into Unity's rotation system.
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Applies the left/right rotation to the Player's body, so you actually face the direction you look.
        playerBody.Rotate(Vector3.up * mouseX);
    }
    #endregion
}