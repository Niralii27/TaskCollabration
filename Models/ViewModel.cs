using System.Reflection;

namespace TaskCollabration.Models
{
    public class ViewModel
    {
        public UserProjectModel Projects { get; set; } // Single Project
        public List<FetchTaskUserModel> Tasks { get; set; } // List of Tasks
    }
}
