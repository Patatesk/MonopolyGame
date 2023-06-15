using UnityEngine;

namespace Flexalon
{
    public class FlexalonMouseInputProvider : InputProvider
    {
        public bool Activated => Input.GetMouseButtonDown(0);
        public bool Active => Input.GetMouseButton(0);
        public Ray Ray => Camera.main.ScreenPointToRay(Input.mousePosition);
    }
}