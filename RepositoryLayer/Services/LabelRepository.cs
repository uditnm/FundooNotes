﻿using CommonLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.FundoContext;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class LabelRepository: ILabelRepository
    {
        private readonly FundoAppContext context;
        public LabelRepository(FundoAppContext context)
        {
            this.context = context;
        }

        public LabelEntity CreateLabel(LabelModel model, long UserId)
        {
            try
            {
                var checkUser = context.Notes.FirstOrDefault(x => x.UserId == UserId && x.NotesId == model.NoteId);
                if (checkUser != null)
                {
                    LabelEntity label = new LabelEntity();
                    label.LabelName = model.LabelName;
                    label.NoteId= model.NoteId;
                    label.UserId = UserId;
                    var check = context.Labels.Add(label);
                    context.SaveChanges();
                    if(check!= null)
                    {
                        return label;
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
    }
}