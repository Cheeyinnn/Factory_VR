using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MovementVignette : MonoBehaviour
{
    [Header("References")]
    public Volume globalVolume;

    // For XR Device Simulator WASD movement.
    public Transform movementTarget;

    [Header("Vignette Settings")]
    public bool vignetteEnabled = true;

    [Range(0f, 1f)]
    public float movingIntensity = 0.45f;

    [Range(0f, 1f)]
    public float idleIntensity = 0f;

    public float transitionSpeed = 6f;

    [Header("Movement Detection")]
    public float movementThreshold = 0.1f;

    [Header("Snap Turn / Teleport Detection")]
    public float rotationThreshold = 10f;

    // How long vignette remains visible after a snap turn / teleport.
    public float motionHoldTime = 0.35f;

    private Vignette vignette;

    private Vector3 lastMovementTargetPosition;

    // XR Rig position/rotation
    private Vector3 lastRigPosition;
    private float lastRigYaw;

    private float comfortMotionTimer = 0f;

    void Start()
    {
        if (globalVolume != null &&
            globalVolume.profile.TryGet(out vignette))
        {
            vignette.intensity.overrideState = true;
            vignette.intensity.value = idleIntensity;
        }

        if (movementTarget != null)
        {
            lastMovementTargetPosition = movementTarget.position;
        }

        lastRigPosition = transform.position;
        lastRigYaw = transform.eulerAngles.y;
    }

    void Update()
    {
        if (vignette == null)
            return;

        bool continuousMovement = false;

        // ------------------------------------------------
        // 1. Simulator / continuous movement detection
        // ------------------------------------------------
        if (movementTarget != null)
        {
            Vector3 currentPosition = movementTarget.position;

            Vector3 movement =
                currentPosition - lastMovementTargetPosition;

            movement.y = 0f;

            float speed =
                movement.magnitude /
                Mathf.Max(Time.deltaTime, 0.0001f);

            continuousMovement =
                speed > movementThreshold;

            lastMovementTargetPosition =
                currentPosition;
        }

        // ------------------------------------------------
        // 2. XR Rig rotation detection
        //    Detects Snap Turning
        // ------------------------------------------------
        float currentRigYaw =
            transform.eulerAngles.y;

        float rotationDelta =
            Mathf.Abs(
                Mathf.DeltaAngle(
                    lastRigYaw,
                    currentRigYaw
                )
            );

        bool snapTurnDetected =
            rotationDelta > rotationThreshold;

        // ------------------------------------------------
        // 3. XR Rig position detection
        //    Detects Teleportation
        // ------------------------------------------------
        Vector3 rigMovement =
            transform.position - lastRigPosition;

        rigMovement.y = 0f;

        bool teleportDetected =
            rigMovement.magnitude > 0.15f;

        // ------------------------------------------------
        // 4. Start / refresh vignette timer
        // ------------------------------------------------
        if (continuousMovement ||
            snapTurnDetected ||
            teleportDetected)
        {
            comfortMotionTimer = motionHoldTime;
        }
        else
        {
            comfortMotionTimer -= Time.deltaTime;
        }

        bool comfortMotion =
            comfortMotionTimer > 0f;

        // ------------------------------------------------
        // 5. Apply vignette
        // ------------------------------------------------
        float targetIntensity =
            vignetteEnabled && comfortMotion
                ? movingIntensity
                : idleIntensity;

        vignette.intensity.value =
            Mathf.Lerp(
                vignette.intensity.value,
                targetIntensity,
                transitionSpeed * Time.deltaTime
            );

        lastRigPosition = transform.position;
        lastRigYaw = currentRigYaw;
    }

    public void SetVignetteEnabled(bool enabled)
    {
        vignetteEnabled = enabled;

        if (!enabled && vignette != null)
        {
            comfortMotionTimer = 0f;
            vignette.intensity.value = idleIntensity;
        }
    }
}