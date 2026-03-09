using UnityEngine; // Required for Unity's core components like MonoBehaviour, Transform, and Vector3.

public class SnailMovement : MonoBehaviour // Inherits from MonoBehaviour so it can be attached to the Snail.
{
    #region Serialized Fields
    [Header("Targeting")] // Groups the targeting options in the Inspector.
    
    [Tooltip("Drag your Player object into this slot so the snail knows who to chase.")] // Explains the variable in the Inspector.
    [SerializeField] private Transform playerTarget; // The target the snail will move towards.

    [Header("Movement Settings")] // Groups the movement options in the Inspector.
    
    [Tooltip("How fast the snail moves. Keep it incredibly low for maximum creeping dread!")] // Explains the speed variable.
    [SerializeField] private float crawlSpeed = 0.5f; // The speed at which the snail moves (units per second).
    #endregion

    #region Unity Lifecycle Methods
    private void Update() // Called once per frame. Perfect for continuous movement.
    {
        ChasePlayer(); // Calls our custom method to handle the chasing logic.
    }
    #endregion

    #region Custom Methods
    private void ChasePlayer() // Defines the logic for moving toward the player.
    {
        // 1. SAFETY CHECK
        // If the playerTarget slot is empty, we "return" (stop running this method) to prevent the game from crashing with a NullReferenceException.
        if (playerTarget == null) return; 

        // 2. FIND THE TARGET DESTINATION
        // We create a new Vector3 using the player's X and Z coordinates. 
        // We strictly use the Snail's CURRENT Y position so it doesn't try to fly up to the player's eyes or sink into the floor.
        Vector3 targetPosition = new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z);

        // 3. ROTATE TO FACE THE PLAYER
        // Makes the snail's "forward" direction point directly at the calculated targetPosition.
        transform.LookAt(targetPosition);

        // 4. MOVE FORWARD
        // Vector3.MoveTowards smoothly transitions from the current position to the target position.
        // Multiplying by Time.deltaTime ensures the speed is based on real-world seconds, not your computer's framerate.
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, crawlSpeed * Time.deltaTime);
    }
    #endregion
}

/*
========================================================================================================
DETAILED IMPLEMENTATION INSTRUCTIONS
========================================================================================================

STEP 1: ATTACH THE SCRIPT
1. Save this script and go back to the Unity Editor.
2. Click on your "Snail" object in the Hierarchy.
3. Drag the "SnailMovement" script from the Project window and drop it onto the Snail in the Inspector.

STEP 2: ASSIGN THE PLAYER TARGET
1. With the Snail still selected, look at the SnailMovement script in the Inspector.
2. You will see an empty slot called "Player Target".
3. Drag your "Player" object directly from the Hierarchy into this "Player Target" slot. 

STEP 3: TEST THE TERROR
1. Press the Play button at the top of Unity.
2. Click inside the Game window to lock your mouse.
3. Turn around and watch the snail. It should slowly rotate to face you and begin its agonizingly slow slide in your direction.
4. Try sprinting away—the snail will relentlessly update its path to follow you!

========================================================================================================
A QUICK SUGGESTION FOR LATER
========================================================================================================
Right now, the snail moves in a perfectly straight line, completely ignoring the walls (meaning it could slide right through them if you stood outside the box). When you eventually add obstacles like tables or couches, you will want to switch this script over to Unity's "NavMesh" system. NavMesh acts like a GPS for AI, allowing it to walk around obstacles instead of through them. For an empty box room, however, this straight-line math works perfectly!
*/