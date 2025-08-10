namespace api.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Department { get; set; }
        public List<string> Sections { get; set; }
        public List<string> Normatives { get; set; }

        public void MarkStudentAttendance(int studentId, string section)
        {
            // Logic to mark attendance for a student
        }

        public void EnrollStudent(int studentId, string section)
        {
            // Logic to enroll a student
        }

        public void RemoveStudent(int studentId, string section)
        {
            // Logic to remove a student
        }

        public void GradeNormative(int studentId, string normative, int grade)
        {
            // Logic to grade a normative
        }

        public string GetCurrentDescription()
        {
            // Logic to get current description
            return "Current description";
        }

        public void CancelClass(string section)
        {
            // Logic to cancel a class
        }
    }
}
