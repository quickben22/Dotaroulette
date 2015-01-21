using System;

namespace WebApplication5.Framework.Abstractions
{
    public interface IProgressManager
    {
        void SetCompleted(String taskId, String format, params Object[] args);
        void SetCompleted(String taskId, Int32 percentage);
        void SetCompleted(String taskId, String step);
        String GetStatus(String taskId);
        void RequestTermination(String taskId);
        Boolean ShouldTerminate(String taskId);
    }
}