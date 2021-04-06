using BotDatabase;
using BotDatabase.Interface;
using BotDatabase.Service;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot;
using DiscordBot.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WowheadParser.Interface;
using WowheadParser.Service;

namespace WoWNotification
{
    class Program
    {
        private DiscordSevice _discordService;
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
                .BuildServiceProvider();

            await _services.GetService<ConfigHandler>().PopulateConfig();

            _discordService = new DiscordSevice(_services);
            await _discordService.Run(_services.GetService<ConfigHandler>().GetToken());
        }
    }
}
