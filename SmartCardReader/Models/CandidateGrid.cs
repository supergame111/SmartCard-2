using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartCardReader.DAL
{
    [Table("CandidateGrid")]
    public class CandidateGrid
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Office { get; set; }
        public string PrimaryPhone { get; set; }
        public string Status { get; set; }
        public string City { get; set; }
        public DateTime? ActionCreatedOn { get; set; }
    }
}