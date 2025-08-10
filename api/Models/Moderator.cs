namespace api.Models
{
    public class Moderator
    {
        public int Id { get; set; }

        public void CreateSection(string sectionName)
        {
            // Logic to create a section
        }

        public void EditSection(string sectionName, string newDetails)
        {
            // Logic to edit a section
        }

        public void DeleteSection(string sectionName)
        {
            // Logic to delete a section
        }

        public void AddNormative(string normativeName)
        {
            // Logic to add a normative
        }

        public void EditNormative(string normativeName, string newDetails)
        {
            // Logic to edit a normative
        }

        public void DeleteNormative(string normativeName)
        {
            // Logic to delete a normative
        }
    }
}
