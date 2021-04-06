using Discord.Commands;
using System.Threading.Tasks;

namespace DiscordWoWWorldQuestBot.Modules
{
    public class InfoModule: ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [Summary("Echoes a message.")]
        public Task SayAsync([Remainder][Summary("The text to echo")] string echo) => ReplyAsync(echo);

        [Command("ping", RunMode = RunMode.Async)]
        [Summary("A simple test")]
        [RequireContext(ContextType.Guild)]
        public async Task PingAsync() => await ReplyAsync("pong");
    }
}
