using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BestNox.Models
{
    public class MyTest
    {
        public int Id { get; set; }

        [MaxLength(45)]
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
