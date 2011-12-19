using System.Collections.Generic;
using PivotalTracker.FluentAPI.Domain;

namespace PivotalTracker.FluentAPI.Repository
{
    //TODO: Implementer TaskRepo
    /// <summary>
    /// This repository manages Pivotal Task
    /// </summary>
    /// <remarks>Not implemented Yet</remarks>
    public class PivotalTaskRepository : PivotalTrackerRepositoryBase
    {
        public PivotalTaskRepository(Token token) : base(token)
        {
            throw new System.NotImplementedException();
        }

        public Task GetTask(int projectId, int storyId, int taskId)
        {
            return null;
            
        }

        public IEnumerable<Task> GetTasks(int projectId, int storyId)
        {
            return null;

        }

        public Task AddTask(int projectId, int storyId, string description)
        {
            return null;
            
        }

        public Task UpdateTask(int projectId, int storyId, Task task)
        {
            return null;
            
        }

        public Task DeleteTask(int projectId, int storyId, int taskId)
        {
            return null;
            
        }
    }
}