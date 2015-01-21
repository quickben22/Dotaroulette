using System;

namespace WebApplication5.Framework.Abstractions
{
    public interface IProgressDataProvider
    {
        void Set(String taskId, String progress, Int32 durationInSeconds=5);
        String Get(String taskId);
        void Abort(String taskId);
        Boolean ShouldTerminate(String taskId);
    }
}