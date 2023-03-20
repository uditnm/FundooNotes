﻿using CommonLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.FundoContext;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RepositoryLayer.Services
{
    public class LabelRepository : ILabelRepository
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
                    label.NoteId = model.NoteId;
                    label.UserId = UserId;
                    var check = context.Labels.Add(label);
                    context.SaveChanges();
                    if (check != null)
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

        public List<LabelEntity> GetLabels(long UserId)
        {
            try
            {
                var checkUser = context.Labels.FirstOrDefault(x => x.UserId == UserId);
                if (checkUser != null)
                {
                    var labels = context.Labels.Where(x => x.UserId == UserId).ToList();
                    return labels;
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

        public List<LabelEntity> GetLabelsByNote(long NoteId, long UserId)
        {
            try
            {
                var checkUser = context.Labels.FirstOrDefault(x => x.UserId == UserId && x.NoteId == NoteId);
                if (checkUser != null)
                {
                    var labels = context.Labels.Where(x => x.NoteId == NoteId).ToList();
                    return labels;
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

        public bool DeleteLabel(int LabelId, long UserId)
        {
            try
            {
                var checkUser = context.Labels.FirstOrDefault(x => x.LabelId == LabelId && x.UserId == UserId);
                if (checkUser != null)
                {
                    context.Labels.Remove(checkUser);
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

        public LabelEntity EditLabel(int LabelId, long UserId, string LabelName)
        {
            try
            {
                var Label = context.Labels.FirstOrDefault(x => x.LabelId == LabelId && x.UserId == UserId);
                if(Label!= null)
                {
                    Label.LabelName = LabelName;
                    context.SaveChanges();
                    return Label;
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
