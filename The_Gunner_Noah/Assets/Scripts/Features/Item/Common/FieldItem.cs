using Features.Item.abc;
using TMPro;
using UnityEngine;

namespace Features.Item
{
    public class FieldItem : MonoBehaviour
    {
        [Header("아이템 데이터")]
        [SerializeField]
        public ItemData itemData;

        [Header("월드 스페이스 UI")]
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
                nameTextOnPanel.text = itemData.itemName;
            if (descriptionTextOnPanel != null)
                descriptionTextOnPanel.text = itemData.itemDescription;

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