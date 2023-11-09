using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Net.Mail;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Threading;
using System.Threading.Tasks;
using static Google.Apis.Requests.BatchRequest;
using System.Web.UI;
using System.Reflection;


namespace WebApplication2
{
    /// <summary>
    /// Summary description for brain
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class brain : System.Web.Services.WebService
    {
        public JObject response = new JObject();
        string strcon = ConfigurationManager.ConnectionStrings["connection_string"].ConnectionString;
        string serverPath = HttpContext.Current.Server.MapPath("");
        public void gmail(string mailto, string link)
        {
            string statusResponse = "200";
            string message = "Request successfull";

            string fromMail = "programareaspnet@gmail.com";
            string fromPassword = "zles zjnf dsgf nqyg";

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(fromMail);
            mailMessage.Subject = "Test Subject";
            mailMessage.To.Add(new MailAddress(mailto));
            mailMessage.Body = "<a href=\"localhost/vlad/webform6?link="+link.ToString()+"\">Link activare</a>";
            mailMessage.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true
            };

            smtpClient.Send(mailMessage);



            response["message"] = message;
            response["status"] = statusResponse;
            response["info"] = "{}";


            HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.OK;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(response));
            HttpContext.Current.Response.Flush();


        }

        public void mailpass(string mailto)
        {            
            string fromMail = "programareaspnet@gmail.com";
            string fromPassword = "zles zjnf dsgf nqyg";

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(fromMail);
            mailMessage.Subject = "Reset Password";
            mailMessage.To.Add(new MailAddress(mailto));
            mailMessage.Body = "<a href=\"localhost/vlad/webform8\">Link resetare parola</a>";
            mailMessage.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true
            };

            smtpClient.Send(mailMessage);

        }

        private static string Base64UrlEncode2(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');
        }



        [WebMethod]
        public void sendForgotPassword()
        {
            SqlConnection con = new SqlConnection(strcon);
            string link = RandomStringGenerator();

            string statusResponse = "200";
            string message = "Request successfull";
            string frontMail = HttpContext.Current.Request.Form["mail_front"];

            string fromMail = "programareaspnet@gmail.com";
            string fromPassword = "zles zjnf dsgf nqyg";

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(fromMail);
            mailMessage.Subject = "Reset Password";
            mailMessage.To.Add(new MailAddress(frontMail));
            mailMessage.Body = "<a href=\"localhost/vlad/webform8?link=" + link.ToString() + "\">Link resetare parola</a>";
            mailMessage.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true
            };

            smtpClient.Send(mailMessage);

            DateTime dataDeMaine = DateTime.Now.AddDays(1);

            string query = "UPDATE users SET linkactivare = @link, time = @dataDeMaine WHERE mail = @mail ";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@link", link.ToString());
            cmd.Parameters.AddWithValue("@mail", frontMail.ToString());
            cmd.Parameters.AddWithValue("@dataDeMaine", dataDeMaine);

            DataTable dt = new DataTable();
            con.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                dt.Load(reader);
                con.Close();
            }

            response["message"] = message;
            response["status"] = statusResponse;
            response["info"] = JsonConvert.SerializeObject(response);

            HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.OK;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(response));
            HttpContext.Current.Response.Flush();
        }



        [WebMethod]
        public void resetpassword()
        {
            string statusResponse = "200";
            

            string frontPass = HttpContext.Current.Request.Form["front_pass"];
            string frontPass2 = HttpContext.Current.Request.Form["front_pass2"];
            string frontMail = HttpContext.Current.Request.Form["front_mail"];

           
            List<string> message = new List<string>();

            int erCount = 0;

            string query = "";
            SqlConnection con = new SqlConnection(strcon);

            frontMail = frontMail.Trim();
            frontPass = frontPass.Trim();
            frontPass2 = frontPass2.Trim();

            
            bool contineCaracterSpecial = Regex.IsMatch(frontPass, @"[!@#$%^&*(),.?\\"":{}|<>]");
            bool contineLiteraMare = Regex.IsMatch(frontPass, @"[A-Z]");
            if (!contineCaracterSpecial || !contineLiteraMare)
            {
                erCount++;
                message.Add("Parola trebuie sa contina minim o majuscula si un caracter special!");
                statusResponse = "500";
            }

            if (frontPass == "" || frontPass.Length < 10 || frontMail == "" || frontMail.Length < 10 || frontPass2 == "" || frontPass2.Length < 10)
            {
                erCount++;
                message.Add("Campurile trebuie sa aiba minim 10 caractere!");
                statusResponse = "500";
            }

            if (frontPass != frontPass2)
            {
                erCount++;
                message.Add("Parolele nu coincid");
                statusResponse = "500";
            }

            string cryptPass = Encrypt(frontPass, encKryKey, frontMail);

            if (erCount == 0)
            {

                query = "UPDATE users SET pass = @pass WHERE mail = @mail";

                
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@mail", frontMail.ToString());              
                cmd.Parameters.AddWithValue("@pass", cryptPass.ToString());
               

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                    con.Close();
                }
                

                message.Add("Parola a fost schimbata cu succees!");
                response["info"] = JsonConvert.SerializeObject(dt);

                DataTable dt2 = new DataTable();
                SqlConnection con2 = new SqlConnection(strcon);


                string query2 = "UPDATE users SET linkactivare = ''  WHERE mail LIKE @mail";



                SqlCommand cmd2 = new SqlCommand(query2, con2);
                cmd2.Parameters.AddWithValue("@mail", frontMail.ToString());


                con2.Open();
                using (SqlDataReader reader = cmd2.ExecuteReader())
                {
                    dt2.Load(reader);
                    con2.Close();
                }

            }


            response["status"] = statusResponse;
            response["message"] = JsonConvert.SerializeObject(message);
            HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.OK;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(response));
            HttpContext.Current.Response.Flush();
        }



        [WebMethod]
        public void insert()
        {
            string statusResponse = "200";
            string message = "Request successfull";
            string frontNume = HttpContext.Current.Request.Form["nume_frontend"];
            string frontCnp = HttpContext.Current.Request.Form["cnp_frontend"];
            string frontMail = HttpContext.Current.Request.Form["mail_frontend"];
            string frontTelefon = HttpContext.Current.Request.Form["telefon_frontend"];


            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(strcon);


            string query = "INSERT INTO tabel2 (nume, cnp, mail, telefon, status) VALUES (@numeFiltru, @cnpFiltru, @mailFiltru, @telefonFiltru, '1')";
            //string query = "INSERT INTO tabel2 (nume) VALUES ('" + frontAsc.ToString() + "')";  //, '" + frontCnp.ToString() + "', '" + frontMail.ToString() + "', '" + frontTelefon.ToString() + "')";


            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@numeFiltru", frontNume.ToString());
            cmd.Parameters.AddWithValue("@cnpFiltru", frontCnp.ToString());
            cmd.Parameters.AddWithValue("@mailFiltru", frontMail.ToString());
            cmd.Parameters.AddWithValue("@telefonFiltru", frontTelefon.ToString());


            con.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                dt.Load(reader);
                con.Close();
            }

            //frontString = frontString + " Hello back";

            response["message"] = message;
            response["status"] = statusResponse;
            response["info"] = JsonConvert.SerializeObject(dt);

            HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.OK;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(response));
            HttpContext.Current.Response.Flush();
        }

        /*DateTime currentDateTime = DateTime.Now;
            string timestamp = currentDateTime.ToString("yyyyMMddHHmmssfff");
            string randomStore = randomStorageName();

            HttpPostedFile file = HttpContext.Current.Request.Files[0];
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            string storageName = timestamp + randomStore + fileExtension;

            if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".pdf")
            {
                try
                {

                    string fileName = Path.GetFileName(file.FileName);
                    string savePath = Server.MapPath("~/media/") + fileName;
                    file.SaveAs(savePath);
                    string savePathStorage = "media/";


                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection(strcon);


                    string query = "INSERT INTO media (status, name, storageName, path) VALUES ('1', @name, @storageName, @path)";
                    


                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@name", fileName.ToString());
                    cmd.Parameters.AddWithValue("@storageName", storageName.ToString());
                    cmd.Parameters.AddWithValue("@path", savePathStorage.ToString());
                  


                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                        con.Close();
                    }





                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }
            else
            {
                message = "Failed";
                statusResponse = "500";
                
            }
        */

        [WebMethod]
        public void insertCars()
        {
            string statusResponse = "200";
            string message = "Request successful";

            string fkUser = HttpContext.Current.Request.Form["fk_user_frontend"];
            string marca = HttpContext.Current.Request.Form["marca_frontend"];
            string model = HttpContext.Current.Request.Form["model_frontend"];

            List<int> fileIds = new List<int>();

            int fileCount = HttpContext.Current.Request.Files.Count;
            for (int i = 0; i < fileCount; i++)
            {
                HttpPostedFile file = HttpContext.Current.Request.Files[i];

                DateTime currentDateTime = DateTime.Now;
                string timestamp = currentDateTime.ToString("yyyyMMddHHmmssfff");
                string randomStore = randomStorageName();

                string fileExtension = Path.GetExtension(file.FileName).ToLower();
                string storageName = timestamp + randomStore + fileExtension;

                if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".pdf")
                {
                    try
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        string savePath = Server.MapPath("~/media/") + fileName;
                        file.SaveAs(savePath);
                        string savePathStorage = "media/";

                        DataTable dt2 = new DataTable();
                        SqlConnection con2 = new SqlConnection(strcon);

                        string query2 = "INSERT INTO media (status, name, storageName, path) OUTPUT Inserted.id VALUES ('1', @name, @storageName, @path)";

                        SqlCommand cmd2 = new SqlCommand(query2, con2);

                        cmd2.Parameters.AddWithValue("@name", fileName.ToString());
                        cmd2.Parameters.AddWithValue("@storageName", storageName.ToString());
                        cmd2.Parameters.AddWithValue("@path", savePathStorage.ToString());

                        con2.Open();
                        using (SqlDataReader reader = cmd2.ExecuteReader())
                        {
                            dt2.Load(reader);
                            con2.Close();
                        }

                        string query3 = "SELECT id FROM media WHERE storageName = @storageName";
                        SqlConnection con3 = new SqlConnection(strcon);
                        SqlCommand cmd3 = new SqlCommand(query3, con3);
                        cmd3.Parameters.AddWithValue("@storageName", storageName.ToString());

                        con3.Open();
                        int userId = (int)cmd3.ExecuteScalar();
                        con3.Close();

                        fileIds.Add(userId);
                        
                    }
                    catch (Exception ex)
                    {
                        message = ex.Message;
                    }
                }
                else
                {
                    message = "Failed";
                    statusResponse = "500";
                }
            }

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(strcon);

            string query = "INSERT INTO cars (fk_user, files, marca, model, status) VALUES (@fkuser, @files, @marca, @model, '1')";

            SqlCommand cmd = new SqlCommand(query, con);

            string fileIdsString = string.Join(",", fileIds);

            cmd.Parameters.AddWithValue("@fkuser", fkUser.ToString());
            cmd.Parameters.AddWithValue("@files", fileIdsString.ToString());
            cmd.Parameters.AddWithValue("@marca", marca.ToString());
            cmd.Parameters.AddWithValue("@model", model.ToString());

            con.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                dt.Load(reader);
                con.Close();
            }

            Dictionary<string, string> response = new Dictionary<string, string>();
            response["message"] = message;
            response["status"] = statusResponse;
            response["info"] = JsonConvert.SerializeObject(dt);

            HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.OK;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(response));
            HttpContext.Current.Response.Flush();
        }

        [WebMethod]
        public void select()
        {
            string statusResponse = "200";
            string message = "Request successfull";


            string frontIdFiltru = HttpContext.Current.Request.Form["id_frontend_filtru"];
            string frontNumeFiltru = HttpContext.Current.Request.Form["nume_frontend_filtru"];
            string frontCnpFiltru = HttpContext.Current.Request.Form["cnp_frontend_filtru"];
            string frontMailFiltru = HttpContext.Current.Request.Form["mail_frontend_filtru"];
            string frontTelefonFiltru = HttpContext.Current.Request.Form["telefon_frontend_filtru"];// preiei variabila din front end

            string orderBy = HttpContext.Current.Request.Form["order_front"];
            string orderElement = HttpContext.Current.Request.Form["element_front"];

            frontIdFiltru = frontIdFiltru.Length > 0 ? frontIdFiltru : "%%";

            frontNumeFiltru = "%" + frontNumeFiltru + "%";
            frontCnpFiltru = "%" + frontCnpFiltru + "%";
            frontMailFiltru = "%" + frontMailFiltru + "%";
            frontTelefonFiltru = "%" + frontTelefonFiltru + "%";

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(strcon);

            //string query2 = "SELECT * FROM tabel2 WHERE  id LIKE '" + frontIdFiltru.ToString() + "' AND nume LIKE '%" + frontNumeFiltru.ToString() + "%' AND  cnp LIKE '%" + frontCnpFiltru.ToString() + "%' AND  mail LIKE '%" + frontMailFiltru.ToString() + "%' AND  telefon LIKE '%" + frontTelefonFiltru.ToString() + "%' AND status LIKE '1' ORDER BY " + orderElement.ToString() + "  " + orderBy.ToString()+"";

            string query2 = "SELECT * FROM tabel2 WHERE  id LIKE @idFiltru AND nume LIKE @numeFiltru AND  cnp LIKE @cnpFiltru AND  mail LIKE @mailFiltru AND  telefon LIKE @telefonFiltru AND status LIKE '1' ORDER BY " + orderElement.ToString() + "  " + orderBy.ToString() + "";



            SqlCommand cmd2 = new SqlCommand(query2, con);

            cmd2.Parameters.AddWithValue("@idFiltru", frontIdFiltru.ToString());
            cmd2.Parameters.AddWithValue("@numeFiltru", frontNumeFiltru.ToString());
            cmd2.Parameters.AddWithValue("@cnpFiltru", frontCnpFiltru.ToString());
            cmd2.Parameters.AddWithValue("@mailFiltru", frontMailFiltru.ToString());
            cmd2.Parameters.AddWithValue("@telefonFiltru", frontTelefonFiltru.ToString());

            con.Open();
            using (SqlDataReader reader = cmd2.ExecuteReader())
            {
                dt.Load(reader);
                con.Close();
            }


            //frontString = frontString + " Hello back";

            response["message"] = message;
            response["status"] = statusResponse;
            response["info"] = JsonConvert.SerializeObject(dt);

            HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.OK;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(response));
            HttpContext.Current.Response.Flush();
        }


        [WebMethod]
        public void selectCars()
        {
            string statusResponse = "200";
            string message = "Request successfull";


            string frontIdFiltru = HttpContext.Current.Request.Form["id_frontend_filtru"];
            string frontMarcaFiltru = HttpContext.Current.Request.Form["marca_frontend_filtru"];
            string frontModelFiltru = HttpContext.Current.Request.Form["model_frontend_filtru"];
            // preiei variabila din front end

            string orderBy = HttpContext.Current.Request.Form["order_front"];
            string orderElement = HttpContext.Current.Request.Form["element_front"];

            frontIdFiltru = frontIdFiltru.Length > 0 ? frontIdFiltru : "%%";

            frontMarcaFiltru = "%" + frontMarcaFiltru + "%";
            frontModelFiltru = "%" + frontModelFiltru + "%";
            

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(strcon);

            //string query2 = "SELECT * FROM tabel2 WHERE  id LIKE '" + frontIdFiltru.ToString() + "' AND nume LIKE '%" + frontNumeFiltru.ToString() + "%' AND  cnp LIKE '%" + frontCnpFiltru.ToString() + "%' AND  mail LIKE '%" + frontMailFiltru.ToString() + "%' AND  telefon LIKE '%" + frontTelefonFiltru.ToString() + "%' AND status LIKE '1' ORDER BY " + orderElement.ToString() + "  " + orderBy.ToString()+"";

            string query2 = "SELECT * FROM cars WHERE  id LIKE @idFiltru AND marca LIKE @marcaFiltru AND  model LIKE @modelFiltru AND status LIKE '1' ORDER BY " + orderElement.ToString() + "  " + orderBy.ToString() + "";



            SqlCommand cmd2 = new SqlCommand(query2, con);

            cmd2.Parameters.AddWithValue("@idFiltru", frontIdFiltru.ToString());
            cmd2.Parameters.AddWithValue("@marcaFiltru", frontMarcaFiltru.ToString());
            cmd2.Parameters.AddWithValue("@modelFiltru", frontModelFiltru.ToString());
            

            con.Open();
            using (SqlDataReader reader = cmd2.ExecuteReader())
            {
                dt.Load(reader);
                con.Close();
            }


            //frontString = frontString + " Hello back";

            response["message"] = message;
            response["status"] = statusResponse;
            response["info"] = JsonConvert.SerializeObject(dt);

            HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.OK;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(response));
            HttpContext.Current.Response.Flush();
        }

        [WebMethod]
        public void update()
        {
            string statusResponse = "200";
            string message = "Request successfull";

            string frontUpdateId = HttpContext.Current.Request.Form["id_update"];
            string frontUpdateNume = HttpContext.Current.Request.Form["nume_update"];
            string frontUpdateCnp = HttpContext.Current.Request.Form["cnp_update"];
            string frontUpdateMail = HttpContext.Current.Request.Form["mail_update"];
            string frontUpdateTelefon = HttpContext.Current.Request.Form["telefon_update"];


            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(strcon);


            string query = "UPDATE tabel2 SET nume = @numeFiltru, cnp = @cnpFiltru, mail = @mailFiltru, telefon = @telefonFiltru WHERE id = @idFiltru ";
            //string query = "INSERT INTO tabel2 (nume) VALUES ('" + frontAsc.ToString() + "')";  //, '" + frontCnp.ToString() + "', '" + frontMail.ToString() + "', '" + frontTelefon.ToString() + "')";


            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@numeFiltru", frontUpdateNume.ToString());
            cmd.Parameters.AddWithValue("@cnpFiltru", frontUpdateCnp.ToString());
            cmd.Parameters.AddWithValue("@mailFiltru", frontUpdateMail.ToString());
            cmd.Parameters.AddWithValue("@telefonFiltru", frontUpdateTelefon.ToString());
            cmd.Parameters.AddWithValue("@idFiltru", frontUpdateId.ToString());

            con.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                dt.Load(reader);
                con.Close();
            }

            //frontString = frontString + " Hello back";

            response["message"] = message;
            response["status"] = statusResponse;
            response["info"] = JsonConvert.SerializeObject(dt);

            HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.OK;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(response));
            HttpContext.Current.Response.Flush();
        }

        [WebMethod]
        public void updateCars()
        {
            string statusResponse = "200";
            string message = "Request successfull";

            string frontUpdateId = HttpContext.Current.Request.Form["id_update"];
            string frontUpdateFkUser = HttpContext.Current.Request.Form["fk_user_update"];
            string frontUpdateFiles = HttpContext.Current.Request.Form["files_update"];
            string frontUpdateMarca = HttpContext.Current.Request.Form["marca_update"];
            string frontUpdateModel = HttpContext.Current.Request.Form["model_update"];


            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(strcon);


            string query = "UPDATE cars SET fk_user = @fkuser, files = @files, marca = @marca, model = @model WHERE id = @idFiltru ";
            //string query = "INSERT INTO tabel2 (nume) VALUES ('" + frontAsc.ToString() + "')";  //, '" + frontCnp.ToString() + "', '" + frontMail.ToString() + "', '" + frontTelefon.ToString() + "')";


            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@fkuser", frontUpdateFkUser.ToString());
            cmd.Parameters.AddWithValue("@files", frontUpdateFiles.ToString());
            cmd.Parameters.AddWithValue("@marca", frontUpdateMarca.ToString());
            cmd.Parameters.AddWithValue("@model", frontUpdateModel.ToString());
            cmd.Parameters.AddWithValue("@idFiltru", frontUpdateId.ToString());

            con.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                dt.Load(reader);
                con.Close();
            }

            //frontString = frontString + " Hello back";

            response["message"] = message;
            response["status"] = statusResponse;
            response["info"] = JsonConvert.SerializeObject(dt);

            HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.OK;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(response));
            HttpContext.Current.Response.Flush();
        }


    /*    [WebMethod]
        public void upload()
        {
            string statusResponse = "200";
            string message = "Request successful";

            DateTime currentDateTime = DateTime.Now;
            string timestamp = currentDateTime.ToString("yyyyMMddHHmmssfff");
            string randomStore = randomStorageName();

            HttpPostedFile file = HttpContext.Current.Request.Files[0];
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            string storageName = timestamp + randomStore + fileExtension;

            if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".pdf")
            {
                try
                {

                    string fileName = Path.GetFileName(file.FileName);
                    string savePath = Server.MapPath("~/media/") + fileName;
                    file.SaveAs(savePath);
                    string savePathStorage = "media/";


                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection(strcon);


                    string query = "INSERT INTO media (status, name, storageName, path) VALUES ('1', @name, @storageName, @path)";
                    


                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@name", fileName.ToString());
                    cmd.Parameters.AddWithValue("@storageName", storageName.ToString());
                    cmd.Parameters.AddWithValue("@path", savePathStorage.ToString());
                  


                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                        con.Close();
                    }





                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }
            else
            {
                message = "Failed";
                statusResponse = "500";
                
            }

            response["message"] = message;
            response["status"] = statusResponse;
            response["info"] = "";

            HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.OK;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(response));
            HttpContext.Current.Response.Flush();
        }

*/

        [WebMethod]
        public void delete()
        {
            string statusResponse = "200";
            string message = "Request successfull";

            string frontDeleteId = HttpContext.Current.Request.Form["id_delete"];


            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(strcon);


            string query = "UPDATE tabel2 SET status = '0'  WHERE id = @idFiltru ";
            //string query = "INSERT INTO tabel2 (nume) VALUES ('" + frontAsc.ToString() + "')";  //, '" + frontCnp.ToString() + "', '" + frontMail.ToString() + "', '" + frontTelefon.ToString() + "')";


            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@idFiltru", frontDeleteId.ToString());

            con.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                dt.Load(reader);
                con.Close();
            }

            //frontString = frontString + " Hello back";

            response["message"] = message;
            response["status"] = statusResponse;
            response["info"] = JsonConvert.SerializeObject(dt);

            HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.OK;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(response));
            HttpContext.Current.Response.Flush();
        }

        [WebMethod]
        public void deleteCars()
        {
            string statusResponse = "200";
            string message = "Request successfull";

            string frontDeleteId = HttpContext.Current.Request.Form["id_delete"];


            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(strcon);


            string query = "UPDATE cars SET status = '0'  WHERE id = @idFiltru ";
            //string query = "INSERT INTO tabel2 (nume) VALUES ('" + frontAsc.ToString() + "')";  //, '" + frontCnp.ToString() + "', '" + frontMail.ToString() + "', '" + frontTelefon.ToString() + "')";


            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@idFiltru", frontDeleteId.ToString());

            con.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                dt.Load(reader);
                con.Close();
            }

            //frontString = frontString + " Hello back";

            response["message"] = message;
            response["status"] = statusResponse;
            response["info"] = JsonConvert.SerializeObject(dt);

            HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.OK;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(response));
            HttpContext.Current.Response.Flush();
        }

        [WebMethod]
        public void crypt()
        {
            string statusResponse = "200";
            string message = "Request successfull";

            string frontPass = HttpContext.Current.Request.Form["pass_front"];
            string frontMail = HttpContext.Current.Request.Form["inputMail"];



            string cryptPass = Encrypt(frontPass, encKryKey, frontMail);

            //frontString = frontString + " Hello back";

            response["message"] = message;
            response["status"] = statusResponse;
            response["info"] = cryptPass;

            HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.OK;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(response));
            HttpContext.Current.Response.Flush();

        }

        [WebMethod]
        public void decrypt()
        {
            string statusResponse = "200";
            string message = "Request successfull";

            string frontPass = HttpContext.Current.Request.Form["inputPass"];

            string frontMail = HttpContext.Current.Request.Form["inputMail"];


            string cryptPass = Decrypt(frontPass, encKryKey, frontMail);

            //frontString = frontString + " Hello back";

            response["message"] = message;
            response["status"] = statusResponse;
            response["info"] = cryptPass;

            HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.OK;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(response));
            HttpContext.Current.Response.Flush();

        }

        string encKryKey = "ceva";

        [WebMethod]
        public void reg()
        {
            string statusResponse = "200";
            response["info"] = "{}";

            DateTime dataDeMaine = DateTime.Now.AddDays(1);

            string frontMail = HttpContext.Current.Request.Form["mail_front"];
            string frontPass = HttpContext.Current.Request.Form["pass_front"];
            string frontPass2 = HttpContext.Current.Request.Form["pass2_front"];
            string frontNick = HttpContext.Current.Request.Form["nick_front"];
            string frontTel = HttpContext.Current.Request.Form["tel_front"];
            List<string> message = new List<string>();

            int erCount = 0;

            string query = "";
            SqlConnection con = new SqlConnection(strcon);

            frontMail = frontMail.Trim();
            frontPass = frontPass.Trim();
            frontPass2 = frontPass2.Trim();
            frontNick = frontNick.Trim();
            frontTel = frontTel.Trim();


            bool contineCaracterSpecial = Regex.IsMatch(frontPass, @"[!@#$%^&*(),.?\\"":{}|<>]");
            bool contineLiteraMare = Regex.IsMatch(frontPass, @"[A-Z]");
            if (!contineCaracterSpecial || !contineLiteraMare)
            {
                erCount++;
                message.Add("Parola trebuie sa contina minim o majuscula si un caracter special!");
                statusResponse = "500";
            }

            if (frontPass == "" || frontPass.Length < 10 || frontMail == "" || frontMail.Length < 10 || frontPass2 == "" || frontPass2.Length < 10 || frontNick == "" || frontNick.Length < 10 || frontTel.Length < 10 || frontTel == "" || frontPass != frontPass2)
            {
                erCount++;
                message.Add("Campurile trebuie sa aiba minim 10 caractere!");
                statusResponse = "500";
            }

            // Verifica daca adresa de e-mail exista deja
            //  int emailCount = checkEmail(con, frontMail.ToString(), "users");

            //  if (emailCount > 0)
            //   {
            //       erCount++;
            //       message.Add("Această adresă de e-mail este deja înregistrată!");
            //        statusResponse = "500";
            //    }
            string link = RandomStringGenerator();
            string cryptPass = Encrypt(frontPass, encKryKey, frontMail);

            string queryCheckEmail = $"SELECT COUNT(*) FROM users WHERE mail = @mail";
            SqlCommand cmdCheckEmail = new SqlCommand(queryCheckEmail, con);
            cmdCheckEmail.Parameters.AddWithValue("@mail", frontMail);
            con.Open();
            int emailCountMail = (int)cmdCheckEmail.ExecuteScalar();
            con.Close();

            if (emailCountMail > 0)
            {
                string queryCheckEmail2 = $"SELECT COUNT(*) FROM users WHERE mail = @mail AND status = '0'";
                SqlCommand cmdCheckEmail2 = new SqlCommand(queryCheckEmail2, con);
                cmdCheckEmail2.Parameters.AddWithValue("@mail", frontMail);
                con.Open();
                int emailCountStatus = (int)cmdCheckEmail2.ExecuteScalar();
                con.Close();

                if (emailCountStatus > 0)
                {
                    erCount++;
                    message.Add("Această adresă de e-mail corespunde unui cont sters!");
                    statusResponse = "500";
                }
                
                string queryCheckEmail3 = $"SELECT COUNT(*) FROM users WHERE mail = @mail AND activ = '1'";
                SqlCommand cmdCheckEmail3 = new SqlCommand(queryCheckEmail3, con);
                cmdCheckEmail3.Parameters.AddWithValue("@mail", frontMail);
                con.Open();
                int emailCountActive = (int)cmdCheckEmail3.ExecuteScalar();
                con.Close();

                if (emailCountActive > 0 && emailCountStatus == 0)
                {
                    erCount++;
                    message.Add("Această adresă de e-mail corespunde unui cont existent!");
                    statusResponse = "500";

                };

                if(emailCountActive == 0 && emailCountStatus == 0)
                {
                   

                    erCount++;
                    string query4 = "UPDATE users SET mail = @mail, pass = @pass, nickname = @nickname, telefon = @telefon, status = '1', activ = '0', linkactivare = @link, time = @dataDeMaine WHERE mail = @mail ";
                    SqlCommand cmdCheckEmail4 = new SqlCommand(query4, con);
                    cmdCheckEmail4.Parameters.AddWithValue("@mail", frontMail.ToString());
                    cmdCheckEmail4.Parameters.AddWithValue("@pass", cryptPass.ToString());
                    cmdCheckEmail4.Parameters.AddWithValue("@nickname", frontNick.ToString());
                    cmdCheckEmail4.Parameters.AddWithValue("@telefon", frontTel.ToString());
                    cmdCheckEmail4.Parameters.AddWithValue("@link", link.ToString());
                    cmdCheckEmail4.Parameters.AddWithValue("@dataDeMaine", dataDeMaine);

                    DataTable dt = new DataTable();
                    con.Open();
                    using (SqlDataReader reader = cmdCheckEmail4.ExecuteReader())
                    {
                        dt.Load(reader);
                        con.Close();
                    }
                    gmail(frontMail, link);
                }

            }




            

            



            //232424234





            if (erCount == 0)
            {
                
                query = "INSERT INTO users (pass, mail, nickname, telefon, status, activ, linkactivare, time) VALUES (@pass, @mail, @nickname, @tel, 1, 0, @link, @dataDeMaine)";

                //string query = "INSERT INTO users (username, pass) VALUES (@user, @pass)";
                //string query = "INSERT INTO tabel2 (nume) VALUES ('" + frontAsc.ToString() + "')";  //, '" + frontCnp.ToString() + "', '" + frontMail.ToString() + "', '" + frontTelefon.ToString() + "')";

                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@mail", frontMail.ToString());
                cmd.Parameters.AddWithValue("@link", link.ToString());
                cmd.Parameters.AddWithValue("@pass", cryptPass.ToString());
                cmd.Parameters.AddWithValue("@nickname", frontNick.ToString());
                cmd.Parameters.AddWithValue("@tel", frontTel.ToString());
                cmd.Parameters.AddWithValue("@dataDeMaine", dataDeMaine);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                    con.Close();
                }
                //frontString = frontString + " Hello back";
                gmail(frontMail, link);

                message.Add("Request succesfull");
                response["info"] = JsonConvert.SerializeObject(dt);
            }
            response["message"] = JsonConvert.SerializeObject(message);
            response["status"] = statusResponse;
            HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.OK;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(response));
            HttpContext.Current.Response.Flush();

        }


      
        public string RandomStringGenerator()
        {
             Random random = new Random();
             const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            
            
                StringBuilder result = new StringBuilder(30);

                for (int i = 0; i < 30; i++)
                {
                    result.Append(chars[random.Next(chars.Length)]);
                }

                return result.ToString();
            
        }
        public string randomStorageName()
        {
            Random random = new Random();
            const string chars = "0123456789";



            StringBuilder result = new StringBuilder(30);

            for (int i = 0; i < 4; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();

        }


        [WebMethod(EnableSession = true)]
        public void login()
        {
            string statusResponse = "200";
            string message = "Request successfull?";

            string frontMail = HttpContext.Current.Request.Form["mail_front"];
            string frontPass = HttpContext.Current.Request.Form["pass_front"];


            string cryptPass = Encrypt(frontPass, encKryKey, frontMail);

            DataTable dt = new DataTable();
            string queryReadPass = "SELECT pass FROM users WHERE mail = @mail AND status = 1 AND activ = 1";
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmdReadPass = new SqlCommand(queryReadPass, con);

            cmdReadPass.Parameters.AddWithValue("@mail", frontMail.ToString());

            con.Open();
            using (SqlDataReader reader = cmdReadPass.ExecuteReader())
            {
                dt.Load(reader);
                con.Close();
            }

            if (dt.Rows.Count == 1)
            {
                if (cryptPass == dt.Rows[0]["pass"].ToString())
                {

                    message = "Login Succes";
                    Session["name"] = "Vlad";

                    statusResponse = "200";

                }
                else
                {
                    message = "Wrong password";
                    statusResponse = "500";

                }
            }
            else {
                message = "Non existent account";
                statusResponse = "500";
            }

            response["message"] = message;
            response["status"] = statusResponse;
            response["info"] = "";

            HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.OK;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(response));
            HttpContext.Current.Response.Flush();
        }



        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        // quality of life start

        public int checkEmail(SqlConnection con, string email, string tabela )
        {
            string queryCheckEmail = $"SELECT COUNT(*) FROM {tabela} WHERE mail = @mail AND status = '1' AND activ = '1'";
            SqlCommand cmdCheckEmail = new SqlCommand(queryCheckEmail, con);
            cmdCheckEmail.Parameters.AddWithValue("@mail", email);
            con.Open();
            int emailCount = (int)cmdCheckEmail.ExecuteScalar();
            con.Close();
            return emailCount;
        }


        // quality of life start
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

        
        private string Encrypt(string clearText, string pass, string mail)
        {
            
            string encryptionKey = pass + mail;
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

                private string Decrypt(string cipherText, string pass, string mail)
        {
            string encryptionKey = pass + mail;
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
                




    }
}


/* public void select()
{
    string statusResponse = "200";
    string message = "Request successfull";
    string frontString = HttpContext.Current.Request.Form["nume_from_front_end"];
    string frontString2 = HttpContext.Current.Request.Form["nume_from_front_end2"];// preiei variabila din front end



    DataTable dt = new DataTable();
    SqlConnection con = new SqlConnection(strcon);

    string query = @"SELECT * FROM table1 
                                      WHERE 
                                            ID LIKE '%" + frontString.ToString() + @"%' AND 
                                            name LIKE '%" + frontString2.ToString() + @"%' 
                            ";




    // LIKE - > iti permite sa folosesti sintaxe de tip '%%' care ce face 
    // '%%' -> cauta in coloana ta toate valorile care cuprind ce pui tu acolo
    //              ex: '%bobi%'
    //                              -> 'el, bobi a luat' -> va fi selectat
    //                              -> 'el, bob a luat' -> nu va fi selectat
    //                              -> 'bobi el,a luat' -> va fi selectat


    // 'bobi%' 
    //  -------> in functie de partea pe care se afla % cauta elementele care incep sau se termina in ce pui tu acolo
    // '%bobi'
    //              ex: 'bobi%'
    //                              -> 'bobi a luat' -> va fi selectat
    //                              -> 'el, bobi a luat' -> nu va fi selectat


    SqlCommand cmd = new SqlCommand(query, con);

    con.Open();
    using (SqlDataReader reader = cmd.ExecuteReader())
    {
        dt.Load(reader);
        con.Close();
    }


    //frontString = frontString + " Hello back";

    response["message"] = message;
    response["status"] = statusResponse;
    response["info"] = JsonConvert.SerializeObject(dt);

    HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.OK;
    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(response));
    HttpContext.Current.Response.Flush();
}



*/