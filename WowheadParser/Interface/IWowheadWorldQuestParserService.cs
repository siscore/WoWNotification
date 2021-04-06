using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WowheadParser.Models;

namespace WowheadParser.Interface
{
    public interface IWowheadWorldQuestParserService
    {
        Task<List<TaskModel>> GetActiveWQ();
    }
}
