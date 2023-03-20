using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class LabelEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LabelId { get; set; }
        public string LabelName { get; set; }
        [ForeignKey("NotesEntity")]
        public long NoteId { get; set; }
        public NotesEntity NotesEntity { get; set; }
        [ForeignKey("UserEntity")]
        public long UserId { get; set; }
        public UserEntity UserEntity { get; set; }
    }
}
