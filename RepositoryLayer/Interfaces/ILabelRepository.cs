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
    }
}