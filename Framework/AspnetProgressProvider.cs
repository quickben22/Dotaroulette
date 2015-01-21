using System;
using System.Web;
using System.Web.Caching;
using WebApplication5.Framework.Abstractions;

namespace WebApplication5.Framework
{
    public class AspnetProgressProvider : IProgressDataProvider
    {
        private Int32 _durationInSeconds = 5;
        public void Set(String taskId, String progress, Int32 durationInSeconds=5)
        {
            _durationInSeconds = 1;

            HttpContext.Current.Cache.Insert(
                taskId,
                new TaskStatus(progress),
                null,
                DateTime.Now.AddSeconds(_durationInSeconds),
                Cache.NoSlidingExpiration);
        }

        public String Get(String taskId)
        {
            var taskStatus = HttpContext.Current.Cache[taskId] as TaskStatus;
            return taskStatus == null ? String.Empty : taskStatus.Status;
        }

        public void Abort(String taskId)
        {
            HttpContext.Current.Cache.Insert(
                taskId,
                new TaskStatus(String.Empty, true),
                null,
                DateTime.Now.AddSeconds(_durationInSeconds),
                Cache.NoSlidingExpiration);
        }

        public Boolean ShouldTerminate(String taskId)
        {
            var taskStatus = HttpContext.Current.Cache[taskId] as TaskStatus;
            return taskStatus == null ? false : taskStatus.Aborted;
        }
    }
}