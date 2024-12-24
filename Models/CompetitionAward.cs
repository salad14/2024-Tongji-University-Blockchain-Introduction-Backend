using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCompetitionAPI.Models
{
    [Table("CompetitionAwards")]
    public class CompetitionAward
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int StudentId { get; set; }

        public User User { get; set; }

        [Required]
        [MaxLength(200)]
        public string CompetitionName { get; set; }

        [MaxLength(50)]
        public string AwardLevel { get; set; }

        public DateTime AwardDate { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
