namespace api.Models
{
    public class Normative
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> Students { get; set; }
        public List<string> Teachers { get; set; }
        public int MaxStudents { get; set; }
    }
}
