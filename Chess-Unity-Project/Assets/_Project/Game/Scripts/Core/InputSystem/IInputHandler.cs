namespace SteampunkChess
{
    interface IInputHandler<T>
    {
        void ProcessInput(T input);
    }
}