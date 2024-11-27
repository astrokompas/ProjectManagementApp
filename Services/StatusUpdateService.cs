using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.Services
{
    public class StatusUpdateEventArgs : EventArgs
    {
        public string EntityType { get; }  // "Employee" or "Equipment"
        public int EntityId { get; }
        public string NewStatus { get; }

        public StatusUpdateEventArgs(string entityType, int entityId, string newStatus)
        {
            EntityType = entityType;
            EntityId = entityId;
            NewStatus = newStatus;
        }
    }

    public interface IStatusUpdateService
    {
        event EventHandler<StatusUpdateEventArgs> StatusUpdated;
        void NotifyStatusUpdate(string entityType, int entityId, string newStatus);
    }

    public class StatusUpdateService : IStatusUpdateService
    {
        public event EventHandler<StatusUpdateEventArgs> StatusUpdated;

        public void NotifyStatusUpdate(string entityType, int entityId, string newStatus)
        {
            StatusUpdated?.Invoke(this, new StatusUpdateEventArgs(entityType, entityId, newStatus));
        }
    }
}