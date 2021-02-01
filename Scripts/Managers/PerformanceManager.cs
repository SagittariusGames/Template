using UnityEngine;

public class PerformanceManager : MonoBehaviour
{
    [Header("Physics")]
    [Tooltip("FixedUpdate does not work anymore!")]
    public bool disableAutoSimulation = false;

    [Header("Profiler")]
    public bool checkedCPU = false;
    public bool checkedGPU = false;
    public bool checkedUI = false;
    public bool checkedRender = false;
    public bool checkedMemory = false;
    public bool checkedAudio = false;
    public bool checkedPhysics = false;
    public bool checkedIllumination = false;

    [Header("General")]
    public bool warningResourcesFolder = true;
    public bool showStatisticsWindow = false;
    public bool checkedVisualStudioCodeAnalyzer = false;

    private static PerformanceManager _instance = null;

    void Awake()
    {
        if (!_instance)
            _instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(_instance);
    }

    // Start is called before the first frame update
    void Start()
    {
        Object[] obj;
        
        // physics
        Physics.autoSimulation = !disableAutoSimulation;
        Physics.autoSyncTransforms = false;

        //general
        if (Debug.isDebugBuild && warningResourcesFolder)
        {
            obj = Resources.LoadAll("");
            Debug.LogWarning("Resources: " + obj.Length);
            foreach (var t in obj)
            {
                Debug.LogWarning("Resources: " + t.name + "(" + t.GetType().ToString() + ")");
            }
            Resources.UnloadUnusedAssets();
        }
    }
}
