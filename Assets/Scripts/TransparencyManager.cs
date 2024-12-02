using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;


public class TransparencyManager : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private Material transparentMaterial;

    private Transform player;
    private List<GameObject> transparentObjects = new List<GameObject>();
    private Dictionary<GameObject, Material[]> replacedMaterials = new Dictionary<GameObject, Material[]>();

    private void Awake() {
        player = GameObject.FindWithTag("Player").transform;
        Assert.IsNotNull(player, "Player not found");
    }

    private void Update() {
        List<GameObject> objectsInTheWay = GetAllObjectsInTheWay();
        objectsInTheWay = objectsInTheWay.Except(transparentObjects).ToList();
        MakeObjectsTransparent(objectsInTheWay);
        MakeObjectsSolid(objectsInTheWay);
    }

    private List<GameObject> GetAllObjectsInTheWay() {
        List<GameObject> objectsInTheWay = new List<GameObject>();
        float playerDistance = Vector3.Distance(player.position, transform.position);

        Ray rayFromCamera = new Ray(transform.position, player.position - transform.position);
        Ray rayFromPlayer = new Ray(player.position, transform.position - player.position);

        var hitsFromCamera = Physics.RaycastAll(rayFromCamera, playerDistance, obstacleLayer);
        var hitsFromPlayer = Physics.RaycastAll(rayFromPlayer, playerDistance, obstacleLayer);

        foreach (var hit in hitsFromCamera) {
            if (!objectsInTheWay.Contains(hit.collider.gameObject)) {
                objectsInTheWay.Add(hit.collider.gameObject);
            }
        }
        foreach (var hit in hitsFromPlayer) {
            if (!objectsInTheWay.Contains(hit.collider.gameObject)) {
                objectsInTheWay.Add(hit.collider.gameObject);
            }
        }
        return objectsInTheWay;
    }

    private void MakeObjectsTransparent(List<GameObject> objects) {
        foreach (var obj in objects) {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null) {
                SetTransparency(renderer);
                transparentObjects.Add(obj);
            }
        }
    }

    private void MakeObjectsSolid(List<GameObject> objectsToExclude) {
        List<GameObject> objectsToReset = new List<GameObject>();
        foreach (var obj in transparentObjects) {
            if (!objectsToExclude.Contains(obj)) {
                objectsToReset.Add(obj);
            }
        }
        foreach (var obj in objectsToReset) {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null) {
                ResetTransparency(renderer);
                transparentObjects.Remove(obj);
            }
        }
    }

    private void SetTransparency(Renderer renderer) {
        replacedMaterials.Add(renderer.gameObject, renderer.materials);
        renderer.material = transparentMaterial;
    }

    private void ResetTransparency(Renderer renderer) {
        Material[] materials;
        if (replacedMaterials.TryGetValue(renderer.gameObject, out materials)) {
            renderer.materials = materials;
            replacedMaterials.Remove(renderer.gameObject);
        }
    }
}
