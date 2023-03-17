using CommonLayer.Models;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Services
{
    public class CollabManager: ICollabManager
    {
        private readonly ICollabRepository _repository;
        public CollabManager(ICollabRepository repository)
        {
            _repository = repository;
        }

        public CollaboratorEntity AddCollab(CollabModel collab, long UserId)
        {
            return _repository.AddCollab(collab, UserId);
        }

        public List<CollaboratorEntity> GetAllCollaborators(long UserId)
        {
            return _repository.GetAllCollaborators(UserId);
        }

        public bool DeleteCollaborator(long UserId, int CollabId)
        {
            return _repository.DeleteCollaborator(UserId, CollabId);
        }
    }
}
