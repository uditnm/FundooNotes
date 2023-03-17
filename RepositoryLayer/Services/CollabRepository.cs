using CommonLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.FundoContext;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class CollabRepository: ICollabRepository
    {
        private readonly FundoAppContext context;

        public CollabRepository(FundoAppContext context)
        {
            this.context = context;
        }

        public CollaboratorEntity AddCollab(CollabModel collab, long UserId)
        {
            try
            {
                var checkUser = context.Notes.FirstOrDefault(x => x.UserId == UserId && x.NotesId==collab.NotesId);
                if (checkUser != null)
                {
                    CollaboratorEntity collaboratorEntity = new CollaboratorEntity();
                    collaboratorEntity.UserId = UserId;
                    collaboratorEntity.collabMail = collab.collabMail;
                    collaboratorEntity.NotesId = collab.NotesId;
                    var check = context.Collaborators.Add(collaboratorEntity);
                    context.SaveChanges();
                    if (check != null)
                    {
                        return collaboratorEntity;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CollaboratorEntity> GetAllCollaborators(long UserId)
        {
            try
            {
                var collabs = context.Collaborators.Where(x => x.UserId == UserId).ToList();
                if (collabs.Count > 0)
                {
                    return collabs;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool DeleteCollaborator(long UserId, int CollabId)
        {
            try
            {
                var checkCollab = context.Collaborators.FirstOrDefault(o=>o.UserId== UserId && o.CollaboratorId== CollabId);
                if (checkCollab != null)
                {
                    context.Collaborators.Remove(checkCollab);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
