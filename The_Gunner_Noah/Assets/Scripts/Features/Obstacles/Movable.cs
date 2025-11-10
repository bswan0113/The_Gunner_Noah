using UnityEngine;

namespace Features.Obstacles
{
    public abstract class Movable : MonoBehaviour
    {
        [SerializeField] private MovementDirection direction = MovementDirection.Vertical;
        [SerializeField] private float patrolHeight = 5f;
        [SerializeField] private float speed = 3f;

        private Vector3 _topPoint;
        private Vector3 _bottomPoint;
        private Vector3 _currentTarget;

        void Start()
        {
            _bottomPoint = transform.position;

            Vector3 moveVector;
            switch (direction)
            {
                case MovementDirection.Horizontal:
                    moveVector = Vector3.right * patrolHeight;
                    break;
                default:
                    moveVector = Vector3.up * patrolHeight;
                    break;
            }

            _topPoint = _bottomPoint + moveVector;
            _currentTarget = _topPoint;
        }

        protected virtual void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, _currentTarget, speed * Time.deltaTime);

            if (transform.position == _currentTarget)
            {
                if (_currentTarget == _topPoint)
                {
                    _currentTarget = _bottomPoint;
                }
                else
                {
                    _currentTarget = _topPoint;
                }
            }
        }

        public enum MovementDirection
        {
            Vertical,
            Horizontal
        }
    }
}