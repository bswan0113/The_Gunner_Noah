
using UnityEngine;

namespace Features.Common
{
    public interface IPushable
    {
        public void Pushed(Vector3 pushDirection);
    }
}