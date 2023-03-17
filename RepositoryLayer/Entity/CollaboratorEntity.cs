using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class CollaboratorEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CollaboratorId { get; set; }
        public string collabMail { get; set; }

        [ForeignKey("NotesEntity")]
        public long NotesId { get; set; }
        public NotesEntity NotesEntity { get; set; }

        [ForeignKey("UserEntity")]
        public long UserId { get; set; }
        public UserEntity UserEntity { get; set; }

    }
}
