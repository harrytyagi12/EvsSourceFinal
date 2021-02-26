using EmployeeMaster.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace EmployeeMaster.Controllers
{
    public class EmployeeController : ApiController
    {
        private string conn = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        [Route("api/Employee/verification")]
        [HttpPost]
        public async Task<object> verfication(employee emp)
        {

            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand("APP.spRegister", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Username", emp.Username);
                    cmd.Parameters.AddWithValue("@Password", emp.Password);
                    cmd.Parameters.AddWithValue("@Email", emp.Email);
                    cmd.Parameters.AddWithValue("@Name", emp.Name);
                    cmd.Parameters.AddWithValue("@UserId", emp.UserId);
                //    cmd.Parameters.Add("@retValue", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.ReturnValue;
                  //  int retval =  (int)cmd.Parameters["@retValue"].Value;



                    con.Open();
                    cmd.ExecuteNonQuery();

                    return new { status = "ok" };
                }
            }

            //return await;

        }
        [Route("api/Employee/DocumentUpload")]
        [HttpPost]
        public async Task<object> DocumentUpload()
        {
            string imageName = null;
            var httpRequest = HttpContext.Current.Request;
            //Upload Image
            var postedFile = httpRequest.Files["docs"];
            
            
            //Create custom filename
            if (postedFile != null)
            {
                imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
                imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
                var filePath = HttpContext.Current.Server.MapPath("~/Documents/" + imageName);
                string User = postedFile.FileName;
                string[] parts = User.Split('_');
                postedFile.SaveAs(filePath);
                int UserId = Convert.ToInt32(parts[0]);
                int DocTypeId = Convert.ToInt32(parts[1]);
                using (SqlConnection con = new SqlConnection(conn))
                {

                    SqlCommand cmd = new SqlCommand("APP.spUploadDocs", con);


                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    cmd.Parameters.AddWithValue("@DocTypeId", DocTypeId);
                    cmd.Parameters.AddWithValue("@DocumentPath", filePath);
                    

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    

                }


            }

            return new { status = "ok" };

        }
        [Route("api/Employee/Login")]
        [HttpPost]
        public DataTable Login(employee emp)
        {

            using (SqlConnection con = new SqlConnection(conn))
            {

                SqlCommand cmd = new SqlCommand("APP.spLogin", con);
                

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", emp.Username);
                cmd.Parameters.AddWithValue("@Password", emp.Password);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                return dt;

                //return await;
            }

        }

        [Route("api/Employee/EmployeeDocs")]
        [HttpGet]
        public DataTable EmployeeDocs()
        {

           
                using (SqlConnection con = new SqlConnection(conn))
                {

                SqlCommand cmd = new SqlCommand("APP.spEmployeesDocs", con);
                
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                return  dt;
            }
            

            //return await;

        }

    }
}
