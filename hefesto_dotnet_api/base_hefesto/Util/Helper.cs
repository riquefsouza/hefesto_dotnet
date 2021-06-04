using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using hefesto.admin.Models;

/*
public class TopUser
{
    public string Name { get; set; }

    public int Count { get; set; }
}

var result = Helper.RawSqlQuery(
    "SELECT TOP 10 Name, COUNT(*) FROM Users U"
    + " INNER JOIN Signups S ON U.UserId = S.UserId"
    + " GROUP BY U.Name ORDER BY COUNT(*) DESC",
    x => new TopUser { Name = (string)x[0], Count = (int)x[1] });

result.ForEach(x => Console.WriteLine($"{x.Name,-25}{x.Count}"));
*/

namespace hefesto.base_hefesto.Util
{
    public static class Helper
    {
        public static List<T> RawSqlQuery<T>(dbhefestoContext context, string query, Func<DbDataReader, T> map)
        {
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                context.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    var entities = new List<T>();

                    while (result.Read())
                    {
                        entities.Add(map(result));
                    }

                    return entities;
                }
            }
        }
    }
}