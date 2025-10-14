using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        RuntimeHelpers.RunClassConstructor(typeof(DataManager).TypeHandle);
        SceneManager.LoadSceneAsync(1);
    }
}
