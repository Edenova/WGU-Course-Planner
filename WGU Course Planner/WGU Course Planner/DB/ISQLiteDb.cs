using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WGU_Course_Planner
{
    //database connection
    public interface ISQLiteDb
    {
        SQLiteConnection GetConnection();
        SQLiteAsyncConnection GetConnectionAsync();
    }
}
