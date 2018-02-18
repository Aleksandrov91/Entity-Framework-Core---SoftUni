namespace PetClinic.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using Data;
    using Models;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Error: Invalid data.";

        public static string ImportAnimalAids(PetClinicContext context, string jsonString)
        {
            AnimalAid[] animalAids = JsonConvert.DeserializeObject<AnimalAid[]>(jsonString);
            List<AnimalAid> validAids = new List<AnimalAid>();
            StringBuilder sb = new StringBuilder();

            foreach (var animalAid in animalAids)
            {
                if (!IsValid(animalAid) || validAids.Any(a => a.Name == animalAid.Name))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                validAids.Add(animalAid);
                sb.AppendLine($"Record {animalAid.Name} successfully imported.");
            }

            string result = sb.ToString().Trim();

            context.AddRange(validAids);
            context.SaveChanges();

            return result;
        }

        public static string ImportAnimals(PetClinicContext context, string jsonString)
        {
            AnimalWithPassportDto[] animalsWithPassport = JsonConvert.DeserializeObject<AnimalWithPassportDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<Animal> validAnimals = new List<Animal>();
            List<Passport> validPasports = new List<Passport>();

            foreach (var animalDto in animalsWithPassport)
            {
                if (!IsValid(animalDto) || !IsValid(animalDto.Passport) || validPasports.Any(p => p.SerialNumber == animalDto.Passport.SerialNumber))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime registrationDate = DateTime.ParseExact(animalDto.Passport.RegistrationDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                Passport passport = new Passport
                {
                    SerialNumber = animalDto.Passport.SerialNumber,
                    OwnerName = animalDto.Passport.OwnerName,
                    OwnerPhoneNumber = animalDto.Passport.OwnerPhoneNumber,
                    RegistrationDate = registrationDate
                };

                Animal animal = new Animal
                {
                    Name = animalDto.Name,
                    Type = animalDto.Type,
                    Age = animalDto.Age,
                    Passport = passport
                };

                validAnimals.Add(animal);
                validPasports.Add(passport);

                sb.AppendLine($"Record {animal.Name} Passport №: {animal.Passport.SerialNumber} successfully imported.");
            }

            string result = sb.ToString().Trim();

            context.AddRange(validAnimals);
            context.SaveChanges();

            return result;
        }

        public static string ImportVets(PetClinicContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Vet[]), new XmlRootAttribute("Vets"));
            Vet[] deserializedVets = (Vet[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));
            List<Vet> validVets = new List<Vet>();
            StringBuilder sb = new StringBuilder();

            foreach (var vet in deserializedVets)
            {
                if (!IsValid(vet) || validVets.Any(v => v.PhoneNumber == vet.PhoneNumber))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                validVets.Add(vet);

                sb.AppendLine($"Record {vet.Name} successfully imported.");
            }

            string result = sb.ToString().Trim();

            context.AddRange(validVets);
            context.SaveChanges();

            return result;
        }

        public static string ImportProcedures(PetClinicContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ProcedureDto[]), new XmlRootAttribute("Procedures"));
            ProcedureDto[] deserializedProcedures = (ProcedureDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));

            Vet[] vets = context.Vets.ToArray();
            Animal[] animals = context.Animals.ToArray();
            AnimalAid[] animalAids = context.AnimalAids.ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var procedure in deserializedProcedures)
            {
                bool isAidInvalid = false;
                bool isAidsExists = procedure.AnimalAids.All(ai => animalAids.Any(ai2 => ai2.Name == ai.Name));

                if (!vets.Any(v => v.Name == procedure.Vet) ||
                    !animals.Any(a => a.PassportSerialNumber == procedure.Animal) || !isAidsExists)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime dateTime = DateTime.ParseExact(procedure.DateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                Procedure proc = new Procedure
                {
                    Animal = animals.Where(a => a.PassportSerialNumber == procedure.Animal).SingleOrDefault(),
                    Vet = vets.Where(v => v.Name == procedure.Vet).SingleOrDefault(),
                    DateTime = dateTime                    
                };

                List<ProcedureAnimalAid> validProcedureAids = new List<ProcedureAnimalAid>();

                foreach (var aid in procedure.AnimalAids)
                {
                    ProcedureAnimalAid procedureAnimalAid = new ProcedureAnimalAid
                    {
                        AnimalAid = animalAids.Where(ai => ai.Name == aid.Name).SingleOrDefault(),
                        Procedure = proc
                    };

                    if (validProcedureAids.Any(vai => vai.AnimalAid.Name == procedureAnimalAid.AnimalAid.Name && 
                        vai.Procedure == proc))
                    {
                        isAidInvalid = true;
                        sb.AppendLine(ErrorMessage);
                        break;
                    }

                    validProcedureAids.Add(procedureAnimalAid);
                }

                if (isAidInvalid)
                {
                    continue;
                }

                context.AddRange(validProcedureAids);
                context.SaveChanges();

                sb.AppendLine("Record successfully imported.");
            }

            string result = sb.ToString().Trim();

            return result;
        }

        private static bool IsValid(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);

            List<ValidationResult> validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}