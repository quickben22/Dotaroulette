using System;
using WebApplication5.Framework.Abstractions;

namespace WebApplication5.Framework
{
    /// <summary>
    /// Server-side API for the SimpleProgress framework
    /// </summary>
    public class ProgressManager : IProgressManager
    {
        /// <summary>
        /// Header through which the ID that uniquely identifies the remote task is identified.
        /// </summary>
        public const String HeaderNameTaskId = "X-ProgressBar-TaskId";

        private readonly IProgressDataProvider _dataProvider;

        /// <summary>
        /// Builds the root of the framework passing the component responsible for storing 
        /// the status of each request. By default, status is stored in the ASP.NET cache.
        /// </summary>
        public ProgressManager() : this(new AspnetProgressProvider())
        {}
        public ProgressManager(IProgressDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        /// <summary>
        /// Used by controllers (or worker services) to set the percentage of work completed.
        /// </summary>
        /// <param name="taskId">ID of the current task being monitored</param>
        /// <param name="format">Format of the string to output</param>
        /// <param name="args">Arguments to embed in the string</param>
        public void SetCompleted(String taskId, String format, params Object[] args)
        {
            if (String.IsNullOrEmpty(taskId))
                return;
            var step = String.Format(format, args);
            _dataProvider.Set(taskId, step);
        }

        /// <summary>
        /// Used by controllers (or worker services) to set the percentage of work completed.
        /// </summary>
        /// <param name="taskId">ID of the current task being monitored</param>
        /// <param name="percentage">Percentage of the work accomplished</param>
        public void SetCompleted(String taskId, Int32 percentage)
        {
            percentage = (percentage < 0) ? 0 : percentage;
            percentage = (percentage > 100) ? 100 : percentage;

            SetCompleted(taskId, "{0} %", percentage);
        }

        /// <summary>
        /// Used by controllers (or worker services) to set a description of the current task. 
        /// </summary>
        /// <param name="taskId">ID of the current task being monitored</param>
        /// <param name="step">Description of the current stage</param>
        public void SetCompleted(String taskId, String step)
        {
            if (String.IsNullOrEmpty(taskId))
                return;

            _dataProvider.Set(taskId, step);
        }

        /// <summary>
        /// Returns the status of the current task.
        /// </summary>
        /// <param name="taskId">ID of the current task being monitored</param>
        /// <returns>Current status of the task</returns>
        public String GetStatus(String taskId)
        {
            const String invalidStatus = "...";
            if (String.IsNullOrEmpty(taskId))
                return invalidStatus;

            var buffer = _dataProvider.Get(taskId);
            return String.IsNullOrEmpty(buffer) ? invalidStatus : buffer;
        }

        /// <summary>
        /// Queues a message for the remote task to terminate
        /// </summary>
        /// <param name="taskId">ID of the current task being monitored</param>
        public void RequestTermination(String taskId)
        {
            if (String.IsNullOrEmpty(taskId))
                return;
            _dataProvider.Abort(taskId);
        }

        /// <summary>
        /// Reads whether the task should terminate
        /// </summary>
        /// <param name="taskId">ID of the current task being monitored</param>
        /// <returns>True if the task should be interrupted, false otherwise</returns>
        public Boolean ShouldTerminate(String taskId)
        {
            return _dataProvider.ShouldTerminate(taskId); 
        }
    }
}