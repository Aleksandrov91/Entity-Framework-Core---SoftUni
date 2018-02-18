namespace BookShop
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Z.EntityFramework.Plus;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                DbInitializer.ResetDatabase(db);

                int removedBooks = RemoveBooks(db);

                Console.WriteLine($"{removedBooks} books were deleted");
            }
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            AgeRestriction restriction = (AgeRestriction)Enum.Parse(typeof(AgeRestriction), command, true);

            string[] books = context.Books
                .Where(b => b.AgeRestriction == restriction)
                .Select(b => b.Title)
                .OrderBy(x => x)
                .ToArray();

            return string.Join("\r\n", books);
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            string[] goldenBooks = context.Books
                                          .Where(b => b.Copies < 5000 && b.EditionType == EditionType.Gold)
                                          .OrderBy(b => b.BookId)
                                          .Select(b => b.Title)                                          
                                          .ToArray();

            return string.Join("\r\n", goldenBooks);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            string[] booksWithPrice = context.Books
                                             .Where(b => b.Price > 40)
                                             .OrderByDescending(b => b.Price)
                                             .Select(b => $"{b.Title} - ${b.Price:F2}")
                                             .ToArray();

            return string.Join("\r\n", booksWithPrice);
        }

        public static string GetBooksNotRealeasedIn(BookShopContext context, int year)
        {
            string[] books = context.Books
                                  .Where(b => b.ReleaseDate.Value.Year != year)
                                  .OrderBy(b => b.BookId)
                                  .Select(b => b.Title)
                                  .ToArray();

            return string.Join("\r\n", books);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                                       .Select(i => i.ToLower())
                                       .ToArray();

            var da = context.Books
                .Include(b => b.BookCategories)
                .ThenInclude(c => c.Category)
                .Where(bc => bc.BookCategories.All(c => categories.Contains(c.Category.Name.ToLower())))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            return string.Join("\r\n", da);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime releaseDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            string[] books = context.Books
                                  .Where(b => b.ReleaseDate < releaseDate)
                                  .OrderByDescending(b => b.ReleaseDate)
                                  .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:F2}")
                                  .ToArray();

            return string.Join("\r\n", books);
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            string[] authors = context.Authors
                                      .Where(a => a.FirstName.EndsWith(input))
                                      .Select(a => $"{a.FirstName} {a.LastName}")
                                      .OrderBy(a => a)
                                      .ToArray();

            return string.Join("\r\n", authors);
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            string[] books = context.Books
                                    .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                                    .Select(b => b.Title)
                                    .OrderBy(b => b)
                                    .ToArray();

            return string.Join("\r\n", books);
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            string[] booksByAuthor = context.Books
                                            .Where(a => a.Author.LastName.ToLower().StartsWith(input.ToLower()))
                                            .OrderBy(b => b.BookId)
                                            .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})")
                                            .ToArray();

            return string.Join("\r\n", booksByAuthor);
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            int booksCount = context.Books
                                    .Where(b => b.Title.Length > lengthCheck)
                                    .Count();

            return booksCount;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authorBooksCount = context.Authors
                                               .Include(b => b.Books)
                                               .Select(a => new
                                               {
                                                   Author = $"{a.FirstName} {a.LastName}",
                                                   BooksCount = a.Books.Sum(b => b.Copies)
                                               })
                                               .OrderByDescending(b => b.BooksCount)
                                               .ToArray();

            return string.Join("\r\n", authorBooksCount.Select(a => $"{a.Author} - {a.BooksCount}"));
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var profitByCategory = context.Categories
                                          .Include(bc => bc.CategoryBooks)
                                          .ThenInclude(b => b.Book)
                                          .Select(p => new
                                          {
                                              CategoryName = p.Name,
                                              Profit = p.CategoryBooks.Sum(b => b.Book.Price * b.Book.Copies)
                                          })
                                          .OrderByDescending(p => p.Profit)
                                          .ThenBy(c => c.CategoryName)
                                          .ToArray();

            return string.Join(Environment.NewLine, profitByCategory.Select(p => $"{p.CategoryName} ${p.Profit:F2}"));
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categoriesWithBooks = context.Categories
                                         .Include(cb => cb.CategoryBooks)
                                         .ThenInclude(b => b.Book)
                                         .OrderBy(c => c.CategoryBooks.Count)
                                         .ThenBy(c => c.Name)
                                         .Select(c => new
                                         {
                                             Name = c.Name,
                                             Books = c.CategoryBooks
                                                      .OrderByDescending(b => b.Book.ReleaseDate)
                                                      .Select(b => $"{b.Book.Title} ({b.Book.ReleaseDate.Value.Year})")
                                                      .Take(3)
                                                      .ToArray()
                                         })
                                         .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var category in categoriesWithBooks)
            {
                sb.AppendLine($"--{category.Name}");

                foreach (var book in category.Books)
                {
                    sb.AppendLine(book);
                }
            }

            return sb.ToString();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            ICollection<Book> booksToUpdate = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToList();

            foreach (var book in booksToUpdate)
            {
                book.Price += 5;
            }

            context.Books.UpdateRange(booksToUpdate);
            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            int deletedBooksCount = context.Books
                .Where(b => b.Copies < 4200)
                .Delete();

            return deletedBooksCount;
        }
    }
}
