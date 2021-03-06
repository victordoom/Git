﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebAdmin.Controllers
{
    public class ContractsController : Controller
    {
        public IConfiguration Configuration { get; private set; }
        public List<Contracts> Contract = new List<Contracts>();
        public ContractsController(IConfiguration con)
        {
            Configuration = con;
        }

        public IActionResult Index()
        {
            var lEmail = this.User.FindFirstValue(ClaimTypes.Name);

            ViewBag.User = lEmail; // HttpContext.Session.GetString("Email");
            ViewBag.Email = lEmail;

            string conString = Microsoft
                              .Extensions
                              .Configuration
                              .ConfigurationExtensions
                              .GetConnectionString(this.Configuration, "dbAdminDatabase");
            string Command;
           
                Command = "SELECT contractheader.creationdate, contractheader.contractid,  salescompany.companyname, SalesLocations.Legal_Name, SalesLocations.Email_Merchant,   saleslocations.dba_name,  dbo.seg_nomusuario(contractheader.userid) nomSaleMan," +
                " ISNULL((SELECT(LTRIM(RTRIM(FirstName)) + ' ' + LTRIM(RTRIM(LASTnAME))) FROM[dbo].[SalesOwners] WHERE IDOwner = contractheader.OwnerID_1),'') ownerName, isnull(saleslocations.dba_address + '. ', '') + ' ' + isnull(saleslocations.dba_city + '. ', '') + ' ' + isnull(saleslocations.dba_state + '. ', '') + ' ' + isnull(saleslocations.dba_zipcode, '') dba_Address, "+
                " saleslocations.Phone, contractheader.contractdate, isnull((SELECT BRAND FROM[dbo].[LicensesBrand] WHERE id = contractheader.brandid),'') SoftwarePos, "+
                " contractstatus.statusdescription FROM { oj contractheader LEFT OUTER JOIN salescompany ON contractheader.companyid = salescompany.companyid LEFT OUTER JOIN saleslocations ON contractheader.locationid = saleslocations.locationid}, "+
                " contractstatus WHERE(contractstatus.statusid = contractheader.contractstatusid) and contractstatus.statusid ='03'";
           


            using (var sqlConnection = new SqlConnection(conString))
            {
                sqlConnection.Open();
                SqlCommand query = new SqlCommand(Command, sqlConnection);
                
                query.CommandType = System.Data.CommandType.Text;
                int result = query.ExecuteNonQuery();


                using (SqlDataReader reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string res = "";
                       // string phoneFormat = "1234567891";
                       // string resul =  String.Format("{0:###-###-####}", Convert.ToInt64(phoneFormat));
                        string phone = reader["Phone"] is DBNull ? "" : reader["Phone"].ToString();
                        if (phone.Length == 10 && phone != "          ")
                        {
                             res = String.Format("{0:(###)-###-####}", Convert.ToInt64(phone));
                        }
                        else
                        {
                            res = phone;
                        }


                        // Usuario
                        var consul = new Contracts
                        {
                            creationdate = reader["creationdate"] is DBNull ? "" : Convert.ToDateTime(reader["creationdate"]).ToString("MM/dd/yyyy"),
                            contractid = reader["contractid"].ToString(),
                            companyname = reader["companyname"].ToString(),
                            LegalName = reader["Legal_Name"].ToString(),
                            EmailMerchant = reader["Email_Merchant"] is DBNull ? "" : reader["Email_Merchant"].ToString(),
                            dbaname = reader["dba_name"].ToString(),
                            nomSaleMan = reader["nomSaleMan"].ToString(),
                            OnwerName = reader["ownerName"].ToString(),
                            dbaAddress = reader["dba_Address"].ToString(),
                            Phone = res,
                            contractdate = reader["contractdate"] is DBNull ? "" : Convert.ToDateTime(reader["contractdate"]).ToString("MM/dd/yyyy"),
                            SoftwarePos = reader["SoftwarePos"].ToString(),
                            statusdescription = reader["statusdescription"].ToString()                           
                        };

                        Contract.Add(consul);

                    }
                }
            }

            var rol = new UserRol.UserRol();
            ViewBag.RolSystem = rol.Rol;


            return View(Contract);
        }
        public class Contracts {

            public string creationdate { get; set; }
            public string contractid { get; set; }
            public string companyname { get; set; }
            public string LegalName { get; set; }
            public string EmailMerchant { get; set; }
            public string dbaname { get; set; }
            public string nomSaleMan { get; set; }
            public string OnwerName { get; set; }
            public string dbaAddress { get; set; }
            public string Phone { get; set; }
            public string contractdate { get; set; }
            public string SoftwarePos { get; set; }
            public string statusdescription { get; set; }
        }

    }
}