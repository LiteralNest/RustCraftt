using System;

namespace Building_System
{
    public interface IDestroyable
    {
        public Action<IDestroyable> OnDestroyed { get; set; }
    }
}