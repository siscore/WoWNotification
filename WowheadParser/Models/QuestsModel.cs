namespace WowheadParser.Models
{
    public class QuestsModel: NameModel
    {
        public string Icon { get; set; }
        public int ReqClass { get; set; }
        public int ReqRace { get; set; }

        public string Name
        {
            get {
                return Name_ruru;
            }
        } 
    }
}
