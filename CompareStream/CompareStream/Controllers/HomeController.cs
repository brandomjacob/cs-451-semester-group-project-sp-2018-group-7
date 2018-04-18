﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using CompareStream.Models;
using System.Web.Script.Serialization;

namespace CompareStream.Controllers
{
    public class HomeController : Controller
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["csDB"].ToString());

        public ActionResult Index()
        {
            ViewBag.Title = "Login";
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult DoLogin()
        {
            var loginEmail = Request["loginEmail"];
            var loginPassword = Request["loginPassword"];

            conn.Open();
            String sql = "SELECT isAdmin FROM Users WHERE email = @email AND password = @password;";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@email", System.Data.SqlDbType.NVarChar, 20);
            cmd.Parameters.Add("@password", System.Data.SqlDbType.NVarChar, 20);
            cmd.Parameters["@email"].Value = loginEmail;
            cmd.Parameters["@password"].Value = loginPassword;

            if (true) //Convert.ToInt16(cmd.ExecuteScalar()) == 1)
            {
                HttpCookie emailCookie = new HttpCookie("email");
                HttpCookie isAdminCookie = new HttpCookie("isAdmin");
                // The below is for clearing old cookies
                // we should probably put this in a Logout() function
                //HttpCookie oldLoginCookie = Request.Cookies["login"];
                //oldLoginCookie.Expires.AddYears(-999);
                emailCookie.Value = loginEmail;
                isAdminCookie.Value = Convert.ToString(cmd.ExecuteScalar());
                //loginCookie.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(emailCookie);
                Response.Cookies.Add(isAdminCookie);
            }
            conn.Close();

            return View("Index");
        }

        public ActionResult EditTv()
        {
            ViewBag.Title = "Edit TV";
            return View();
        }

        public ActionResult SelectTvShows()
        {
            ViewBag.Title = "Select TV Shows";
            return View();
        }

        public ActionResult EditStreamingServices()
        {
            ViewBag.Title = "Edit Streaming Services";
            return View();
        }

        public string SearchUsers(string email)
        {
            List<User> userList = new List<User>();
            string query = "SELECT userID, email FROM Users WHERE email LIKE '%" + email + "%';";
            var cmd = new SqlCommand(query, conn);
            cmd.CommandType = System.Data.CommandType.Text;
            conn.Open();
            using (SqlDataReader Reader = cmd.ExecuteReader())
            {
                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        var userID = Reader.GetInt32(Reader.GetOrdinal("userID"));
                        string userEmail = Reader.GetString(Reader.GetOrdinal("email"));
                        User foundUser = new User(userID, userEmail);
                        userList.Add(foundUser);
                    }
                }
            }
            conn.Close();
            string output = new JavaScriptSerializer().Serialize(userList);
            return "{\"users\":" + output + "}";
        }

        public string AddStreamingService()
        {
            // This is not a full page, but a function used by a form
            var serviceName = Request["serviceName"];
            var serviceprice = Request["servicePrice"];
            int affectedRows = 0;
            string output = "Error: Failure to add service to database.";

            conn.Open();
            String sql = "INSERT INTO Services (serviceName, price) VALUES (@name, @price);";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@name", System.Data.SqlDbType.NVarChar, 20);
            cmd.Parameters.Add("@price", System.Data.SqlDbType.Float, 20);
            cmd.Parameters["@name"].Value = serviceName;
            cmd.Parameters["@price"].Value = serviceprice;

            try
            {
                affectedRows = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                // We can log the exception here
            }
            conn.Close();

            if (affectedRows == 1)
                output = serviceName + " with price: " + serviceprice + " was successfully added.";

            return "<div id=\"content\">" + output + "</div>";
        }
        
        
        public string AddTVShow()
        {
           
            // This is not a full page, but a function used by a form
            
            var showName = Request["showName"];

            int affectedRows = 0;

            string output = "Error: Failure to add TV show to database.";


            conn.Open();

            String sql = "INSERT INTO Shows (showName) VALUES (@name);";

            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add("@name", System.Data.SqlDbType.NVarChar, 40);

            cmd.Parameters["@name"].Value = showName;


            try

            {

                affectedRows = cmd.ExecuteNonQuery();

            }

            catch (Exception e)

            {

                // We can log the exception here

            }

            conn.Close();

            if (affectedRows == 1)
                output = showName + " was successfully added.";

            return "<div id=\"content\">" + output + "</div>";
            
        }    
        

        public ActionResult ReportProblem()
        {
            ViewBag.Title = "Report Problem";
            return View();
        }

        public string AddProblem()
        {
            string output = "nothing";
            string outputError = "Error: Failure to report problem.";
            string outputSuccess = "Problem report was successfully sent.";
            var problemDescription = Request["problemDescription"];
            bool problemFixed = false;
            int affectedRows = 0;

            conn.Open();

            String sql = "INSERT INTO Report (problemDescription, userID) VALUES (@problemDescription, @userID);";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@problemDescription", System.Data.SqlDbType.NVarChar, 300);
            cmd.Parameters.Add("@problemFixed", System.Data.SqlDbType.Bit);
            cmd.Parameters["@problemDescription"].Value = problemDescription;
            cmd.Parameters["@problemFixed"].Value = problemFixed;

            try

            {

                affectedRows = cmd.ExecuteNonQuery();

            }

            catch (Exception e)

            {

                // We can log the exception here

            }

            conn.Close();

            if (affectedRows == 1)
                output = outputSuccess;
            else
                output = outputError;

            return "<div id=\"content\">" + output + "</div>";
        }

        public ActionResult ViewStatistics()
        {
            ViewBag.Title = "View Statistics";
            return View();
        }

        public ActionResult ViewReports()
        {
            ViewBag.Title = "View Reports";
            return View();
        }

        public ActionResult ViewAccounts()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
  
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
