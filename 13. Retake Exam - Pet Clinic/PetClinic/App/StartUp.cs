namespace PetClinic.App
{
    using System;
    using System.IO;

    using AutoMapper;
    using Data;

    public class StartUp
    {
        public static void Main()
        {
            using (var context = new PetClinicContext())
            {
                Mapper.Initialize(config => config.AddProfile<PetClinicProfile>());

                ResetDatabase(context);

                ImportEntities(context);

                ExportEntities(context);

                BonusTask(context);
            }
        }

        private static void ImportEntities(PetClinicContext context, string baseDir = @"..\Datasets\")
        {
            const string ExportDir = "./Results/";

            string animalAids = DataProcessor.Deserializer.ImportAnimalAids(context, File.ReadAllText(baseDir + "animalAids.json"));
            PrintAndExportEntityToFile(animalAids, ExportDir + "AnimalAidsImport.txt");

            string animals = DataProcessor.Deserializer.ImportAnimals(context, File.ReadAllText(baseDir + "animals.json"));
            PrintAndExportEntityToFile(animals, ExportDir + "AnimalsImport.txt");

            string vets = DataProcessor.Deserializer.ImportVets(context, File.ReadAllText(baseDir + "vets.xml"));
            PrintAndExportEntityToFile(vets, ExportDir + "VetsImport.txt");

            var procedures = DataProcessor.Deserializer.ImportProcedures(context, File.ReadAllText(baseDir + "procedures.xml"));
            PrintAndExportEntityToFile(procedures, ExportDir + "ProceduresImport.txt");
        }

        private static void ExportEntities(PetClinicContext context)
        {
            const string ExportDir = "./Results/";

            string animalsExport = DataProcessor.Serializer.ExportAnimalsByOwnerPhoneNumber(context, "0887446123");
            PrintAndExportEntityToFile(animalsExport, ExportDir + "AnimalsExport.json");

            string proceduresExport = DataProcessor.Serializer.ExportAllProcedures(context);
            PrintAndExportEntityToFile(proceduresExport, ExportDir + "ProceduresExport.xml");
        }

        private static void BonusTask(PetClinicContext context)
        {
            var bonusOutput = DataProcessor.Bonus.UpdateVetProfession(context, "+359284566778", "Primary Care");
            Console.WriteLine(bonusOutput);
        }

        private static void PrintAndExportEntityToFile(string entityOutput, string outputPath)
        {
            Console.WriteLine(entityOutput);
            File.WriteAllText(outputPath, entityOutput.TrimEnd());
        }

        private static void ResetDatabase(PetClinicContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            Console.WriteLine("Database reset.");
        }
    }
}
