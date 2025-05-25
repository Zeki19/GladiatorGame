namespace Utilitys.Factory.Essentials
{
        public interface IConfigurable<in TConfig>
        {
                void Configure(TConfig config);
                void Initialize(TConfig config);
        }
}
