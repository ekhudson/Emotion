using UnityEngine;
using System.Collections;

public class SingletonComponent : MonoBehaviour
{
    public bool DoNotDestroyOnLoad = true;
    private static SingletonComponent mInstance;

    void Start () 
    {
        if (DoNotDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }

        if (mInstance != null)
        {
            Destroy(gameObject);
            return;
        }

        mInstance = this;
    }
}
