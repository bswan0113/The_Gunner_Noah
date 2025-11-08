
using UnityEngine;

namespace Features
{
    public interface IPushable
    {
        public void Pushed(Vector3 pushDirection);
    }
}