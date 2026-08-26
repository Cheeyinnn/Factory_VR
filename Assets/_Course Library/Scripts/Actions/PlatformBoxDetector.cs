using UnityEngine;
using System.Collections.Generic;

public class PlatformBoxDetector : MonoBehaviour
{
    public ProgressTracker progressTracker;

    private HashSet<GameObject> countedBoxes = new HashSet<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;

        if (rb == null)
            return;

        GameObject box = rb.gameObject;

        // Only count objects with the Box tag
        if (!box.CompareTag("Box"))
            return;

        // Prevent the same box from being counted twice
        if (countedBoxes.Contains(box))
            return;

        countedBoxes.Add(box);

        if (progressTracker != null)
        {
            progressTracker.BoxCompleted();
            Debug.Log("Box counted: " + box.name);
        }
    }

    // Called when the Reset Button is pressed
    public void ResetDetector()
    {
        countedBoxes.Clear();

        Debug.Log("Platform box detector reset.");
    }
}