namespace Core
{
    public class AmbushResetter : IAmbushResetter
    {
        private readonly Ambush[] _ambushes;
        
        public AmbushResetter(IObjectLocator objectLocator)
        {
            _ambushes = objectLocator.FindAll<Ambush>();
        }

        public void ResetAll()
        {
            foreach (var ambush in _ambushes)
            {
                ambush.ResetAmbush();
            }
        }
    }
}