using UnityEngine;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;

    void Start()
    {
        buildManager = BuildManager.Instance;
    }

    // --- Freya Button ---
    public void BuyFreyaTower()
    {
        // Is there a currently selected lot?
        Node node = buildManager.GetSelectedNode();

        if (node != null)
        {
            // If so, give the "Start Construction" order to that lot
            // We are calling the 'BuildTurret' function in the Node code.
            node.BuildTurret(buildManager.FreyaTowerPrefab);

            // Deselect when finished (so the lot's color resets)
            buildManager.DeselectNode();
        }
    }

    // --- OdinTower Button ---
    public void BuyOdinTower()
    {
        Node node = buildManager.GetSelectedNode();
        if (node != null)
        {
            node.BuildTurret(buildManager.OdinTowerPrefab);
            buildManager.DeselectNode();
        }
    }

    // --- YmirTower Button ---
    public void BuyYmirTower()
    {
        Node node = buildManager.GetSelectedNode();
        if (node != null)
        {
            node.BuildTurret(buildManager.YmirTowerPrefab);
            buildManager.DeselectNode();
        }
    }
}