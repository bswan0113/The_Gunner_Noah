using Core;
using Features.Monster;
using UnityEngine;

namespace Features.Obstacles
{
    public class RazorTrap : Movable
    {
        [SerializeField]private float detectRange = 5f;
        [SerializeField] private LayerMask playerLayer;
        protected override void Update()
        {
            base.Update();
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectRange, playerLayer);


            Transform targetPlayer = colliders[0].transform;
            Vector3 directionToPlayer = (targetPlayer.position - transform.position).normalized;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectRange))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    AudioManager.Instance.PlaySfx(AudioManager.Instance.razor);
                }
            }
        }
    }
}