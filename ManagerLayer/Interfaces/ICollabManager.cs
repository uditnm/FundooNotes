using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Interfaces
{
    public interface ICollabManager
    {
        public CollaboratorEntity AddCollab(CollabModel collab, long UserId);
        public List<CollaboratorEntity> GetAllCollaborators(long UserId);
        public bool DeleteCollaborator(long UserId, int CollabId);
    }
}
