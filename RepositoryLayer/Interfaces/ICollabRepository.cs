using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface ICollabRepository
    {
        public CollaboratorEntity AddCollab(CollabModel collab, long UserId);
        public List<CollaboratorEntity> GetAllCollaborators(long UserId);
        public bool DeleteCollaborator(long UserId, int CollabId);
    }
}
