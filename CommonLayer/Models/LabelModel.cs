using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CommonLayer.Models
{
    public class LabelModel
    {
        public string LabelName { get; set; }
        public long NoteId { get; set; }
    }
}
