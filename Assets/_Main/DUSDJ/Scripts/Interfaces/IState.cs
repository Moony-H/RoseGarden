using System;

namespace DUSDJ
{
    public interface IState<T>
    {
        event Action StateEnterEvent;
        event Action StateExitEvent;

        void EnterState(T t);

        void ExitState();

    }

}

