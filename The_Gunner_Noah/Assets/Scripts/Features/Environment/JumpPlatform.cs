using UnityEngine;

namespace Features.Environment
{
    public class JumpPlatform : MonoBehaviour
    {

        [SerializeField] private float jumpForce = 50f;
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

    }
}