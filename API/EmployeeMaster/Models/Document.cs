using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeMaster.Models
{
    public class Document
    {
        public int UserId { get; set; }
        public int DocTypeId { get; set; }
        public string DocumentPath { get; set; }
        public int DocId { get; set; }
    }
}