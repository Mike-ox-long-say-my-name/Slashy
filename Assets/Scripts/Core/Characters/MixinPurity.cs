using Core.Characters.Interfaces;
using Core.Characters.Mono;

namespace Core.Characters
{
    public class MixinPurity : MixinResource
    {
        public override IResource Resource => Purity;

        private IResource _resource;

        public IResource Purity
        {
            get { return _resource ??= new PurityResource(MaxValue); }
        }
    }
}