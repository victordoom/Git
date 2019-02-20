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
        private List<OpportunitiesOnline> OpportunitiesOnline = new List<OpportunitiesOnline>();

        [HttpGet("getconsulta/{user}")]
        public List<consulta> GetConsulta(int user)
        {
            string Command;
            if (user == 0)
            {
                Command = "select dbo.Opp_getHowFoundName(HowFoundID)HowFound, count(*) cases from opportunities where (VisitedDate>= DATEADD (dd , - 7 , GETDATE() )   AND VisitedDate <= GETDATE()) group by HowFoundID";
            } else
            {
                Command = "select dbo.Opp_getHowFoundName(HowFoundID)HowFound, count(*) cases from opportunities where (VisitedDate>= DATEADD (dd , - 7 , GETDATE() )   AND VisitedDate <= GETDATE()) and UserID = @User group by HowFoundID";
            }
             
            
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand query = new SqlCommand(Command, sqlConnection);

                if (user != 0)
                {
                    query.Parameters.AddWithValue("@User", user);
                }

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

        [HttpGet("getdataset/{user}")]
        public List<consulta> GetDataset(int user)
        {
            string Command;
            if (user == 0)
            {
                Command = "select dbo.Opp_getCategoryName(categoryID) category, count(*) cases from opportunities where (VisitedDate>= DATEADD (dd , - 7 , GETDATE() )   AND VisitedDate <= GETDATE()) group by categoryID";
            }
            else
            {
                Command = "select dbo.Opp_getCategoryName(categoryID) category, count(*) cases from opportunities where (VisitedDate>= DATEADD (dd , - 7 , GETDATE() )   AND VisitedDate <= GETDATE()) and UserID = @User group by categoryID";
            }

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand query = new SqlCommand(Command, sqlConnection);

                if (user != 0)
                {
                    query.Parameters.AddWithValue("@User", user);
                }

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
                SqlCommand query = new SqlCommand("select dbo.seg_nomUsuario(userID) AssignedTo, count(*) cases from opportunities where (VisitedDate>= DATEADD (dd ,  - 7 , GETDATE() )   AND VisitedDate <= GETDATE()) group by userID", sqlConnection);
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
                SqlCommand query = new SqlCommand("SELECT   ltrim(rtrim(substring(H.keyDate,1,5))) date, sum(isnull(D.cases, 0)) cases FROM  vw_last7days AS H LEFT OUTER JOIN  vw_SalesLast7day AS D ON H.keyDate = D.date group by  h.keydate order by h.keydate", sqlConnection);
                query.CommandType = System.Data.CommandType.Text;
                int result = query.ExecuteNonQuery();


                using (SqlDataReader reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Usuario
                        var consul = new consulta
                        {
                            date = (reader["date"]).ToString(),
                            
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

        [HttpGet("GetOpportunitiesOnline")]
        public List<OpportunitiesOnline> GetOpportunitiesOnline()
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand query = new SqlCommand("select top 20 id, dbo.seg_nomUsuario(userID) salesman, VisitedDate date, NumberLeadToFollowUp, PhoneNumber, EmailAddress, rating, dbo.prg_getProgramName(programID) ProgramName," + 
                    " CompanyName company, ltrim(rtrim(address)) + '. ' + ltrim(rtrim(city)) + ' ' + ltrim(rtrim(state)) + ' ' + isnull(ltrim(rtrim(zipcode)), '') nlocation, 1 cases, dbo.Opp_getHowFoundName(howfoundid) HowFoundName," + 
                    " dbo.Opp_verifyLeadOnlinetmp(id, 1) vrfd1, dbo.Opp_verifyLeadOnlinetmp(id, 2) vrfd2, dbo.Opp_verifyLeadOnlinetmp(id, 3) vrfd3, dbo.Opp_verifyLeadOnlinetmp(id, 4) vrfd4, dbo.Opp_verifyLeadOnlinetmp(id, 5) vrfd5, dbo.Opp_GetLastUpdate(id) LastFollowup from " + 
                    " opportunities where (VisitedDate >= DATEADD(dd, -7, GETDATE())   AND VisitedDate <= GETDATE()) and howfoundid in (10) order by VisitedDate DESC, CompanyName", sqlConnection);
                query.CommandType = System.Data.CommandType.Text;
                int result = query.ExecuteNonQuery();


                using (SqlDataReader reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Usuario
                        var consul = new OpportunitiesOnline
                        {
                            salesman = (reader["salesman"]).ToString(),
                            date = Convert.ToDateTime(reader["date"]).ToString("dd/MM/yyyy"),
                            rating = (reader["rating"]).ToString(),
                            ProgramName = (reader["ProgramName"]).ToString(),
                            company = (reader["company"]).ToString(),
                            HowFoundName = (reader["HowFoundName"]).ToString(),
                            vrfd1 = (reader["vrfd1"]).ToString(),
                            vrfd2 = (reader["vrfd2"]).ToString(),
                            vrfd3 = (reader["vrfd3"]).ToString(),
                            vrfd4 = (reader["vrfd4"]).ToString(),
                            vrfd5 = (reader["vrfd5"]).ToString(),
                            LastFollowup = (reader["LastFollowup"]).ToString(),
                            NumberLead = (reader["NumberLeadToFollowUp"]).ToString(),
                            PhoneNumber = (reader["PhoneNumber"]).ToString(),
                            email = (reader["EmailAddress"]).ToString(),



                        };

                        OpportunitiesOnline.Add(consul);

                    }
                }
            }
            return OpportunitiesOnline;
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

    public class OpportunitiesOnline
    {
        public string salesman { get; set; }
        public string date { get; set; }
        public string rating { get; set; }
        public string ProgramName { get; set; }
        public string company { get; set; }
        public string HowFoundName { get; set; }
        public string vrfd1 { get; set; }
        public string vrfd2 { get; set; }
        public string vrfd3 { get; set; }
        public string vrfd4 { get; set; }
        public string vrfd5 { get; set; }
        public string LastFollowup { get; set; }
        public string NumberLead { get; set; }
        public string PhoneNumber { get; set; }
        public string email { get; set; }
    }


}