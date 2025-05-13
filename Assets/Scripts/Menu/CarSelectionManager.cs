using UnityEngine;
using Cinemachine;

public class CarSelectionManager : MonoBehaviour
{
    [SerializeField] private string[] carPrefabNames; // Names like "Car1", "Car2", etc.
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private GameObject selectionPanel;

    private GameObject currentCarInstance;

    public void ShowSelectionPanel()
    {
        selectionPanel.SetActive(true);
    }

    public void HideSelectionPanel()
    {
        selectionPanel.SetActive(false);
    }

    public void SelectCar(int index)
    {
        if (currentCarInstance != null)
            Destroy(currentCarInstance);

        // Load prefab by name
        GameObject prefab = Resources.Load<GameObject>("Cars/" + carPrefabNames[index]);
        currentCarInstance = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

        // Update ScoreManager
        FindObjectOfType<ScoreManager>().SetPlayer(currentCarInstance.transform);

        // Update camera
        virtualCamera.Follow = currentCarInstance.transform;
        virtualCamera.LookAt = currentCarInstance.transform;

        // Hide the panel
        HideSelectionPanel();
    }
}
