using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.IO;

namespace DBLibrary
{
    public class DBWriter
    {
        public static String LogFile { get; set; }
        private static TextWriter LogWriter;
        private static int Iterator;
        private static SqlConnection conn;

        public static void Initialize(){
            Iterator = 0;
            
            try
            {
                LogWriter = new StreamWriter(LogFile);
                LogWriter.WriteLine("=================================================");
                LogWriter.WriteLine("Generated " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "\n");
                LogWriter.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open logfile:"+e.ToString());
            }
            if ((conn = CreateSqlConnection()) == null)
            {
                Console.WriteLine("cannot open sql connection");
            }
        }

        public static SqlConnection CreateSqlConnection()
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(@"Data Source=C:\Users\bkt\Desktop\explorer\ExplorerService\DBLibrary\explorer_db.sdf");
                connection.Open();
            }
            catch (Exception ex)
            {
                //ex should be written into a error log
                LogWriter.WriteLine(ex.ToString());
                // dispose of the connection to avoid connections leak
                if (connection != null)
                {
                    connection.Dispose();
                }
            }
            return connection;
        }

        public static MyFile ListDrive(string drive)
        {
            DirectoryInfo di = new DirectoryInfo(@drive);
            MyFile root = new MyFile(Iterator, drive, 1, -1); 
            SqlCommand myCommand = new SqlCommand("INSERT INTO table (id, name,is_directory,parent) " +
                                     "Values (" + Iterator + "," + drive + ",1,-1)", conn);
            myCommand.ExecuteNonQuery();
            ListDriveRec(di, Iterator++);
            return root;
            
        }

        public static void ListDriveRec(DirectoryInfo dir, int parent)
        {
            // Subdirs
            try         // Avoid errors such as "Access Denied"
            {
                foreach (DirectoryInfo iInfo in dir.GetDirectories())
                {
                    MyFile file = new MyFile(Iterator++, iInfo.Name, 1, parent);
                    SqlCommand myCommand = new SqlCommand("INSERT INTO table (id, name,is_directory,parent) " +
                                     "Values (" + Iterator + "," + iInfo.Name + ",1,"+parent+ ")", conn);
                    myCommand.ExecuteNonQuery();
                    ListDriveRec(iInfo, Iterator);
                }
            }
            catch (Exception e)
            {
                LogWriter.WriteLine(e.ToString());
            }

            // Subfiles
            try         // Avoid errors such as "Access Denied"
            {
                foreach (FileInfo iInfo in dir.GetFiles())
                {
                    MyFile file = new MyFile(Iterator++, iInfo.Name, 0, parent);
                    SqlCommand myCommand = new SqlCommand("INSERT INTO table (id, name,is_directory,parent) " +
                                     "Values (" + Iterator + "," + iInfo.Name + ",0," + parent + ")", conn);
                    myCommand.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                LogWriter.WriteLine(e.ToString());
            }
        }

    }
}
