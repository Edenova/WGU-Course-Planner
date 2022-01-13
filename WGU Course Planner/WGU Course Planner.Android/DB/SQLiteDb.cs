using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System.IO;
using Xamarin.Forms;
using WGU_Course_Planner.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(SQLiteDb))]
namespace WGU_Course_Planner.Droid
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteConnection GetConnection()
        {

            string dbName = "wgucourseplanner_db.sq3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, dbName);
            return new SQLiteConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex);
        }

        public SQLiteAsyncConnection GetConnectionAsync()
        {
            string dbName = "wgucourseplanner_db.sq3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); 
            var path = Path.Combine(documentsPath, dbName);
            return new SQLiteAsyncConnection(path);
        }
    }
}