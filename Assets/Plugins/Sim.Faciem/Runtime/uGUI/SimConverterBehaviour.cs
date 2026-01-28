namespace Sim.Faciem.uGUI
{
    public abstract class SimConverterBehaviour<TFrom, TTo> : SimConverterBaseBehaviour
    {
        public abstract TTo Convert(TFrom from);
    }
}