using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // Tower Prefabs 
    public GameObject FreyaTowerPrefab;
    public GameObject OdinTowerPrefab;
    public GameObject YmirTowerPrefab;

    // Which node (lot) is currently selected on the map?
    private Node selectedNode;

    public Node GetSelectedNode()
    {
        return selectedNode;
    }

    public void SelectNode(Node node)
    {
        selectedNode = node;
        Debug.Log("BuildManager: Node stored in memory -> " + node.name);
    }

    public void DeselectNode()
    {
        selectedNode = null;
        Debug.Log("BuildManager: Selection cleared.");
    }
}