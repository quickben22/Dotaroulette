using System;
using System.Web.Mvc;

namespace WebApplication5.Framework
{
    /// <summary>
    /// Shields some common tasks for a controller that includes monitorable operations.
    /// </summary>
    public class ProgressBarController : Controller
    {
        protected readonly ProgressManager ProgressManager;

        /// <summary>
        /// Initializes a progress manager instance.
        /// </summary>
        public ProgressBarController()
        {
            ProgressManager = new ProgressManager();
        }

        /// <summary>
        /// Retrieves the ID of the current task (via HTTP header)
        /// </summary>
        /// <returns>Task ID</returns>
        public String GetTaskId()
        {
            // Get the header with the task ID
            var id = Request.Headers[ProgressManager.HeaderNameTaskId];
            return id ?? String.Empty;
        }

        /// <summary>
        /// Endpoint that retrieves the current status of an operation.
        /// </summary>
        /// <returns>Description</returns>
        public String Status()
        {
            var taskId = GetTaskId(); 
            return ProgressManager.GetStatus(taskId);
        }

        /// <summary>
        /// Endpoint that places a request for aborting the operation
        /// </summary>
        public void Abort()
        {
            var taskId = GetTaskId();
            ProgressManager.RequestTermination(taskId);
        }
    }
}