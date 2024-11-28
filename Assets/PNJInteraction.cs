using UnityEngine;

public class PNJInteractionWithCharacterController : MonoBehaviour
{
    public LayerMask pnjLayer; // Layer des PNJs
    public float detectionRadius = 2f; // Rayon de détection autour du joueur
    private DialogueSystem detectedPNJ; // Référence au système de dialogue du PNJ détecté
    private BasicController playerController; // Référence au script de mouvement du joueur
    private bool isDialogueActive = false; // Indique si un dialogue est actif

    void Start()
    {
        playerController = GetComponent<BasicController>(); // Récupère le script de mouvement
    }

    void Update()
    {
        // Empêche le joueur de redétecter un PNJ si un dialogue est en cours
        if (isDialogueActive)
        {
            if (Input.GetKeyDown(KeyCode.Q) && detectedPNJ != null)
            {
                detectedPNJ.ContinueDialogue(); // Continue le dialogue en cours
            }
            return; // Sort de la méthode Update pour empêcher toute autre action
        }

        // Vérifie si le joueur appuie sur Q pour interagir
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, pnjLayer);

            if (colliders.Length > 0)
            {
                // Trouve le DialogueSystem attaché au PNJ détecté
                detectedPNJ = colliders[0].GetComponent<DialogueSystem>();

                if (detectedPNJ != null)
                {
                    Debug.Log("PNJ détecté : " + colliders[0].name);
                    isDialogueActive = true; // Active le mode dialogue
                    playerController.canMove = false; // Désactive le mouvement du joueur
                    detectedPNJ.StartDialogue(); // Démarre le dialogue du PNJ détecté
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

    // Méthode appelée lorsque le dialogue se termine
    public void EndDialogue()
    {
        Debug.Log("Fin du dialogue.");
        isDialogueActive = false; // Désactive le mode dialogue
        playerController.canMove = true; // Réactive le mouvement du joueur
        detectedPNJ = null; // Réinitialise la référence au PNJ
    }

    // Dessine la zone de détection dans l'éditeur pour visualisation
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
