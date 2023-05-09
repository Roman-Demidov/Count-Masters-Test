using System;

namespace countMastersTest.infrastructure.input
{
    public interface IInputService
    {
        public event Action<SwipeData> onSwipe;
    }
}
