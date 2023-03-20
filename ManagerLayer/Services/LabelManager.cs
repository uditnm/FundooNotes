using CommonLayer.Models;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Services
{
    public class LabelManager: ILabelManager
    {
        private readonly ILabelRepository repository;
        public LabelManager(ILabelRepository repository)
        {
            this.repository = repository;
        }

        public LabelEntity CreateLabel(LabelModel model, long UserId)
        {
            return repository.CreateLabel(model, UserId);
        }
        public List<LabelEntity> GetLabels(long UserId)
        {
            return repository.GetLabels(UserId);
        }
        public List<LabelEntity> GetLabelsByNote(long NoteId, long UserId)
        {
            return repository.GetLabelsByNote(NoteId, UserId);
        }

        public bool DeleteLabel(int LabelId, long UserId)
        {
            return repository.DeleteLabel(LabelId, UserId);
        }
    }
}
