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

        if (!box.CompareTag("Box"))
            return;

        if (countedBoxes.Contains(box))
            return;

        countedBoxes.Add(box);

        if (progressTracker != null)
        {
            progressTracker.BoxCompleted();
            Debug.Log("Box counted: " + box.name);
        }
    }
}