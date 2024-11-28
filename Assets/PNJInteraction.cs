using UnityEngine;

public class PNJInteractionWithCharacterController : MonoBehaviour
{
    public LayerMask pnjLayer; // Layer des PNJs
    public float detectionRadius = 2f; // Rayon de d�tection autour du joueur
    private DialogueSystem detectedPNJ; // R�f�rence au syst�me de dialogue du PNJ d�tect�
    private BasicController playerController; // R�f�rence au script de mouvement du joueur
    private bool isDialogueActive = false; // Indique si un dialogue est actif

    void Start()
    {
        playerController = GetComponent<BasicController>(); // R�cup�re le script de mouvement
    }

    void Update()
    {
        // Emp�che le joueur de red�tecter un PNJ si un dialogue est en cours
        if (isDialogueActive)
        {
            if (Input.GetKeyDown(KeyCode.Q) && detectedPNJ != null)
            {
                detectedPNJ.ContinueDialogue(); // Continue le dialogue en cours
            }
            return; // Sort de la m�thode Update pour emp�cher toute autre action
        }

        // V�rifie si le joueur appuie sur Q pour interagir
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, pnjLayer);

            if (colliders.Length > 0)
            {
                // Trouve le DialogueSystem attach� au PNJ d�tect�
                detectedPNJ = colliders[0].GetComponent<DialogueSystem>();

                if (detectedPNJ != null)
                {
                    Debug.Log("PNJ d�tect� : " + colliders[0].name);
                    isDialogueActive = true; // Active le mode dialogue
                    playerController.canMove = false; // D�sactive le mouvement du joueur
                    detectedPNJ.StartDialogue(); // D�marre le dialogue du PNJ d�tect�
                }
                else
                {
                    Debug.LogWarning("Le PNJ d�tect� n'a pas de DialogueSystem.");
                }
            }
            else
            {
                Debug.Log("Aucun PNJ � proximit�.");
            }
        }
    }

    // M�thode appel�e lorsque le dialogue se termine
    public void EndDialogue()
    {
        Debug.Log("Fin du dialogue.");
        isDialogueActive = false; // D�sactive le mode dialogue
        playerController.canMove = true; // R�active le mouvement du joueur
        detectedPNJ = null; // R�initialise la r�f�rence au PNJ
    }

    // Dessine la zone de d�tection dans l'�diteur pour visualisation
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
