using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    public class NotesCreationModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Reminder { get; set; }
    }
}
