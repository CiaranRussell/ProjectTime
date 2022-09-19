using ProjectTime.Data;
using ProjectTime.Models;

namespace ProjectTime.Utility
{
    public static class UserHelper
    {
        // User helper method using Linq query to return the logged in users List of assigned active project to log ProjectTime aganist 
        public static List<Project> GetUserProjects(ApplicationDbContext _db, string user)
        {
            var projects = (from p in _db.projects
                            join pu in _db.projectUsers on p.Id equals pu.ProjectId
                            where pu.UserId == user && pu.IsActive == true
                            select p).OrderBy(p => p.Name).ToList();
            return projects; 
            
        }

        // User helper method using Linq query to return the logged in users ProjectUser Id for the selected project they are creating
        // a time log against
        public static int GetProjectUserId(ApplicationDbContext _db, string user, int projectId)
        {
            var projectUserId = (from pu in _db.projectUsers
                                 join p in _db.projects on pu.ProjectId equals p.Id
                                 join u in _db.applicationUsers on pu.UserId equals u.Id
                                 where u.Id == user && p.Id == projectId
                                 select pu).First<ProjectUser>().Id;
    
            return projectUserId;

        }

    }
}
