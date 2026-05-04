using UnityEngine;
using System;

[DisallowMultipleComponent]
public class PlayerStats : MonoBehaviour
{
    [Header("Movement")]
    public float Speed;
    public float RunSpeed;
    public float Stamina;
    public float JumpStrength;
    public float CrouchSpeed;

    [Header("Limits")]
    public float MaxSpeed;
    public float MaxRunSpeed;
    public float MaxStamina = 100f;
    public float MaxJumpStrength;
    public float MaxCrouchSpeed;

    public event Action OnStatsChanged;

    void OnValidate()
    {
        // Clamp stamina to valid range.
        Stamina = Mathf.Clamp(Stamina, 0, MaxStamina);
    }
}