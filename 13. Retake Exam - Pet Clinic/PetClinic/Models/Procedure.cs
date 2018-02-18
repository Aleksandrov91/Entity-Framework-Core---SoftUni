namespace PetClinic.Models
{
    using System;
    using System.Collections.Generic;

    public class Procedure
    {
        public int Id { get; set; }

        public int AnimalId { get; set; }

        public Animal Animal { get; set; }

        public int VetId { get; set; }

        public Vet Vet { get; set; }

        public List<ProcedureAnimalAid> ProcedureAnimalAids { get; set; } = new List<ProcedureAnimalAid>();

        public decimal Cost { get; set; }

        public DateTime DateTime { get; set; }
    }
}