using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DiscordWoWWorldQuestBot
{
    public class ConfigHandler
    {
        private Config config;
        private string configPath;

        struct Config
        {
            public string Token { get; set; }
        }

        public ConfigHandler()
        {
            config = new Config { Token = string.Empty };
        }

        public async Task PopulateConfig()
        {
            configPath = Path.Combine(Directory.GetCurrentDirectory(), "config.json".Replace(@"\", @"\\"));
            Console.WriteLine(configPath);

            if (!File.Exists(configPath))
            {
                using (var writer = File.AppendText(configPath))
                {
                    await writer.WriteLineAsync(JsonConvert.SerializeObject(config));
                }

                Console.WriteLine("WARNING! New config!");
                throw new Exception("No config available! Fill out newly created config file!");
            }

            using(var reader = new StreamReader(configPath))
            {
                config = JsonConvert.DeserializeObject<Config>(reader.ReadToEnd());
            }

            await Task.CompletedTask;
        }

        public string GetToken()
        {
            return config.Token;
        }
    }
}
