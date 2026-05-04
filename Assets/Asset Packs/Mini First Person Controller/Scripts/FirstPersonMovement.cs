using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public KeyCode runningKey = KeyCode.LeftShift;

    [Header("Stamina")]
    [Tooltip("Stamina cost per second while sprinting.")]
    public float staminaCostPerSecond = 20f;
    [Tooltip("Stamina recovery per second while not sprinting.")]
    public float staminaRecoveryPerSecond = 15f;

    /// <summary> Reference to PlayerStats. Must be on the same GameObject. </summary>
    public PlayerStats playerStats;

    Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();


    void Awake()
    {
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();
        // Get PlayerStats from the same GameObject.
        if (playerStats == null)
        {
            playerStats = GetComponent<PlayerStats>();
        }
    }

    void FixedUpdate()
    {
        // Update IsRunning from input and stamina availability.
        bool wantToRun = canRun && Input.GetKey(runningKey);
        IsRunning = wantToRun && playerStats.Stamina > 0;

        // Handle stamina consumption and recovery.
        UpdateStamina();

        float targetMovingSpeed = IsRunning ? playerStats.RunSpeed : playerStats.Speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

        // Apply movement.
        rigidbody.linearVelocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.linearVelocity.y, targetVelocity.y);
    }

    void UpdateStamina()
    {
        if (IsRunning)
        {
            // Consume stamina while sprinting.
            playerStats.Stamina -= staminaCostPerSecond * Time.fixedDeltaTime;
            playerStats.Stamina = Mathf.Max(playerStats.Stamina, 0);
        }
        else
        {
            // Recover stamina when not sprinting.
            playerStats.Stamina += staminaRecoveryPerSecond * Time.fixedDeltaTime;
            playerStats.Stamina = Mathf.Min(playerStats.Stamina, 100f); // Assuming max stamina is 100
        }
    }
}