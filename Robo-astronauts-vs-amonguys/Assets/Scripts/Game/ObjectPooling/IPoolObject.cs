using System;

namespace ObjectPooling
{
    public interface IPoolObject
    {
        void OnActivation();
    }
}