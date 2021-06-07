using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace hefesto.base_hefesto.Services
{
    public interface IMessageService
    {
        string GetMessage(string key);
        List<IConfigurationSection> GetMessages();
    }
}