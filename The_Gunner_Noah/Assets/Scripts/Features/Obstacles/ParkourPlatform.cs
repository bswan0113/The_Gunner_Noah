using Core;
using Core.Managers;
using UnityEngine;

namespace Features.Obstacles
{
    public class ParkourPlatform : MonoBehaviour
    {
        public enum ParkourDirection
        {
            XPositive,
            XNegative,
            ZPositive,
            ZNegative
        }

        public ParkourDirection direction = ParkourDirection.XPositive;
        public float moveForwardSpeed = 40f;
        public float climbUpSpeed = 15f;

        private Vector3 moveDirection;

        private void Awake()
        {
            switch (direction)
            {
                case ParkourDirection.XPositive:
                    moveDirection = Vector3.right;
                    break;
                case ParkourDirection.XNegative:
                    moveDirection = Vector3.left;
                    break;
                case ParkourDirection.ZPositive:
                    moveDirection = Vector3.forward;
                    break;
                case ParkourDirection.ZNegative:
                    moveDirection = Vector3.back;
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Player.Player player = other.GetComponent<Player.Player>();
                Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();

                player.SetPakurState(true);
                player.ReleaseUseGravity();

                Vector3 initialVelocity = (moveDirection * moveForwardSpeed) + (Vector3.up * climbUpSpeed);
                playerRigidbody.velocity = initialVelocity;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Player.Player player = other.GetComponent<Player.Player>();
                player.SetUseGravity();
                player.SetPakurState(false);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Player.Player player = other.GetComponent<Player.Player>();
                Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();

                if(player.IsPressingForward)
                {
                    Vector3 parkourVelocity = (moveDirection * moveForwardSpeed) + (Vector3.up * climbUpSpeed);
                    playerRigidbody.velocity = parkourVelocity;
                    AudioManager.Instance.PlaySfx(AudioManager.Instance.run);
                }
                else
                {
                    playerRigidbody.velocity = Vector3.down * 2f;
                }
            }
        }
    }
}