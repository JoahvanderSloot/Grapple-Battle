using UnityEngine;

public class GenericScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
{
    private static T m_instance;

    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                // Try to find an existing instance in the project
                T[] instances = Resources.FindObjectsOfTypeAll<T>();

                if (instances != null && instances.Length > 0)
                {
                    m_instance = instances[0];
                }
                else
                {
                    // If not found, create a new instance
                    m_instance = CreateInstance<T>();
                }
            }
            return m_instance;
        }
    }

    // Optional: if needed, use Awake for initialization logic
    protected virtual void OnEnable()
    {
        if (m_instance == null)
        {
            m_instance = this as T;
        }
    }
}