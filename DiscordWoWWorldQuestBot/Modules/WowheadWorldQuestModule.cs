using BotDatabase.Interface;
using Discord;
using Discord.Commands;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordWoWWorldQuestBot.Modules
{
    public class WowheadWorldQuestModule : ModuleBase<SocketCommandContext>
    {
        private readonly IDbService _dbService;

        public WowheadWorldQuestModule(IDbService dbService)
        {
            _dbService = dbService;
        }

        /// <summary>
        /// Список команд
        /// </summary>
        /// <returns></returns>
        [Command("help", RunMode = RunMode.Async)]
        [Summary("Помощь")]
        public async Task Help()
        {
            var EB = new EmbedBuilder();
            EB.WithTitle("Список доступных команд");
            EB.WithDescription("!help - вывести все команды \n" +
                "!setregion - Установить регион (EU|US) - в процессе \n" +
                "!setlanguage - Установить язык (Default|ruru) - в процессе \n" +
                "!addwq questId - Добавить отслеживание по Id\n" +
                "!addwq questName - Добавить отслеживание по наименоваю квеста\n" +
                "!delwq questId - Удалить отслеживание по Id\n" +
                "!delwq questName - Удалить отслеживание по наименоваю квеста\n" +
                "!showwq - показать какие квесты отслеживаются");

            await ReplyAsync("", false, EB.Build());
        }

        /// <summary>
        /// Добавляет квест по Id для слежения 
        /// </summary>
        /// <param name="questId"></param>
        /// <returns></returns>
        [Command("addwq", RunMode = RunMode.Async)]
        [Summary("Добавляет квест по Id для слежения")]
        public async Task AddAsync(int questId)
        {
            var userId = Context.User.Id;
            await _dbService.AddQuest(userId.ToString(), questId);
            await ReplyAsync($"Добавлен квест с Id: { questId }");
        }

        /// <summary>
        /// Добавляет квест по наименованию для слежения 
        /// </summary>
        /// <param name="questName"></param>
        /// <returns></returns>
        [Command("addwq", RunMode = RunMode.Async)]
        [Summary("Добавляет квест по наименованию для слежения")]
        public async Task AddAsync(string questName)
        {
            var userId = Context.User.Id;
            await _dbService.AddQuest(userId.ToString(), questName);
            await ReplyAsync($"Добавлен квест: { questName }");
        }

        /// <summary>
        /// Удаление квеста по Id
        /// </summary>
        /// <param name="questId"></param>
        /// <returns></returns>
        [Command("delwq", RunMode = RunMode.Async)]
        [Summary("Добавляет квест по наименованию для слежения")]
        public async Task DeleteAsync(int questId)
        {
            var userId = Context.User.Id;
            await _dbService.DeleteQuest(userId.ToString(), questId);
            await ReplyAsync($"Квест удален: { questId }");
        }

        /// <summary>
        /// Удаление квеста по названию
        /// </summary>
        /// <param name="questName"></param>
        /// <returns></returns>
        [Command("delwq", RunMode = RunMode.Async)]
        [Summary("Добавляет квест по наименованию для слежения")]
        public async Task DeleteAsync(string questName)
        {
            var userId = Context.User.Id;
            await _dbService.DeleteQuest(userId.ToString(), questName);
            await ReplyAsync($"Добавлен квест: { questName }");
        }
        /// <summary>
        /// Отображает список всех добавленных квестов
        /// </summary>
        /// <returns></returns>
        [Command("showwq", RunMode = RunMode.Async)]
        [Summary("Список всех добавленных квестов")]
        public async Task ShowAsync()
        {
            var userId = Context.User.Id;
            var quests = await _dbService.GetListByUser(userId.ToString());
            var title = string.Empty;
            var description = string.Empty;

            if (quests.Count == 0)
            {
                title = "У Вас еще нет квестов для отслеживания!";
                return;
            }

            var filteredQuests = quests.Where(x => x.QuestId.HasValue).ToList();
            var msg = new StringBuilder();

            if (filteredQuests.Any())
            {
                msg.AppendLine("Отслеживается по Id:");
                filteredQuests.ForEach(x => msg.AppendLine($"{x.QuestId.Value.ToString()}"));
            }

            filteredQuests = quests.Where(x => !string.IsNullOrEmpty(x.QuestName)).ToList(); ;

            if (filteredQuests.Any())
            {
                msg.AppendLine("Отслеживается по наименованию:");
                filteredQuests.ForEach(x => msg.AppendLine($"{x.QuestName}"));
            }

            var EB = new EmbedBuilder();
            EB.WithTitle(title);
            EB.WithDescription(msg.ToString());

            await ReplyAsync(string.Empty, false, EB.Build());
        }
    }
}