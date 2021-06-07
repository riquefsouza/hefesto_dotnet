#pragma warning disable 0649

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace hefesto.base_hefesto.Services
{
    public class MessageService: IMessageService
    {
        private readonly IConfiguration _configuration;

        private List<IConfigurationSection> _configSection;

        private readonly IDictionary<string, string> _message;

        public MessageService(IConfiguration configuration)
        {
            _configuration = configuration;
            _configSection = _configuration.GetSection("Messages_pt_BR").GetChildren().ToList();

            _message = new Dictionary<string, string>();

            foreach (var msg in _configSection)
            {
                _message.Add(msg.Key, msg.Value);
            }
        }

        public string GetMessage(string key)
        {
            return _message[key];
        }

        public List<IConfigurationSection> GetMessages()
        {
            return _configSection;
        }
    }
}
