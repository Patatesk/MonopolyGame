using UnityEngine;

namespace Flexalon
{
    public interface InputProvider
    {
        // True on the frame that the input becomes active.
        bool Activated { get; }

        // True if the input is active, e.g. button is being held down.
        bool Active { get; }

        // Ray to cast to determine what should be moved / hit.
        Ray Ray { get; }
    }
}