using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : Controller
    {
        DBAdminContext db = new DBAdminContext();
        private readonly string connectionString = "Server=96.231.33.87;Initial Catalog=dbABposDEV;User ID=dev;Password=dev;MultipleActiveResultSets=true";
        private List<consulta> Consulta = new List<consulta>();

        [HttpGet("getconsulta")]
        public List<consulta> GetConsulta()
        {

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand query = new SqlCommand("select dbo.Opp_getHowFoundName(HowFoundID)HowFound, count(*) cases from opportunities where (VisitedDate>= DATEADD (dd , - 60 , GETDATE() )   AND VisitedDate <= GETDATE()) group by HowFoundID", sqlConnection);
                query.CommandType = System.Data.CommandType.Text;
                int result = query.ExecuteNonQuery();


                using (SqlDataReader reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Usuario
                        var consul = new consulta
                        {
                           // date = Convert.ToDateTime(reader["date"]),
                            HowFoung = reader["HowFound"].ToString(),
                          //  category = reader["category"].ToString(),
                          //  AssignedTo = reader["AssignedTo"].ToString(),
                            cases = Convert.ToInt32(reader["cases"])
                        };

                        Consulta.Add(consul);

                    }
                }
            }
            return Consulta;
        }

        [HttpGet("getdataset")]
        public List<consulta> GetDataset()
        {

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand query = new SqlCommand("select dbo.Opp_getCategoryName(categoryID) category, count(*) cases from opportunities where (VisitedDate>= DATEADD (dd , - 60 , GETDATE() )   AND VisitedDate <= GETDATE()) group by categoryID", sqlConnection);
                query.CommandType = System.Data.CommandType.Text;
                int result = query.ExecuteNonQuery();


                using (SqlDataReader reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Usuario
                        var consul = new consulta
                        {
                           // date = Convert.ToDateTime(reader["date"]),
                          //  HowFoung = reader["HowFound"].ToString(),
                            category = reader["category"].ToString(),
                          //  AssignedTo = reader["AssignedTo"].ToString(),
                            cases = Convert.ToInt32(reader["cases"])
                        };

                        Consulta.Add(consul);

                    }
                }
            }
            return Consulta;
        }


        [HttpGet("getdatasetvisited")]
        public List<consulta> GetDatasetvisited()
        {

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand query = new SqlCommand("select dbo.seg_nomUsuario(userID) AssignedTo, count(*) cases from opportunities where (VisitedDate>= DATEADD (dd ,  - 30 , GETDATE() )   AND VisitedDate <= GETDATE()) group by userID", sqlConnection);
                query.CommandType = System.Data.CommandType.Text;
                int result = query.ExecuteNonQuery();


                using (SqlDataReader reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Usuario
                        var consul = new consulta
                        {
                            //date = Convert.ToDateTime(reader["date"]),
                            //HowFoung = reader["HowFound"].ToString(),
                            //category = reader["category"].ToString(),
                            AssignedTo = reader["AssignedTo"].ToString(),
                            cases = Convert.ToInt32(reader["cases"])
                        };

                        Consulta.Add(consul);

                    }
                }
            }
            return Consulta;
        }

    }

    public class consulta
    {
        public DateTime date { get; set; }
        public string HowFoung { get; set; }
        public string category { get; set; }
        public string AssignedTo { get; set; }
        public int cases { get; set; }
    }


}