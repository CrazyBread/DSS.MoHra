using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DSS.MoHra.Models
{
    public static class DBExtensions
    {
        public static int GetNextQuestion(this DataContext db, int userSessionId)
        {
            return db.Database.SqlQuery<int>("SELECT dss.GetNextQuestion(@userSessionId)", new SqlParameter("@userSessionId", userSessionId)).First();
        }
    }
}