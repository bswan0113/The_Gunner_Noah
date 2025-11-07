using UnityEngine;

namespace Features.UI
{
    public class CrosshairFollow : MonoBehaviour
    {
        void Start()
        {
            Cursor.visible = false;
        }

        void Update()
        {
            transform.position = Input.mousePosition;
        }
    }
}