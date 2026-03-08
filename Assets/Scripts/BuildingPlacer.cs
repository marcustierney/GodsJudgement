using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingPlacer : MonoBehaviour
{
    public static BuildingPlacer Instance;
    public Material validPlacementMaterial;
    public Material invalidPlacementMaterial;
    private GameObject ghostObject;
    private GameObject prefabToPlace;
    private float buildingCost;
    private string buildingType;
    private bool isPlacing = false;
    private Camera mainCam;
    private LayerMask groundLayer;
    public AudioClip farmPlaceSound;
    public AudioClip turretPlaceSound;
    public AudioClip lumbermillPlaceSound;
    public AudioClip barracksPlaceSound;
    public AudioClip wallPlaceSound;
    private AudioSource audioSource;

    void Awake()
    {
        Instance = this;
        mainCam = Camera.main;
        groundLayer = LayerMask.GetMask("Ground");
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isPlacing)
        {
            return;
        }

        UpdateGhostPosition();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryPlace();
        }
        if (Mouse.current.rightButton.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CancelPlacement();
        }
    }

    public void StartPlacing(GameObject prefab, float cost, string type)
    {
        if (isPlacing)
        {
            CancelPlacement();
        }

        if (!ResourceManager.Instance.HasWood(cost))
        {
            Debug.Log("Not enough wood");
            return;
        }
        prefabToPlace = prefab;
        buildingCost = cost;
        buildingType = type;
        isPlacing = true;
        ghostObject = Instantiate(prefab);
        ghostObject.name = "GhostPreview";
        DisableGhostComponents(ghostObject);
        SetGhostMaterial(validPlacementMaterial);
    }

    void UpdateGhostPosition()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = mainCam.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, 200f, groundLayer))
        {
            Vector3 snapped = SnapToGrid(hit.point);
            ghostObject.transform.position = snapped;
            bool valid = IsValidPlacement(snapped);
            if (valid)
            {
                SetGhostMaterial(validPlacementMaterial);
            }
            else
            {
                SetGhostMaterial(invalidPlacementMaterial);
            }
        }
    }

    void TryPlace()
    {
        Vector3 pos = ghostObject.transform.position;
        if (!IsValidPlacement(pos))
        {
            Debug.Log("Invalid placement position");
            return;
        }

        if (!ResourceManager.Instance.SpendWood(buildingCost))
        {
            Debug.Log("Not enough wood");
            CancelPlacement();
            return;
        }
        Instantiate(prefabToPlace, pos, Quaternion.identity);
        PlayPlacementSound();
        CancelPlacement();
    }

    void PlayPlacementSound()
    {
        AudioClip clip;
        switch (buildingType)
        {
            case "farm":
                clip = farmPlaceSound;
                break;

            case "turret":
                clip = turretPlaceSound;
                break;

            case "lumbermill":
                clip = lumbermillPlaceSound;
                break;

            case "barracks":
                clip = barracksPlaceSound;
                break;

            case "wall":
                clip = wallPlaceSound;
                break;

            default:
                clip = null;
                break;
        }
        audioSource.PlayOneShot(clip);
    }

    void CancelPlacement()
    {
        isPlacing = false;
        if (ghostObject != null)
        {
            Destroy(ghostObject);
        }
        ghostObject = null;
        prefabToPlace = null;
    }

    bool IsValidPlacement(Vector3 pos)
    {
        Vector3 halfExtents;
        if (buildingType == "farm")
        {
            halfExtents = new Vector3(2f, 0.5f, 2f);
        }
        else if (buildingType == "lumbermill")
        {
            halfExtents = new Vector3(1.5f, 0.5f, 1f);
        }
        else if (buildingType == "barracks")
        {
            halfExtents = new Vector3(1f, 1.5f, 1f);
        }
        else if (buildingType == "wall")
        {
            halfExtents = new Vector3(0.5f, 2.5f, 1.5f);
        }
        else
        {
            halfExtents = new Vector3(0.5f, 1f, 0.5f);
        }
        Collider[] hits = Physics.OverlapBox(pos, halfExtents, Quaternion.identity);
        foreach (var hit in hits)
        {
            if (hit.gameObject == ghostObject)
            {
                continue;
            }
            if (hit.CompareTag("Building") || hit.CompareTag("TownHall") || hit.CompareTag("Tree"))
            {
                return false;
            }
        }
        return true;
    }

    Vector3 SnapToGrid(Vector3 worldPos)
    {
        float gridSize = 1f;
        return new Vector3(Mathf.Round(worldPos.x / gridSize) * gridSize, 1f, Mathf.Round(worldPos.z / gridSize) * gridSize);
    }

    void DisableGhostComponents(GameObject ghost)
    {
        foreach (var col in ghost.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }
        foreach (var rb in ghost.GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = true;
        }
        foreach (var script in ghost.GetComponentsInChildren<MonoBehaviour>())
        {
            script.enabled = false;
        }
    }

    void SetGhostMaterial(Material mat)
    {
        if (mat == null)
        {
            return;
        }
        foreach (var renderer in ghostObject.GetComponentsInChildren<Renderer>())
        {
            renderer.material = mat;
        }
    }

    public bool IsPlacing()
    {
        return isPlacing;
    }
}