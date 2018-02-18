namespace PetClinic.DataProcessor
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Procedure")]
    public class ProcedureDto
    {
        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Vet { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Animal { get; set; }

        public string DateTime { get; set; }

        public List<AnimalAidDto> AnimalAids { get; set; } = new List<AnimalAidDto>();
    }
}
