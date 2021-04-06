using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using WowheadParser.Interface;
using WowheadParser.Static.Enums;
using WowheadParser.Models;
using WowheadParser.Extensions;

namespace WowheadParser.Service
{
    public class WowheadWorldQuestParserService : IWowheadWorldQuestParserService
    {
        private readonly string urlSource;
        private WowheadLocalizationsEnum _localization;
        private WowRegionsEnum _region;

        public WowheadWorldQuestParserService(WowheadLocalizationsEnum localizaion, WowRegionsEnum region)
        {
            _localization = localizaion;
            _region = region;

            switch (_localization)
            {
                case WowheadLocalizationsEnum.RuRu:
                    urlSource = $"http://ru.wowhead.com/world-quests/sl/{_region.EnumToString()}";
                    break;
                case WowheadLocalizationsEnum.Default:
                default:
                    urlSource = $"http://wowhead.com/world-quests/sl/{_region.EnumToString()}";
                    break;
            }

        }

        public async Task<List<TaskModel>> GetActiveWQ()
        {
            Dictionary<int, RewardModel> rewards = null;
            Dictionary<int, QuestsModel> quests = null;
            Dictionary<int, NameModel> fractions = null;

            using (WebClient client = new WebClient())
            {
                string htmlCode = await client.DownloadStringTaskAsync(urlSource);

                if (!string.IsNullOrEmpty(htmlCode))
                {
                    var pattern = @"WH.Gatherer.addData\((.*?)\);";
                    var options = RegexOptions.Multiline;

                    foreach (Match match in Regex.Matches(htmlCode, pattern, options))
                    {
                        var code = match.Groups[1].ToString();

                        if (code.StartsWith("3"))
                            rewards = DeserializeObject<Dictionary<int, RewardModel>>(code);

                        if (code.StartsWith("5"))
                            quests = DeserializeObject<Dictionary<int, QuestsModel>>(code);

                        if (code.StartsWith("8"))
                            fractions = DeserializeObject<Dictionary<int, NameModel>>(code);
                    }
                }
            }

            return quests.Select(x => new TaskModel 
            {
                Id = x.Key,
                Name = x.Value.Name
            }).ToList();
        }

        private T DeserializeObject<T>(string code)
        {
            code = code.Substring(code.IndexOf("{"));
            return JsonConvert.DeserializeObject<T>(code);
        }
    }
}
