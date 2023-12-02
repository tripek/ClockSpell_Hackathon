using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    [HideInInspector] public UnityEvent onBlockClean;
    [HideInInspector] public UnityEvent onEffectActivated;
    public static EventManager instance;
    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
}
