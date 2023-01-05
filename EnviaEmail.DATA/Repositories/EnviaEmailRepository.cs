using EnviaEmail.DATA.Interface;
using EnviaEmail.DATA.Model;
using EnviaEmail.DATA.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EnviaEmail.DATA.Repositories
{
    public class EnviaEmailRepository
    {
        private IHostEnvironment _hostEnvironment;
        readonly IConfiguration Configuration;

        private readonly string _ConnMA;
        private readonly string _MA;
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        //readonly EmailService _mailService;
        //private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy = Policy<HttpResponseMessage>.Handle<HttpRequestException>()
        //    .OrResult(x => x.StatusCode >= System.Net.HttpStatusCode.InternalServerError || x.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
        //    .RetryAsync(5);

        //public EnviaEmailRepository(IConfiguration Configuration, IHostEnvironment hostingEnvironment, IHttpClientFactory httpClientFactory)
        public EnviaEmailRepository(IConfiguration Configuration, IHostEnvironment hostingEnvironment)
        {
            _hostEnvironment = hostingEnvironment;
            this.Configuration = Configuration;
            _ConnMA = Configuration.GetValue<string>("ConnMA");
            _MA = Configuration.GetValue<string>("MA");
        }

        public GenerateTokenModel InsertTokenAndEmailDB(EmailTokenModel model)
        {
            TimeSpan diff = TimeSpan.Zero;
            GenerateTokenModel obj = new GenerateTokenModel();
            var querySelect = @"SELECT * FROM " + _MA + @" WHERE email like @email";
            using (SqlConnection con = new SqlConnection(_ConnMA))
            {
                con.Open();

                SqlCommand command = con.CreateCommand();


                command.Connection = con;
                try
                {
                    SqlParameter parameter = new SqlParameter();
                    parameter.ParameterName = "@email";
                    parameter.SqlDbType = SqlDbType.VarChar;
                    parameter.Direction = ParameterDirection.Input;
                    parameter.Value = model.email;

                    // Add the parameter to the Parameters collection.
                    command.Parameters.Add(parameter);
                    command.CommandText = querySelect;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var _obj = new GenerateTokenModel();
                                _obj.email = reader["email"].ToString();
                                _obj.token = reader["token"].ToString();
                                _obj.data_insert = (DateTime)reader["data_insert"];
                                obj = _obj;
                            }
                        }
                        else
                        {
                            Console.WriteLine("No rows found.");
                        }
                        reader.Close();
                    }

                }


                catch (Exception e)
                {
                    //transaction.Rollback();
                    con.Close();
                    throw;
                }

                if (obj.data_insert != null)
                {
                    diff = DateTime.Now - obj.data_insert.Value;
                }


                if (obj.token != null && diff.TotalHours < 24)
                {

                    con.Close();
                    return obj;
                }

                //generate random token
                var random = new Random();
                var token = new char[5];
                for (int i = 0; i < 5; i++)
                {
                    token[i] = validChars[random.Next(validChars.Length)];
                }
                string tokenString = new string(token);
                //end

                SqlTransaction transaction;

                if (obj.token != null && diff.TotalHours > 24)
                {
                    transaction = con.BeginTransaction("Transaction");
                    command.Transaction = transaction;
                    querySelect = @"UPDATE " + _MA + @" SET token = @token, data_insert = @data_insert WHERE email = @email";
                    try
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add("@email", SqlDbType.VarChar);
                        command.Parameters["@email"].Value = model.email;
                        command.Parameters.Add("@token", SqlDbType.VarChar);
                        command.Parameters["@token"].Value = tokenString;
                        command.Parameters.Add("@data_insert", SqlDbType.DateTime);
                        command.Parameters["@data_insert"].Value = DateTime.Now;

                        command.CommandText = querySelect;
                        command.ExecuteNonQuery();
                        transaction.Commit();
                        obj.token = tokenString;
                    }

                    catch (Exception e)
                    {
                        transaction.Rollback();
                        con.Close();
                        throw;
                    }
                    finally
                    {
                        con.Close();
                    }
                    con.Close();
                    return obj;
                }



                transaction = con.BeginTransaction("Transaction");
                command.Transaction = transaction;
                querySelect = @"INSERT INTO " + _MA + @" (email,
                                    token,
                                    data_insert) 
                                    VALUES (@email,
                                    @token,
                                    @data_insert)";
                try
                {
                    command.Parameters.Clear();
                    command.Parameters.Add("@email", SqlDbType.VarChar);
                    command.Parameters["@email"].Value = model.email;
                    command.Parameters.Add("@token", SqlDbType.VarChar);
                    command.Parameters["@token"].Value = tokenString;
                    command.Parameters.Add("@data_insert", SqlDbType.DateTime);
                    command.Parameters["@data_insert"].Value = DateTime.Now;

                    command.CommandText = querySelect;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    obj.token = tokenString;

                }

                catch (Exception e)
                {
                    transaction.Rollback();
                    con.Close();
                    throw;
                }
                finally
                {
                    con.Close();
                }
            }
            return obj;
        }
    }

}


