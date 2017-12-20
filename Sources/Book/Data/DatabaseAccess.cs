using AlphaCodex.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace AlphaCodex.Data
{
    public partial class DatabaseAccess
    {
        /// <summary>
        /// Retrieve the list of books from the DB
        /// </summary>
        public List<Book> GetBooks()
        {
            SQLiteCommand command = new SQLiteCommand("SELECT *, CASE WHEN id BETWEEN 1 AND 3 THEN 0 ELSE 1 END AS Ordering FROM 'book' ORDER BY Ordering, name");

            return ExecuteQuery(command, BooksHandler) as List<Book>;
        }

        /// <summary>
        /// Retrieve the list of books from the DB based on the content needed
        /// </summary>
        public List<Book> GetBooks(string category)
        {
            SQLiteCommand command = new SQLiteCommand(
                "SELECT book.*, CASE WHEN book.id BETWEEN 1 AND 3 THEN 0 ELSE 1 END AS Ordering, COUNT(feat.id) as number " +
                "FROM book LEFT JOIN " + category + " ON book.id = " + category + ".book_id" +
                " GROUP BY book.id HAVING number > 0 ORDER BY Ordering, book.name");

            return ExecuteQuery(command, BooksHandler) as List<Book>;
        }

        /// <summary>
        /// Handle the books data fetched from the database
        /// </summary>
        /// <returns></returns>
        private Object BooksHandler()
        {
            List<Book> Books = new List<Book>();
            while (reader.Read())
            {
                Books.Add(new Book(reader));
            }
            System.Diagnostics.Debug.WriteLine("[General] Books Fetched");
            return Books;
        }
    }
}
