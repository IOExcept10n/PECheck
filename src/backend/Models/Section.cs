namespace api.Models
{
    public class Section
    {
        public string Name { get; set; }
        public string Schedule { get; set; }
        public List<string> Teachers { get; set; }
        public double Rating { get; set; }
        public List<string> Reviews { get; set; }
        public decimal Cost { get; set; }
        public List<int> Students { get; set; }
    }
}
