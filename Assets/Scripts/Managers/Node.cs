using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Vector3 positionOffset;

    public GameObject turret;
    private Renderer rend;
    private Color startColor;
    private Collider nodeCollider;

    [Header("Construction Settings")]
    public Vector3 buildOffset;

    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        nodeCollider = GetComponent<Collider>();
        if (nodeCollider == null)
        {
            nodeCollider = gameObject.AddComponent<BoxCollider>();
            Debug.Log("BoxCollider added!");
        }

        Debug.Log($"Node started. Collider: {nodeCollider.name}, Is Trigger: {nodeCollider.isTrigger}");
    }

    void Update()
    {
        // COLOR CONTROL
        if (BuildManager.Instance.GetSelectedNode() == this)
        {
            rend.material.color = hoverColor;
        }
        else
        {
            rend.material.color = startColor;
        }

        // CLICK CONTROL 
        // Was the mouse clicked?
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Don't click if we are over the UI (Only for UI buttons)
            if (EventSystem.current.IsPointerOverGameObject() && IsPointerOverUIButton())
            {
                Debug.Log("UI BUTTON clicked, Node click blocked");
                return;
            }

            Debug.Log("LEFT MOUSE BUTTON PRESSED!");
            CheckClick();
        }
    }

    // Helper function: Check only if interactive elements like Button are clicked
    bool IsPointerOverUIButton()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Mouse.current.position.ReadValue();

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        // If there is any Button return true
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<Button>() != null)
            {
                return true;
            }
        }
        return false;
    }

    void CheckClick()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        Debug.Log($"=== RAYCAST ===");
        Debug.Log($"Mouse Pos: {mousePos}");
        Debug.Log($"Ray Origin: {ray.origin}");
        Debug.Log($"Ray Direction: {ray.direction}");
        Debug.Log($"Node Position: {transform.position}");
        Debug.Log($"Node Collider Bounds: {nodeCollider.bounds}");

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 5f);

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log($"✓ RAYCAST HIT: {hit.transform.name} | Distance: {hit.distance}");
            Debug.Log($"  Hit Point: {hit.point}");
            Debug.Log($"  Hit Collider: {hit.collider.name}");

            // If the clicked object is ME
            if (hit.transform == transform)
            {
                Debug.Log(">>> THIS SHOULD BE ME <<<");
                // Is it selected? Yes → Cancel selection. No → Select.
                if (BuildManager.Instance.GetSelectedNode() == this)
                {
                    BuildManager.Instance.DeselectNode();
                    Debug.Log("✓✓✓ SELECTION CANCELLED ✓✓✓");
                }
                else
                {
                    BuildManager.Instance.SelectNode(this);
                    Debug.Log("✓✓✓ LOT SELECTED ✓✓✓");
                }
            }
            else
            {
                Debug.Log($"!!! Another object clicked: {hit.transform.name}");
            }
        }
        else
        {
            Debug.Log("✗ RAYCAST DID NOT HIT ANYTHING");
        }
    }

    public void BuildTurret(GameObject prefab)
    {
        Debug.Log("--- 3. NODE: Construction order received ---");

        Tower towerScript = prefab.GetComponent<Tower>();
        int realCost = (towerScript != null) ? towerScript.Cost : 50;

        if (MissionControl.Instance.SpendMoney(realCost))
        {
            Vector3 buildPos = transform.position + new Vector3(0, 0.2f, 0);
            turret = Instantiate(prefab, buildPos, transform.rotation);

            // Clean "(Clone)" text from the name (Ex: "ArcherTower(Clone)" -> "ArcherTower")
            string rawName = turret.name.Replace("(Clone)", "");

            // Get a new ID from GameLogger (Ex: "ArcherTower-ID001")
            string uniqueID = GameLogger.GetNewID(rawName);

            // Change the object name in the scene with this ID
            turret.name = uniqueID;

            // Coordinate format (X, Y)
            string posStr = $"({transform.position.x:F0}, {transform.position.y:F0})";

            // Get remaining money
            int remainingMoney = MissionControl.Instance.GetCurrentMoney();

            // Write to log
            GameLogger.Write($"User built '{uniqueID}' at {posStr}. Remaining Money: {remainingMoney}.");

            // Also inform the console
            Debug.Log($"LOG ADDED: {uniqueID} built.");

            // Debug.Log($"--- 4. RESULT: TOWER PLANTED for {realCost} gold! ---");
        }
        else
        {
            Debug.Log($"--- ERROR: Not enough money! Required: {realCost} ---");
        }
    }

}