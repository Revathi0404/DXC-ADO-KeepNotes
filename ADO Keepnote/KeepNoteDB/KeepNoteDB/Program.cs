using System.Data;
using System.Data.SqlClient;
using System;

namespace KeepNoteDB
{ 
        class Program

        {
            static void Main(string[] args)

            {

            SqlConnection con = new SqlConnection("Data source =IN-4LSQ8S3; Initial Catalog = TakeNoteDB;Integrated Security=true");

            SqlDataAdapter adp = new SqlDataAdapter("select * from Note", con);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            while (true)
            {
                Console.WriteLine("Enter Your Choice:");
                Console.WriteLine("1. Create Note");
                Console.WriteLine("2. View Note");
                Console.WriteLine("3. View All Notes");
                Console.WriteLine("4. Update Note");
                Console.WriteLine("5. Delete Note");
                Console.WriteLine("6. Exit");

                int choice;
                int.TryParse(Console.ReadLine(), out choice);

                switch (choice)
                {
                    case 1:
                        CreateNote(ds, adp);
                        break;

                    case 2:
                        ViewNoteById(ds);
                        break;

                    case 3:
                        ViewAllNotes(ds);
                        break;

                    case 4:
                        UpdateNoteById(ds, adp);
                        break;

                    case 5:
                        DeleteNoteById(ds, adp);
                        break;

                    case 6:
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please enter a valid choice.");
                        break;
                }
            }
        }

        static void CreateNote(DataSet ds, SqlDataAdapter adp)
        {
            DataRow newRow = ds.Tables[0].NewRow();
            Console.WriteLine("Enter the title:");
            newRow["Title"] = Console.ReadLine();
            Console.WriteLine("Enter the description:");
            newRow["Description"] = Console.ReadLine();
            newRow["Date"] = DateTime.Now;
            ds.Tables[0].Rows.Add(newRow);
            SqlCommandBuilder builder = new SqlCommandBuilder(adp);
            adp.Update(ds);
            Console.WriteLine("Note Created Successfully!");
        }

        static void ViewNoteById(DataSet ds)
        {
            Console.WriteLine("Enter the ID of the Note:");
            int id;
            int.TryParse(Console.ReadLine(), out id);
            DataRow[] selectedRows = ds.Tables[0].Select("ID = " + id);
            if (selectedRows.Length > 0)
            {
                Console.WriteLine("ID: " + selectedRows[0]["ID"]);
                Console.WriteLine("Title: " + selectedRows[0]["Title"]);
                Console.WriteLine("Description: " + selectedRows[0]["Description"]);
                Console.WriteLine("Date: " + selectedRows[0]["Date"]);
            }
            else
            {
                Console.WriteLine("Note with ID " + id + " does not exist.");
            }
        }

        static void ViewAllNotes(DataSet ds)
        {
            Console.WriteLine("ID\tTitle\t\tDescription\t\tDate");
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                Console.WriteLine(dataRow["ID"] + "\t" + dataRow["Title"] + "\t" + dataRow["Description"] + "\t" + dataRow["Date"]);
            }
            Console.WriteLine("Total Notes: " + ds.Tables[0].Rows.Count);
        }

        static void UpdateNoteById(DataSet ds, SqlDataAdapter adp)
        {
            Console.WriteLine("Enter the ID of the Note to Update:");
            int noteId;
            int.TryParse(Console.ReadLine(), out noteId);
            DataRow[] updatedRows = ds.Tables[0].Select("ID = " + noteId);
            if (updatedRows.Length > 0)
            {
                Console.WriteLine("Enter the new title:");
                updatedRows[0]["Title"] = Console.ReadLine();
                Console.WriteLine("Enter the new description:");
                updatedRows[0]["Description"] = Console.ReadLine();
                updatedRows[0]["Date"] = DateTime.Now;
                SqlCommandBuilder builder = new SqlCommandBuilder(adp);
                adp.Update(ds);
                Console.WriteLine("Note Updated Successfully!");
            }
            else
            {
                Console.WriteLine("Note with ID " + noteId + " does not exist.");
            }
        }

        static void DeleteNoteById(DataSet ds, SqlDataAdapter adp)
        {
            Console.WriteLine("Enter the ID of the Note to Delete:");
            int noteId;
            int.TryParse(Console.ReadLine(), out noteId);
            DataRow[] deletedRows = ds.Tables[0].Select("ID = " + noteId);
            if (deletedRows.Length > 0)
            {
                deletedRows[0].Delete();
                SqlCommandBuilder builder = new SqlCommandBuilder(adp);
                adp.Update(ds);
                Console.WriteLine("Note Deleted Successfully!");
            }
            else
            {
                Console.WriteLine("Note with ID " + noteId + " does not exist.");
            }
        }
    }
}

