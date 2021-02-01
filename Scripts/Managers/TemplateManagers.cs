using UnityEngine;


/*
Controllers:
- Managers
- Prefabs
- Singleton (sempre vivos na tela, sempre existem somente 1 no jogo inteiro)
- Instancia models/datas
 */

public class Managers : MonoBehaviour
{
    // Variables declared in Unity Editor
    //[Header("Header")]
    //[Tooltip("Tooltip")]
    //[HideInInspector]
    //public float gizmoRadius = 0.0f;
    //public Rigidbody rb = null;

    // Script Lyfecycle: https://docs.unity3d.com/Manual/ExecutionOrder.html


    private static Managers _instance = null;

    /// <summary>
    /// Singleton implementation for prefabs
    /// Awake is called when the script instance is being loaded. Only once in your life.
    /// </summary>
    void Awake()
    {
        if (!_instance)
            _instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(_instance);
    }
}
