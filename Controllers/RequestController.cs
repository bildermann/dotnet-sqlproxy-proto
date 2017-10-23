using Microsoft.AspNetCore.Mvc;
using dotnet_sandbox.Models;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SqlKata;
using SqlKata.Compilers;
using System;


namespace dotnet_sandbox.Controllers
{

    [Route("api/[controller]")]
    public class RequestController : Controller
    {

        [HttpPost()]
        public IActionResult Post([FromBody]SQLRequest req)
        {

            var conn = new MySqlConnection("server=192.168.99.100;port=3306;user=root;password=root;database=employees");
            var cmd = new MySqlCommand("", conn);

            // var compiler = new SqlServerCompiler();
            var compiler = new MySqlCompiler();
            var q = new Query(req.From);
            var paramCounter = 0;

            foreach (var literal in req.Where)
            {
                var val = literal.Values[0];
                q.Where(
                    literal.FieldName,
                    literal.Operator,
                    val.Value);
                // var p = cmd.CreateParameter();
                // p.Value = val.Value;
                // p.ParameterName = "p" + paramCounter;

                cmd.Parameters.AddWithValue("@p" + paramCounter, val.Value);
                paramCounter++;
                // .Value = val.Value
            }

            q.Limit(req.Limit);
            q.Offset(req.Offset);

            cmd.Parameters.AddWithValue("@p" + paramCounter, req.Limit);
            paramCounter++;
            if (req.Offset > 0)
            {
                cmd.Parameters.AddWithValue("@p" + paramCounter, req.Offset);
                paramCounter++;
            }

            var sql = compiler.Compile(q).Sql;

            Console.Write(sql);

            cmd.CommandText = sql;
            conn.Open();
            var resp = cmd.ExecuteReader();

            var responseList = new List<Dictionary<string, object>>();

            while (resp.Read())
            {
                var rowDict = new Dictionary<string, object>();
                for (var i = 0; i < resp.FieldCount; i++)
                {
                    rowDict.Add(resp.GetName(i), resp[i]);
                }
                responseList.Add(rowDict);
            }

            conn.Close();

            return Ok(new SQLResponse()
            {
                ResultSet = responseList,
                Next = new SQLRequest()
                {
                    Select = req.Select,
                    From = req.From,
                    Where = req.Where,
                    OrderBy = req.OrderBy,
                    Limit = req.Limit,
                    Offset = req.Offset + req.Limit
                },
                Page = 1,
                Pages = 10,
                Count = 100
            });
        }

    }

}