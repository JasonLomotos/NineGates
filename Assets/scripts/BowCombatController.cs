using UnityEngine;
using System.Linq;

/// <summary>
/// Manages the bow and arrow combat for the player.
/// </summary>
public class BowCombatController : MonoBehaviour
{
    [Header("Components")]
    [Tooltip("Animator used by the character.")]
    [SerializeField] private Animator animator;

    [Header("Combat Settings")]
    [Tooltip("The arrow prefab to be fired.")]
    [SerializeField] private GameObject arrowPrefab;
    [Tooltip("The point where the arrow will be spawned (e.g., the character's hand).")]
    [SerializeField] private Transform arrowSpawnPoint;
    [Tooltip("The radius to detect enemies within.")]
    [SerializeField] private float detectionRadius = 5f;
    [Tooltip("The layer your enemies are on.")]
    [SerializeField] private LayerMask enemyLayer;

    private bool isAiming = false;
    private Transform aimTarget = null;

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // When the Left Mouse Button is pressed down
        if (Input.GetMouseButtonDown(0))
        {
            isAiming = true;
            animator.SetTrigger("drawBow");
            FindNearestEnemy();
        }

        // When the Left Mouse Button is released
        if (Input.GetMouseButtonUp(0))
        {
            if (isAiming)
            {
                animator.SetTrigger("fireBow");
                isAiming = false;
                // The arrow is fired via an Animation Event, not here directly.
            }
        }
    }

    private void FindNearestEnemy()
    {
        // Find all colliders on the enemy layer within the detection radius
        Collider[] enemies = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);

        // Find the closest one
        aimTarget = enemies.OrderBy(enemy => Vector3.Distance(transform.position, enemy.transform.position))
                           .Select(enemy => enemy.transform)
                           .FirstOrDefault(); // This will be null if no enemies are found

        if (aimTarget != null)
        {
            Debug.Log("Found nearest enemy: " + aimTarget.name);
        }
        else
        {
            Debug.Log("No enemies nearby, will fire straight.");
        }
    }

    /// <summary>
    /// This method is called by an Animation Event on the "Fire" animation clip.
    /// </summary>
    public void FireArrow()
    {

        if (arrowSpawnPoint == null)
        {
            Debug.LogError("Spawn Point is not set!");
            return;
        }

        if (arrowPrefab == null)
        {
            Debug.LogError("Arrow Prefab is not set!");
            return;
        }

        // Instantiate the arrow at the spawn point
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);

        // Aim the arrow
        if (aimTarget != null)
        {
            // Aim at the center of the target enemy
            Vector3 targetCenter = aimTarget.GetComponent<Collider>().bounds.center;
            arrow.transform.LookAt(targetCenter);
        }
        else
        {
            // Aim straight forward from the camera's perspective
            Vector3 aimDirection = Camera.main.transform.forward;
            aimDirection.y = 0; // Optional: Keep the arrow level
            arrow.transform.rotation = Quaternion.LookRotation(aimDirection);
        }
        
        // Reset aim target after firing
        aimTarget = null;
    }

    // Optional: Draw a sphere in the editor to visualize the detection radius
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}