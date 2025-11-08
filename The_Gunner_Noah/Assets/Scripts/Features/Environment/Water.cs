using UnityEngine;

namespace Features.Environment
{
    public class Water : MonoBehaviour
    {

        [SerializeField] private float riseSpeed = 0.2f;

        void Update()
        {
            transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);
        }
    }
}
