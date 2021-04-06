using BotDatabase.Interface;
using BotDatabase.Model;
using BotDatabase.Service;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WowheadParser.Interface;
using WowheadParser.Service;
using WowheadParser.Static.Enums;

namespace DiscordWoWWorldQuestBot.Service
{
    public class DiscordSevice
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private readonly IDbService _dbService;

        public DiscordSevice(IServiceProvider services, IDbService dbService)
        {
            _services = services;
            _dbService = dbService;
        }

        public async Task Run(string token)
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _client.Log += Log;

            var _commandHandler = new CommandHandler(_services, _client, _commands);
            await _commandHandler.InstallCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            var timer = new Timer(CheckWorldQuestUpdate, null, 5 * 1000, 1 * 60 * 60000);

            await Task.Delay(-1);
        }

        private async void CheckWorldQuestUpdate(object state)
        {
            Console.WriteLine("->> CheckWorldQuestUpdate()");
            var parser = new WowheadWorldQuestParserService(WowheadLocalizationsEnum.RuRu, WowRegionsEnum.EU);

            var wqList = await parser.GetActiveWQ();

            Console.WriteLine($"->> Count: {wqList.Count}");

            var wqByUsers = await _dbService.GetUserToNotification(wqList.Select(x => new WorldQuestTaskModel 
            { 
                QuestId = x.Id,
                QuestName = x.Name
            }).ToList());

            await SendNotificationAboutWQ(wqByUsers);

            GC.Collect();
        }

        private async Task SendNotificationAboutWQ(List<UserWorldQuestModel> tasks)
        {
            var groupTask = tasks.GroupBy(x => x.UserId);

            foreach (var task in groupTask)
            {
                var userId = ulong.Parse(task.Key);
                var user = _client.GetUser(userId);

                if (user != null)
                {
                    var description = new StringBuilder();
                    task.ToList().ForEach(x => description.Append($"{x.QuestName} (Id: {x.QuestId})\n"));

                    var EB = new EmbedBuilder();
                    EB.WithTitle("Сейчас доступны следующие интересующие Вас квесты:");
                    EB.WithDescription(description.ToString());

                    await user.SendMessageAsync("", false, EB.Build());
                }
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
