using System;
using WowheadParser.Static.Enums;

namespace WowheadParser.Extensions
{
    static class EnumExtensions
    {
        public static string EnumToString(this WowRegionsEnum source)
        {
            var result = Enum.GetName(typeof(WowRegionsEnum), source).ToLower();
            return result;
        }
    }
    
}
