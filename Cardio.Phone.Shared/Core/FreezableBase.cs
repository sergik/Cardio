namespace Cardio.Phone.Shared.Core
{
    public abstract class FreezableBase: IFreezable
    {
        protected bool IsFrozen { get; private set; }

        public void Freeze()
        {
            IsFrozen = true;
        }

        protected void EnsureNotFrozen()
        {
            if (IsFrozen)
            {
                throw new UnableToModifyFrozenObjectException();
            }
        }
    }
}