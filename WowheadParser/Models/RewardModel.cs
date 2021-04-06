namespace WowheadParser.Models
{
    public class RewardModel: NameModel
    {
        public int Quality { get; set; }
        public string Icon { get; set; }
        public ScreenshotModel Screenshot { get; set; }
        public JsonEquip jsonequip { get; set; }
        public int Attainable { get; set; }
        public int Flags2 { get; set; }
    }

    public class ScreenshotModel
    { 
        public int Id { get; set; }
        public int ImageType { get; set; }
    }

    public class JsonEquip
    { 
        public int? ReqLevel { get; set; }
    }
}
