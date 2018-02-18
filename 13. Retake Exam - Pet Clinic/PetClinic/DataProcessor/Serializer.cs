namespace PetClinic.DataProcessor
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    using Data;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportAnimalsByOwnerPhoneNumber(PetClinicContext context, string phoneNumber)
        {
            var animalsByPhoneNumber = context.Animals
                .Where(a => a.Passport.OwnerPhoneNumber == phoneNumber)
                .Select(ap => new
                {
                    OwnerName = ap.Passport.OwnerName,
                    AnimalName = ap.Name,
                    Age = ap.Age,
                    SerialNumber = ap.PassportSerialNumber,
                    RegisteredOn = ap.Passport.RegistrationDate.ToString("dd-MM-yyyy")
                })
                .OrderBy(a => a.Age)
                .ThenBy(sn => sn.SerialNumber)
                .ToArray();

            var json = JsonConvert.SerializeObject(animalsByPhoneNumber);

            return json.ToString();
        }

        public static string ExportAllProcedures(PetClinicContext context)
        {
            var procedures = context.Procedures
                .OrderBy(d => d.DateTime)
                .ThenBy(p => p.Animal.PassportSerialNumber)
                .Select(p => new ExportDtos.ProcedureDto
                {
                    Passport = p.Animal.PassportSerialNumber,
                    OwnerNumber = p.Animal.Passport.OwnerPhoneNumber,
                    DateTime = p.DateTime.ToString("dd-MM-yyyy"),
                    AnimalAids = p.ProcedureAnimalAids.Select(ai => new ExportDtos.AnimalAidDto
                    {
                        Name = ai.AnimalAid.Name,
                        Price = ai.AnimalAid.Price
                    }).ToList(),
                    TotalPrice = p.ProcedureAnimalAids.Sum(ai => ai.AnimalAid.Price)
                })
                .ToList();

            var sb = new StringBuilder();
            var serializer = new XmlSerializer(typeof(List<ExportDtos.ProcedureDto>), new XmlRootAttribute("Procedures"));
            serializer.Serialize(new StringWriter(sb), procedures, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));

            var result = sb.ToString();
            return result;
        }
    }
}
