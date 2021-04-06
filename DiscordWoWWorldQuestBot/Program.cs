using BotDatabase;
using BotDatabase.Interface;
using BotDatabase.Service;
using Discord.Commands;
using Discord.WebSocket;
using DiscordWoWWorldQuestBot.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WowheadParser.Interface;
using WowheadParser.Service;

namespace DiscordWoWWorldQuestBot
{
    class Program
    {
        private IServiceProvider _services;

        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _services = new ServiceCollection()
                .AddDbContext<WorldQuestContext>()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<ConfigHandler>()
                .AddScoped<IDbService, DbService>()
                .AddScoped<IWowheadWorldQuestParserService, WowheadWorldQuestParserService>()
                .AddSingleton<DbService>()
                .AddSingleton<WowheadWorldQuestParserService>()
                .AddSingleton<DiscordSevice>()
                .BuildServiceProvider();

            await _services.GetService<ConfigHandler>().PopulateConfig();

            await _services.GetService<DiscordSevice>().Run(_services.GetService<ConfigHandler>().GetToken());
        }
    }
}
