using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCardReader.Models
{
    public class IdentityCard
    {
        public string CardNumber { get; set; }
        public string Surname { get; set; }
        public string Firstnames { get; set; }
        public string Nationality { get; set; }
        public string BirthPlace { get; set; }
        public string BirthDay { get; set; }
        public string Gender { get; set; }
        public string Nobility { get; set; }
        public string Photohash { get; set; }
        public bool HasFamily { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }

    }
}