namespace SpaceInvaders.Iterator
{
    public interface IIterator<T>
    {
        void Next();
        T Current();
        bool HasNext();
        void Reset();
    }
}