﻿using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface ILabelRepository
    {
        public LabelEntity CreateLabel(LabelModel model, long UserId);
        public List<LabelEntity> GetLabels(long UserId);
        public List<LabelEntity> GetLabelsByNote(long NoteId, long UserId);
        public bool DeleteLabel(int LabelId, long UserId);
        public LabelEntity EditLabel(int LabelId, long UserId, string LabelName);
    }
}
