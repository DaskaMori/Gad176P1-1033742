using UnityEngine;
using UnityEngine.SceneManagement;

public class SpinnerTrap : MonoBehaviour
{
    public GameObject bladePrefab;
    public float radius = 0.5f;
    public float spinSpeed = 180f;

    private GameObject bladeInstance;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (bladePrefab == null) return;
        bladeInstance = Instantiate(bladePrefab, transform.position, Quaternion.identity, transform);
        var orbiter = bladeInstance.GetComponent<Orbiter>();
        if (orbiter != null)
        {
            orbiter.center = transform;
            orbiter.radius = radius;
            orbiter.spinSpeed = spinSpeed;
        }
    }
}