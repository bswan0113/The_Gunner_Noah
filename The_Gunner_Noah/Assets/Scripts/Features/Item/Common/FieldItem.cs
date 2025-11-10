using TMPro;
using UnityEngine;

namespace Features.Item.Common
{
    public class FieldItem : MonoBehaviour
    {
        [SerializeField]
        public ItemData itemData;

        [SerializeField] private GameObject infoPanel;
        [SerializeField] private TextMeshProUGUI nameTextOnPanel;
        [SerializeField] private TextMeshProUGUI descriptionTextOnPanel;

        public ItemData GetItemData() => itemData;

        private void Awake()
        {
            if (infoPanel != null)
            {
                infoPanel.SetActive(false);
            }
        }

        public void OnInvestigateStart()
        {
            if (itemData == null || infoPanel == null) return;

            if (nameTextOnPanel != null)
            {
                nameTextOnPanel.text = itemData.itemName;
            }
            if (descriptionTextOnPanel != null)
            {
                descriptionTextOnPanel.text = itemData.itemDescription;
            }
            if (Camera.main != null)
            {
                Transform mainCamTransform = Camera.main.transform;
                Vector3 lookDirection = transform.position - mainCamTransform.position;
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                infoPanel.transform.rotation = targetRotation;
            }
            infoPanel.SetActive(true);
        }

        public void OnInvestigateEnd()
        {
            if (infoPanel != null)
            {
                infoPanel.SetActive(false);
            }
        }

        public void OnPickup()
        {
            Destroy(gameObject);
        }
    }
}