using Core.Characters;
using Core.Characters.Interfaces;
using Core.Characters.Mono;

namespace Core.Modules
{
    public class MixinBalance : MixinResource
    {
        private IResource _balance;

        public IResource Balance
        {
            get
            {
                if (_balance != null)
                {
                    return _balance;
                }

                _balance = new BalanceResource(MaxValue);
                return _balance;
            }
        }

        public override IResource Resource => Balance;
    }
}