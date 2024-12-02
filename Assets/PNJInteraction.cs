using UnityEngine;

public class PNJInteractionWithCharacterController : MonoBehaviour
{
    public LayerMask pnjLayer;
    public float detectionRadius = 2f;
    private DialogueSystem detectedPNJ;
    private BasicController playerController;
    private bool isDialogueActive = false;

    void Start()
    {
        playerController = GetComponent<BasicController>();
    }

    void Update()
    {
        if (isDialogueActive)
        {
            if (Input.GetKeyDown(KeyCode.Q) && detectedPNJ != null)
            {
                detectedPNJ.ContinueDialogue();
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, pnjLayer);

            if (colliders.Length > 0)
            {
                detectedPNJ = colliders[0].GetComponent<DialogueSystem>();

                if (detectedPNJ != null)
                {
                    isDialogueActive = true;
                    playerController.canMove = false;
                    detectedPNJ.StartDialogue();
                }
                else
                {
                    Debug.LogWarning("Le PNJ détecté n'a pas de DialogueSystem.");
                }
            }
            else
            {
                Debug.Log("Aucun PNJ à proximité.");
            }
        }
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        playerController.canMove = true;
        detectedPNJ = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
