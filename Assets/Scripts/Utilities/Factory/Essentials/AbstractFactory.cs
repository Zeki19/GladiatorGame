using Factory.Essentials;

namespace Utilities.Factory.Essentials
{
  public abstract class AbstractFactory<TObject, TConfig> 
        where TObject : IConfigurable<TConfig>, new()
    {
        public virtual TObject Create( TConfig config)
        {
            var newborn = new TObject();
            newborn.Initialize(config);
            newborn.Configure(config);
            return newborn;
        }
    }
}
