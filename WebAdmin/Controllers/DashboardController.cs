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
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : Controller
    {
        DBAdminContext db = new DBAdminContext();
        private readonly string connectionString = "Server=96.231.33.87;Initial Catalog=dbABposDEV;User ID=dev;Password=dev;MultipleActiveResultSets=true";
        private List<consulta> Consulta = new List<consulta>();
        private List<Goal> ConsulGoal = new List<Goal>();
        private List<ByCurrentMonth> ConsulBy = new List<ByCurrentMonth>();

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


        [HttpGet("getdatasetvisitedday")]
        public List<consulta> GetDatasetvisitedday()
        {

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand query = new SqlCommand("SELECT       ltrim(rtrim(substring(H.keyDate,1,5))) date, isnull(D.cases,0) cases FROM    vw_last7days AS H LEFT OUTER JOIN vw_SalesLast7day AS D ON H.keyDate = D.date order by h.keydate", sqlConnection);
                query.CommandType = System.Data.CommandType.Text;
                int result = query.ExecuteNonQuery();


                using (SqlDataReader reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Usuario
                        var consul = new consulta
                        {
                            date = Convert.ToDateTime(reader["date"]).ToString("dd/MM/yyyy"),
                            
                            //HowFoung = reader["HowFound"].ToString(),
                            //category = reader["category"].ToString(),
                            //AssignedTo = reader["AssignedTo"].ToString(),
                            cases = Convert.ToInt32(reader["cases"])
                        };

                        Consulta.Add(consul);

                    }
                }
            }
            return Consulta;
        }


        [HttpGet("getcurrentmonth")]
        public List<Goal> GetCurrentMonth()
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand query = new SqlCommand("SELECT      top 1  GoalID, GoalYear, GoalMonth, GoalQuantityNewContracts, (select count(*) cases from ContractHeader where year(CreationDate)=GoalYear  and month(CreationDate)=GoalMonth ) QuantityReal FROM  SalesGoals where Status=1 order by  GoalYear, GoalMonth desc", sqlConnection);
                query.CommandType = System.Data.CommandType.Text;
                int result = query.ExecuteNonQuery();


                using (SqlDataReader reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Usuario
                        var consul = new Goal
                        {
                            GoalID = Convert.ToInt32(reader["GoalID"]),
                            GoalYear = Convert.ToInt32(reader["GoalYear"]),
                            GoalMonth = Convert.ToInt32(reader["GoalMonth"]),
                            GoalNewContracts = Convert.ToInt32(reader["GoalQuantityNewContracts"]),
                            QuantityReal = Convert.ToInt32(reader["QuantityReal"]),

                            
                        };

                        ConsulGoal.Add(consul);

                    }
                }
            }
            return ConsulGoal;

        }
        
        [HttpGet("GetByCurrentMonth/{month}/{year}")]
        public List<ByCurrentMonth> GetByCurrentMonth(int month, int year)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand query = new SqlCommand("declare @CurrentYear int, @CurrentMonth int set @CurrentYear = @year set  @CurrentMonth = @month"+
                     " select NOMBRE_USUARIO + ' ' + APELLIDO_USUARIO SalesMan, (select count(*) from ContractHeader"+
                     " where month(CreationDate) = @CurrentMonth AND YEAR(CreationDate) = @CurrentYear and userid = SEG_USUARIOS.ID_USUARIO) cases "+
                     "from SEG_USUARIOS where ID_PERSONA in (select id from Employees where DepartmentID = 2 ) and ESTADO_USUARIO = 'A' order by SalesMan", sqlConnection);

                query.Parameters.AddWithValue("@month", month);
                query.Parameters.AddWithValue("@year", year);

                query.CommandType = System.Data.CommandType.Text;
                int result = query.ExecuteNonQuery();


                using (SqlDataReader reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Usuario
                        var consul = new ByCurrentMonth
                        {
                            SalesMan = (reader["SalesMan"]).ToString(),
                            cases = Convert.ToInt32(reader["cases"])
                            


                        };

                        ConsulBy.Add(consul);

                    }
                }
            }
            return ConsulBy;

        }





        [HttpGet("getcurrentmonthlast")]
        public List<Goal> GetCurrentMonthLast()
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand query = new SqlCommand("SELECT      top 1  GoalID, GoalYear, GoalMonth, GoalQuantityNewContracts, (select count(*) cases from ContractHeader where year(CreationDate)=GoalYear  and month(CreationDate)=GoalMonth ) QuantityReal FROM  SalesGoals where Status=0 order by  GoalYear, GoalMonth desc", sqlConnection);
                query.CommandType = System.Data.CommandType.Text;
                int result = query.ExecuteNonQuery();


                using (SqlDataReader reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Usuario
                        var consul = new Goal
                        {
                            GoalID = Convert.ToInt32(reader["GoalID"]),
                            GoalYear = Convert.ToInt32(reader["GoalYear"]),
                            GoalMonth = Convert.ToInt32(reader["GoalMonth"]),
                            GoalNewContracts = Convert.ToInt32(reader["GoalQuantityNewContracts"]),
                            QuantityReal = Convert.ToInt32(reader["QuantityReal"]),


                        };

                        ConsulGoal.Add(consul);

                    }
                }
            }
            return ConsulGoal;

        }

        [HttpGet("GetByCurrentMonthLast/{month}/{year}")]
        public List<ByCurrentMonth> GetByCurrentMonthLast(int month, int year)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand query = new SqlCommand("declare @LastYear int, @LastMonth int set @LastYear = @year set  @LastMonth = @month" +
                     " select NOMBRE_USUARIO + ' ' + APELLIDO_USUARIO SalesMan, (select count(*) from ContractHeader" +
                     " where month(CreationDate) = @LastMonth AND YEAR(CreationDate) = @LastYear and userid = SEG_USUARIOS.ID_USUARIO) cases " +
                     "from SEG_USUARIOS where ID_PERSONA in (select id from Employees where DepartmentID = 2 ) and ESTADO_USUARIO = 'A' order by SalesMan", sqlConnection);

                query.Parameters.AddWithValue("@month", month);
                query.Parameters.AddWithValue("@year", year);

                query.CommandType = System.Data.CommandType.Text;
                int result = query.ExecuteNonQuery();


                using (SqlDataReader reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Usuario
                        var consul = new ByCurrentMonth
                        {
                            SalesMan = (reader["SalesMan"]).ToString(),
                            cases = Convert.ToInt32(reader["cases"])



                        };

                        ConsulBy.Add(consul);

                    }
                }
            }
            return ConsulBy;

        }

    }

    public class consulta
    {
        public string date { get; set; }
        public string HowFoung { get; set; }
        public string category { get; set; }
        public string AssignedTo { get; set; }
        public int cases { get; set; }
    }

    public class Goal
    {
        public int GoalID { get; set; }
        public int GoalYear { get; set; }
        public int GoalMonth { get; set; }
        public int GoalNewContracts { get; set; }
        public int QuantityReal { get; set; }
    }

    public class ByCurrentMonth
    {
        public string SalesMan { get; set; }
        public int cases { get; set; }
    }


}