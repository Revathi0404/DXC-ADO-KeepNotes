using System;
using System.Data;
using System.Data.SqlClient;

namespace KeepNote_Database
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection("Data Source=IN-4LSQ8S3; Initial Catalog=TakeNoteDB; Integrated Security=true");

            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Notes", connection);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "Notes");

            while (true)
            {
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Create Note");
                Console.WriteLine("2. View Note");
                Console.WriteLine("3. View All Notes");
                Console.WriteLine("4. Update Note");
                Console.WriteLine("5. Delete Note");

                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Wrong Choice Entered");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        CreateNote createNote = new CreateNote();
                        createNote.Create(connection, dataSet, adapter);
                        break;
                    case 2:
                        ViewNote viewNote = new ViewNote();
                        viewNote.View(connection, dataSet);
                        break;
                    case 3:
                        ViewAllNotes viewAllNotes = new ViewAllNotes();
                        viewAllNotes.ViewAll(connection, dataSet);
                        break;
                    case 4:
                        UpdateNote updateNote = new UpdateNote();
                        updateNote.Update(connection, dataSet, adapter);
                        break;
                    case 5:
                        DeleteNote deleteNote = new DeleteNote();
                        deleteNote.Delete(connection, dataSet, adapter);
                        break;
                    default:
                        Console.WriteLine("Wrong Choice Entered");
                        break;
                }

                Console.WriteLine();
            }
        }
    }

    public class CreateNote
    {
        public void Create(SqlConnection connection, DataSet dataSet, SqlDataAdapter adapter)
        {
            DataTable notesTable = dataSet.Tables["Notes"];

            DataRow newRow = notesTable.NewRow();

            Console.Write("Enter the title: ");
            string title = Console.ReadLine();
            newRow["Title"] = title;

            Console.Write("Enter the description: ");
            string description = Console.ReadLine();
            newRow["Description"] = description;

            newRow["Date"] = DateTime.Now;

            notesTable.Rows.Add(newRow);

            adapter.Update(dataSet, "Notes");

            Console.WriteLine("Note Created Successfully.");
        }
    }

    public class ViewNote
    {
        public void View(SqlConnection connection, DataSet dataSet)
        {
            Console.Write("Enter the id of the note to view: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid id format.");
                return;
            }

            DataTable notesTable = dataSet.Tables["Notes"];

            DataRow row = notesTable.Rows.Find(id);

            if (row == null)
            {
                Console.WriteLine("Note with id '{0}' does not exist.", id);
            }
            else
            {
                Console.WriteLine("Id: {0}", row["Id"]);
                Console.WriteLine("Title: {0}", row["Title"]);
                Console.WriteLine("Description: {0}", row["Description"]);
                Console.WriteLine("Date: {0}", row["Date"]);
            }
        }
    }

    public class ViewAllNotes
    {
        public void ViewAll(SqlConnection connection, DataSet dataSet)
        {
            DataTable notesTable = dataSet.Tables["Notes"];

            Console.WriteLine("All Notes:");
            Console.WriteLine("Id\tTitle\t\t\tDescription\t\t\tDate");
            Console.WriteLine("----------");

            foreach (DataRow row in notesTable.Rows)
            {
                Console.WriteLine($"{row["Id"]}\t{row["Title"].ToString().PadRight(24)}{row["Description"].ToString().PadRight(24)}{row["DateCreated"].ToString().PadRight(24)}");
            }

            Console.WriteLine();
        }
    }
    public class UpdateNote
    {
        public void Update(SqlConnection connection, DataSet dataSet, SqlDataAdapter adapter)
        {
            Console.Write("Enter the id of the note to update: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid id format.");
                return;
            }

            DataTable notesTable = dataSet.Tables["Notes"];

            DataRow row = notesTable.Rows.Find(id);

            if (row == null)
            {
                Console.WriteLine("Note with id '{0}' does not exist.", id);
                return;
            }

            Console.WriteLine("Enter new values (leave empty for no change):");

            Console.Write("Title ({0}): ", row["Title"]);
            string title = Console.ReadLine();
            if (!string.IsNullOrEmpty(title))
            {
                row["Title"] = title;
            }

            Console.Write("Description ({0}): ", row["Description"]);
            string description = Console.ReadLine();
            if (!string.IsNullOrEmpty(description))
            {
                row["Description"] = description;
            }

            adapter.Update(dataSet, "Notes");

            Console.WriteLine("Note Updated Successfully.");
        }
    }

    public class DeleteNote
    {
        public void Delete(SqlConnection connection, DataSet dataSet, SqlDataAdapter adapter)
        {
            Console.Write("Enter the id of the note to delete: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid id format.");
                return;
            }

            DataTable notesTable = dataSet.Tables["Notes"];

            DataRow row = notesTable.Rows.Find(id);

            if (row == null)
            {
                Console.WriteLine("Note with id '{0}' does not exist.", id);
                return;
            }

            row.Delete();

            adapter.Update(dataSet, "Notes");

            Console.WriteLine("Note Deleted Successfully.");
        }
    }
}