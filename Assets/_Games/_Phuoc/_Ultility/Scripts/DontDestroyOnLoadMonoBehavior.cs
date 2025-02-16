using UnityEngine;

namespace MyExtension
{
    public class DontDestroyOnLoadMonoBehavior : MonoBehaviour
    {
        private void Awake()
        {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
    } 
}
