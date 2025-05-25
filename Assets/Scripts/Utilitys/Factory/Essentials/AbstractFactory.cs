using Factory.Essentials;

namespace Utilitys.Factory.Essentials
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
