using System;

namespace Entities.Interfaces
{
    public interface ISound
    {
        public void PlaySound(string soundName, string entityName);
        public event Action<string, string> Sounds;
    }
}