using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

using Npgsql;
using System.IO;

namespace Avaliacao
{
    internal class Program
    {
        async Task Iniciar()
        {
            try
            {
                Conexao();

                CreateTable();

                await InsertTable();

                Relatorio();
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }

        }
        static async Task Main(string[] args)
        {
            Program programa = new Program();
            await programa.Iniciar();
        }

        void Relatorio()
        {
            try
            {
                var users = new List<User>();

                users = GetAllUsers();

                foreach (var item in users)
                {
                    Log($"{item.Id} - {item.Username} - {item.Email}");
                }
            } catch (Exception ex)
            {
                Log(ex.ToString());
            }
        }
        
        public async Task<JArray> GetRandomUsersAsync(int count = 1)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync($"https://randomuser.me/api/?results={count}");
                var users = JObject.Parse(response)["results"] as JArray;
                return users;
            }
        }


        //static string connString = "Host=localhost;Username=postgres;Password=76577614;Database=AVALIACAO_PROD";

        static string connString = "Host=localhost;Username=postgres;Password=76577614;Database=postgres";
        void Conexao()
        {
            //var connString = "Host=localhost;Username=postgres;Password=76577614;Database=AVALIACAO_PROD";

            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    // Utilize a conexão
                }
            }catch  (Exception ex)
            {
                ExitApplication(ex.ToString());
            }
        }

        void ExitApplication(string msg)
        {
            try {
                Log(msg.ToString());
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }

        }

        static void CreateTable()
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Users (
                    Id SERIAL PRIMARY KEY,
                    Gender VARCHAR(10),
                    Name_Title VARCHAR(50),
                    Name_FirstName VARCHAR(50),
                    Name_LastName VARCHAR(50),
                    Location_Street_Number INTEGER,
                    Location_Street_Name VARCHAR(50),
                    Location_City VARCHAR(50),
                    Location_State VARCHAR(50),
                    Location_Country VARCHAR(50),
                    Location_Pastcode VARCHAR(50),
                    Location_Coordinates_Latitude VARCHAR(20),
                    Location_Coordinates_Logintude VARCHAR(20),
                    Location_Timezone_Offset VARCHAR(20),
                    Email VARCHAR(100),
                    Login_uuid VARCHAR(50),
                    Login_Username VARCHAR(50),
                    Login_Password VARCHAR(50),
                    Login_Salt VARCHAR(50),
                    Login_md5 VARCHAR(50),
                    Login_Sha1 VARCHAR(50),
                    Login_Sha256 VARCHAR(100),
                    Dob_Date DATE,
                    Dob_Age INTEGER,
                    Registered_Date DATE,
                    Registered_Age INTEGER,
                    Phone VARCHAR(20),
                    Cell VARCHAR(20),
                    Id_Name VARCHAR(20),
                    Id_Value VARCHAR(20),
                    Picture_Large VARCHAR(255),
                    Picture_Medium VARCHAR(255),
                    Picture_Thumbnail VARCHAR(255),
                    Nat VARCHAR(20)
                    )";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        bool UserExists(string email, string uuid)
        {

            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    // Verificar se o usuário já existe pelo nome de usuário ou endereço de e-mail
                    using (var checkCmd = new NpgsqlCommand())
                    {
                        checkCmd.Connection = conn;
                        checkCmd.CommandText = @"
                    SELECT COUNT(*)
                    FROM Users
                    WHERE Email = @email OR Login_uuid = @uuid";
                        checkCmd.Parameters.AddWithValue("email", email);
                        checkCmd.Parameters.AddWithValue("uuid", uuid);

                        object result = checkCmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            int count;
                            if (int.TryParse(result.ToString(), out count))
                            {
                                if (count > 0)
                                {
                                    Log($"Usuário {email} já cadastrados na base!");
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                // Tratar erro de conversão
                                ExitApplication("Erro na conversão do resultado para int");
                                return false;
                            }
                        }
                        else
                        {
                            // Tratar valor nulo retornado
                            Console.WriteLine("Valor nulo retornado");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"{ex.ToString()}");
                return true;
            }
        }
        async Task InsertTable()
        {
            try
            {
                var users = await GetRandomUsersAsync(10);

                foreach (var user in users)
                {
                    var gender = user["gender"].ToString();
                    var nameTitle = user["name"]["title"].ToString();
                    var firstName = user["name"]["first"].ToString();
                    var lastName = user["name"]["last"].ToString();
                    var streetNumber = Convert.ToInt32(user["location"]["street"]["number"]);
                    var streetName = user["location"]["street"]["name"].ToString();
                    var city = user["location"]["city"].ToString();
                    var state = user["location"]["state"].ToString();
                    var country = user["location"]["country"].ToString();
                    var postcode = user["location"]["postcode"].ToString();
                    var latitude = user["location"]["coordinates"]["latitude"].ToString();
                    var longitude = user["location"]["coordinates"]["longitude"].ToString();
                    var timezoneOffset = user["location"]["timezone"]["offset"].ToString();
                    var email = user["email"].ToString();
                    var uuid = user["login"]["uuid"].ToString();
                    var username = user["login"]["username"].ToString();
                    var password = user["login"]["password"].ToString();
                    var salt = user["login"]["salt"].ToString();
                    var md5 = user["login"]["md5"].ToString();
                    var sha1 = user["login"]["sha1"].ToString();
                    var sha256 = user["login"]["sha256"].ToString();
                    var dobDate = DateTime.Parse(user["dob"]["date"].ToString());
                    var dobAge = Convert.ToInt32(user["dob"]["age"]);
                    var registeredDate = DateTime.Parse(user["registered"]["date"].ToString());
                    var registeredAge = Convert.ToInt32(user["registered"]["age"]);
                    var phone = user["phone"].ToString();
                    var cell = user["cell"].ToString();
                    var idName = user["id"]["name"].ToString();
                    var idValue = user["id"]["value"].ToString();
                    var largePicture = user["picture"]["large"].ToString();
                    var mediumPicture = user["picture"]["medium"].ToString();
                    var thumbnailPicture = user["picture"]["thumbnail"].ToString();
                    var nat = user["nat"].ToString();

                    if (!UserExists(email, uuid))
                    {

                        using (var conn = new NpgsqlConnection(connString))
                        {
                            conn.Open();

                            using (var cmd = new NpgsqlCommand())
                            {
                                cmd.Connection = conn;
                                cmd.CommandText = @"
                    INSERT INTO Users (
                        Gender, Name_Title, Name_FirstName, Name_LastName,
                        Location_Street_Number, Location_Street_Name, Location_City, Location_State, Location_Country, Location_Pastcode,
                        Location_Coordinates_Latitude, Location_Coordinates_Logintude, Location_Timezone_Offset,
                        Email, Login_uuid, Login_Username, Login_Password, Login_Salt, Login_md5, Login_Sha1, Login_Sha256,
                        Dob_Date, Dob_Age, Registered_Date, Registered_Age,
                        Phone, Cell, Id_Name, Id_Value,
                        Picture_Large, Picture_Medium, Picture_Thumbnail, Nat
                    ) 
                    VALUES (
                        @gender, @nameTitle, @firstName, @lastName,
                        @streetNumber, @streetName, @city, @state, @country, @postcode,
                        @latitude, @longitude, @timezoneOffset,
                        @email, @uuid, @username, @password, @salt, @md5, @sha1, @sha256,
                        @dobDate, @dobAge, @registeredDate, @registeredAge,
                        @phone, @cell, @idName, @idValue,
                        @largePicture, @mediumPicture, @thumbnailPicture, @nat
                    )";
                                cmd.Parameters.AddWithValue("firstName", firstName);
                                cmd.Parameters.AddWithValue("lastName", lastName);
                                cmd.Parameters.AddWithValue("email", email);
                                cmd.Parameters.AddWithValue("username", username);
                                cmd.Parameters.AddWithValue("gender", gender);
                                cmd.Parameters.AddWithValue("nameTitle", nameTitle);
                                cmd.Parameters.AddWithValue("streetNumber", streetNumber);
                                cmd.Parameters.AddWithValue("streetName", streetName);
                                cmd.Parameters.AddWithValue("city", city);
                                cmd.Parameters.AddWithValue("state", state);
                                cmd.Parameters.AddWithValue("country", country);
                                cmd.Parameters.AddWithValue("postcode", postcode);
                                cmd.Parameters.AddWithValue("latitude", latitude);
                                cmd.Parameters.AddWithValue("longitude", longitude);
                                cmd.Parameters.AddWithValue("timezoneOffset", timezoneOffset);
                                cmd.Parameters.AddWithValue("uuid", uuid);
                                cmd.Parameters.AddWithValue("password", password);
                                cmd.Parameters.AddWithValue("salt", salt);
                                cmd.Parameters.AddWithValue("md5", md5);
                                cmd.Parameters.AddWithValue("sha1", sha1);
                                cmd.Parameters.AddWithValue("sha256", sha256);
                                cmd.Parameters.AddWithValue("dobDate", dobDate);
                                cmd.Parameters.AddWithValue("dobAge", dobAge);
                                cmd.Parameters.AddWithValue("registeredDate", registeredDate);
                                cmd.Parameters.AddWithValue("registeredAge", registeredAge);
                                cmd.Parameters.AddWithValue("phone", phone);
                                cmd.Parameters.AddWithValue("cell", cell);
                                cmd.Parameters.AddWithValue("idName", idName);
                                cmd.Parameters.AddWithValue("idValue", idValue);
                                cmd.Parameters.AddWithValue("largePicture", largePicture);
                                cmd.Parameters.AddWithValue("mediumPicture", mediumPicture);
                                cmd.Parameters.AddWithValue("thumbnailPicture", thumbnailPicture);
                                cmd.Parameters.AddWithValue("nat", nat);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    else
                    {
                        Log($"Usuário {firstName} - Uuid {uuid} já existe na base de dados");
                    }
                }
            }catch(Exception ex)
            {
                ExitApplication(ex.ToString());

            }
        }

        public void Log(string message)
        {

            
            try
            {
                Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] {message}");

                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string logDirectory = Path.Combine(baseDirectory, "log", DateTime.Now.ToString("yyyy/MM/dd"));
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                string logFilePath = Path.Combine(logDirectory, "log.txt");

                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] {message}");
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao gravar o log: {ex.Message}");
            }
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("SELECT * FROM Users", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("Name_FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("Name_LastName")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Username = reader.GetString(reader.GetOrdinal("Login_Username")),
                            Gender = reader.GetString(reader.GetOrdinal("Gender")),
                            DateOfBirth = reader.GetDateTime(reader.GetOrdinal("Dob_Date"))
                        });
                    }
                }
            }

            return users;
        }
    }


    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }



}
