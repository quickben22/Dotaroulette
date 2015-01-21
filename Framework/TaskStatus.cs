using System;

namespace WebApplication5.Framework
{
    [Serializable]
    public class TaskStatus
    {
        public TaskStatus(String status) : this (status, false)
        {
        }
        public TaskStatus(String status, Boolean aborted)
        {
            Aborted = aborted;
            Status = status;
        }
        
        public String Status { get; set; }
        public Boolean Aborted { get; set; }
}
}