using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaPasantia.Models
{
    public class Auto
    {
        [Key]
        public int IdAuto { get; set; }
        [RegularExpression(@"^\D{3}\d{3}$")]
        [Required]
        public string Patente { get; set; }
        [Required]
        [RegularExpression(@"^\D*$")]
        public string Marca { get; set; }
        [Required]
        public string Modelo { get; set; }
        [Required]
        public int Año { get; set; }
        [Required]
        public int Kms { get; set; }

        public string Foto1 { get; set; }
        [NotMapped]
        [Required]
        public IFormFile Foto1File { get; set; }

        public string Foto2 { get; set; }
        [NotMapped]
        
        public IFormFile Foto2File { get; set; }
    }
}
