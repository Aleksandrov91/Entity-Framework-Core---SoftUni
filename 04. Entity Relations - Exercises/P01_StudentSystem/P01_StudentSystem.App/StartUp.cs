namespace P01_StudentSystem.App
{
    using System;

    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (StudentSystemContext context = new StudentSystemContext())
            {
                context.Database.EnsureDeleted();

                context.Database.Migrate();

                Student[] students = new[]
                {
                    new Student() { Name = "Pesho", PhoneNumber = "0884291562", Birthday = DateTime.Now },
                    new Student() { Name = "Gosho", PhoneNumber = "0883445210", Birthday = DateTime.Now },
                    new Student() { Name = "Ivan", PhoneNumber = "0898214367", Birthday = DateTime.Now },
                    new Student() { Name = "Stanislav", PhoneNumber = "0562341252", Birthday = DateTime.Now },
                    new Student() { Name = "Micnho", PhoneNumber = "0294827582", Birthday = DateTime.Now },
                    new Student() { Name = "Ani", PhoneNumber = "0994512634", Birthday = DateTime.Now },
                    new Student() { Name = "Dimitrinka", Birthday = DateTime.Now }
                };

                Course[] courses = new[]
                {
                    new Course()
                    {
                        Name = "C# OOP",
                        Description = "Best Course Ever",
                        StartDate = DateTime.Parse("2017-05-23"),
                        EndDate = DateTime.Parse("2017-08-20"),
                        Price = 490.23M
                    },
                    new Course()
                    {
                        Name = "Java DB",
                        Description = "nai gadniqt kurs",
                        StartDate = DateTime.Parse("2017-04-18"),
                        EndDate = DateTime.Parse("2017-09-17"),
                        Price = 600M
                    },
                    new Course()
                    {
                        Name = "JS Web",
                        StartDate = DateTime.Parse("2017-06-10"),
                        EndDate = DateTime.Parse("2017-09-25"),
                        Price = 350M
                    },
                    new Course()
                    {
                        Name = "Programming Fundamentals",
                        Description = "Fundamentals of programming with C#",
                        StartDate = DateTime.Parse("2017-02-05"),
                        EndDate = DateTime.Parse("2017-04-28"),
                        Price = 300M
                    }
                };

                Resource[] resources = new[]
                {
                    new Resource()
                    {
                        Name = "Arrays",
                        ResourceType = ResourceType.Presentation,
                        Course = courses[3],
                        Url = "softuni.bg/trainings/arrays.pptx"
                    },
                    new Resource()
                    {
                        Name = "Regex",
                        ResourceType = ResourceType.Document,
                        Course = courses[3],
                        Url = "regex.Bate"
                    },
                    new Resource()
                    {
                        Name = "Introduction in Angular 5",
                        ResourceType = ResourceType.Video,
                        Course = courses[2],
                        Url = "aa.bg/intr/ang/5"
                    },
                    new Resource()
                    {
                        Name = "Reflection",
                        ResourceType = ResourceType.Document,
                        Course = courses[0],
                        Url = "vinkel/bace"
                    },
                    new Resource()
                    {
                        Name = "domashno",
                        ResourceType = ResourceType.Other,
                        Course = courses[1],
                        Url = "nqkuflink"
                    },
                };

                Homework[] homeworks = new[]
                {
                    new Homework()
                    {
                        Content = "domashno",
                        ContentType = ContentType.Application,
                        Course = courses[1],
                        Student = students[2],
                        SubmissionTime = DateTime.Now
                    },
                    new Homework()
                    {
                        Content = "Generics Exercise",
                        ContentType = ContentType.Zip,
                        Course = courses[0],
                        Student = students[0],
                        SubmissionTime = DateTime.Now
                    },
                    new Homework()
                    {
                        Content = "Components in Angular",
                        ContentType = ContentType.Zip,
                        Course = courses[2],
                        Student = students[4],
                        SubmissionTime = DateTime.Now
                    },
                };

                StudentCourse[] studentsCourses = new[]
                {
                    new StudentCourse()
                    {
                        Course = courses[0],
                        Student = students[2]
                    },
                    new StudentCourse()
                    {
                        Course = courses[1],
                        Student = students[6]
                    },
                    new StudentCourse()
                    {
                        Course = courses[3],
                        Student = students[6]
                    },
                    new StudentCourse()
                    {
                        Course = courses[2],
                        Student = students[2]
                    },
                    new StudentCourse()
                    {
                        Course = courses[2],
                        Student = students[0]
                    },
                };

                context.AddRange(students);
                context.AddRange(courses);
                context.AddRange(resources);
                context.AddRange(homeworks);
                context.AddRange(studentsCourses);

                context.SaveChanges();
            }
        }
    }
}
