using System;
using Utilities.Factory.Essentials;
using Utilities.Factory.WeaponFactory;
using Weapons;

namespace Factory.Essentials
{
    public class Factory<TObject, TConfig> : AbstractFactory<TObject, TConfig>
        where TObject : IConfigurable<TConfig>, new()
    { 
        public override TObject Create(TConfig config)
        {
           return base.Create(config);
        }
    }
}