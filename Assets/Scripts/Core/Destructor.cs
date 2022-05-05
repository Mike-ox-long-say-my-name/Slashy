using UnityEngine;

namespace Core
{
    public class Destructor : MonoBehaviour
    {
        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}