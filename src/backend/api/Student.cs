namespace api.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Group { get; set; }
        public string Section { get; set; }
        public string Normatives { get; set; }
        public bool HasPaidForSection { get; set; }
        public bool HasTakenTest { get; set; }

        public void EnrollInSection(string section)
        {
            // Logic to enroll in a section
        }

        public void MarkAttendance(string section)
        {
            // Logic to mark attendance
        }

        public void UnenrollFromSection(string section)
        {
            // Logic to unenroll from a section
        }

        public void LeaveFeedback(string section, string feedback)
        {
            // Logic to leave feedback
        }

        public void PayForSection(decimal amount)
        {
            // Logic to pay for a section
        }

        public string GetSchedule()
        {
            // Logic to get the current schedule
            return "Schedule details";
        }

        public void EnrollInNormatives(string normative)
        {
            // Logic to enroll in normatives
        }

        public string GetNormativeDetails()
        {
            // Logic to get normative details
            return "Normative details";
        }
    }
}
