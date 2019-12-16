using Newtonsoft.Json;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Services;
using System;
using System.Web.Services.Protocols;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Services;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;

namespace WebApplication1
{
    /// <summary>
    /// Summary description for HeadPhonesAPI
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class HeadPhonesAPI : System.Web.Services.WebService
    {
        string connStr = ConfigurationManager.ConnectionStrings["sqlConnectionString"].ConnectionString;

        [WebMethod]
        public string Logout(string token)
        {
            var response = new ResponseObject();
            using (var sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                var sqlQuery = "UPDATE [Users] SET [Token] = null, [TokenExpireDate] =null  WHERE [Token] = @token";
                using (var cmd = new SqlCommand(sqlQuery, sqlConn))
                {
                    cmd.Parameters.AddWithValue("@token", token);
                    var obj = cmd.ExecuteNonQuery();
                    if (obj > 0)
                    {
                        response.status = 200;
                        response.data = "Succesfuly loged out";
                    }
                    else
                    {
                        response.status = 500;
                        response.data = "Incorect token";
                    }
                }
            }
            return JsonConvert.SerializeObject(response, Formatting.Indented);
        }

        [WebMethod]
        public string RemoveProduct(int id, string token)
        {
            var response = new ResponseObject();
            var check = CheckToken(token);
            var personalInfo = JsonConvert.DeserializeObject<ResponseObject>(check);
            if (!personalInfo.role)
            {
                response.status = 500;
                response.data = "Incorrect token";
                return JsonConvert.SerializeObject(response, Formatting.Indented);
            }

            UpdateTokenExpireDate(token);
            using (var sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                var sqlQuery = "UPDATE [Products] SET [Active] = 0 WHERE [Id] = @id";
                using (var cmd = new SqlCommand(sqlQuery, sqlConn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    var obj = cmd.ExecuteNonQuery();
                    if (obj > 0)
                    {
                        response.status = 200;
                        response.data = "Succesfuly removed";
                    }
                    else
                    {
                        response.status = 500;
                        response.data = "Incorect id";
                    }
                }
            }
            return JsonConvert.SerializeObject(response, Formatting.Indented);
        }

        [WebMethod]
        public string SaveProduct(ProductModel model, string token)
        {
            var response = new ResponseObject();
            var check = CheckToken(token);

            var personalInfo = JsonConvert.DeserializeObject<ResponseObject>(check);
            var userId = JsonConvert.DeserializeObject<PersonalInfoModel>(personalInfo.data.ToString()).Id;
            if (userId <= 0 || !personalInfo.role)
            {
                response.status = 500;
                response.data = "Incorrect token";
                return JsonConvert.SerializeObject(response, Formatting.Indented);
            }

            UpdateTokenExpireDate(token);
            using (var sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                var sqlQuery = @"INSERT INTO [Products] ([Name]
                               ,[Price]
                               ,[Url]
                               ,[Discount]
                               ,[Details]
                               ,[CreateUser]) VALUES (@name
                               ,@price
                               ,@url
                               ,@discount
                               ,@details
                               ,@createUser)";
                if (model.Id != 0)
                    sqlQuery = @"UPDATE [Products] SET [Name] = @name
                               ,[Price] = @price
                               ,[Url] = @url
                               ,[Discount] = @discount
                               ,[Details] = @details
                             WHERE [Id] = @id";
                using (var cmd = new SqlCommand(sqlQuery, sqlConn))
                {
                    cmd.Parameters.AddWithValue("@name", model.Name);
                    cmd.Parameters.AddWithValue("@price", model.Price);
                    cmd.Parameters.AddWithValue("@url", model.Url);
                    cmd.Parameters.AddWithValue("@discount", model.Discount);
                    cmd.Parameters.AddWithValue("@details", string.IsNullOrEmpty(model.Details) ? (object)DBNull.Value : model.Details);
                    if (model.Id == 0)
                        cmd.Parameters.AddWithValue("@createUser", userId);
                    if (model.Id != 0)
                        cmd.Parameters.AddWithValue("@id", model.Id);

                    var obj = cmd.ExecuteNonQuery();
                    if (obj > 0)
                    {
                        response.status = 200;
                        response.data = "Succesfuly added/updated";
                    }
                    else
                    {
                        response.status = 500;
                        response.data = "The product not added/updated";
                    }
                }
            }
            return JsonConvert.SerializeObject(response, Formatting.Indented);
        }

        [WebMethod]
        public string CreateMailing(MailingModel model, string token)
        {
            var response = new ResponseObject();
            response.status = 200;
            response.data = "Succesfuly added new mailing";
            var check = CheckToken(token);

            var personalInfo = JsonConvert.DeserializeObject<ResponseObject>(check);
            var userId = JsonConvert.DeserializeObject<PersonalInfoModel>(personalInfo.data.ToString()).Id;
            if (userId <= 0 || !personalInfo.role)
            {
                response.status = 500;
                response.data = "Incorrect token";
                return JsonConvert.SerializeObject(response, Formatting.Indented);
            }

            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Mail))
            {
                response.status = 500;
                response.data = "Incorrect Name or Mail";
                return JsonConvert.SerializeObject(response, Formatting.Indented);
            }

            UpdateTokenExpireDate(token);

            var mailingId = CreateMailing(model.Name, userId, model.Mail);
            if (mailingId > 0)
            {
                var addressFrom = new MailAddress("notifications@headphones.com", "Mailing");
                var addressTo = new MailAddress("headphones.shop.ua@gmail.com");
                var message = new MailMessage(addressFrom, addressTo)
                {
                    Subject = model.Name,
                    Body = model.Mail,
                    IsBodyHtml = true

                };

                var sqlQuery = @"SELECT [Id],[Email] FROM [Users] where [Active] = 1 and [Role] = 0 and [Email] is not null";
                using (var sqlConn = new SqlConnection(connStr))
                {
                    sqlConn.Open();
                    using (var cmd = new SqlCommand(sqlQuery, sqlConn))
                    {
                        using (var dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var id = dr.GetInt32(0);
                                var email = dr.GetString(1);

                                if (!AddUserToMailing(id, mailingId))
                                {
                                    response.status = 500;
                                    response.data = "The problem occurred when try to add product to order";
                                    return JsonConvert.SerializeObject(response, Formatting.Indented);
                                }
                                else
                                {
                                    message.Bcc.Add(email);
                                }
                            }
                        }
                    }
                }

                if (message.Bcc.Count > 0)
                {
                    var client = new SmtpClient("smtp.gmail.com", 587)
                    {
                        Credentials = new NetworkCredential("headphones.shop.ua@gmail.com", "Head123!!"),
                        EnableSsl = true
                    };
                    client.Send(message);

                }
            }
            else
            {
                response.status = 500;
                response.data = "The problem occurred when try create mailing";
                return JsonConvert.SerializeObject(response, Formatting.Indented);
            }

            return JsonConvert.SerializeObject(response, Formatting.Indented);
        }


        [WebMethod]
        public string CheckToken(string token)
        {
            var response = new ResponseObject();
            var sqlQuery = "SELECT [Role],[FirstName],[LastName],[Email],[Id] FROM [Users] where [Token] = @token and [TokenExpireDate] > GETDATE() and [Active] = 1";
            using (var sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                using (var cmd = new SqlCommand(sqlQuery, sqlConn))
                {
                    cmd.Parameters.AddWithValue("@token", token);

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            var info = new PersonalInfoModel()
                            {
                                FirstName = dr.IsDBNull(1) ? "" : dr.GetString(1),
                                LastName = dr.IsDBNull(2) ? "" : dr.GetString(2),
                                Email = dr.IsDBNull(3) ? "" : dr.GetString(3),
                                Id = dr.GetInt32(4)
                            };

                            UpdateTokenExpireDate(token);
                            response.status = 200;
                            response.role = dr.IsDBNull(0) ? false : dr.GetInt32(0) == 1;
                            response.data = info;
                        }
                        else
                        {
                            response.status = 500;
                            response.data = "Session expired";
                        }
                    }
                }
            }
            return JsonConvert.SerializeObject(response, Formatting.Indented);
        }

        [WebMethod]
        public string Login(LoginModel model)
        {
            var response = new ResponseObject();

            var sqlQuery = "SELECT * FROM [Users] where [Login] = @login and [Password] = @password";
            using (var sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                using (var cmd = new SqlCommand(sqlQuery, sqlConn))
                {
                    cmd.Parameters.AddWithValue("@login", model.Login);
                    cmd.Parameters.AddWithValue("@password", HashPassword(model.Password));
                    //cmd.Parameters.AddWithValue("@password", model.Password);

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            response.status = 200;
                            response.role = dr.IsDBNull(0) ? false : dr.GetInt32(0) == 1;
                            response.data = GenerateToken(model.Login);
                        }
                        else
                        {
                            response.status = 500;
                            response.data = "Login or password is not valid";
                        }
                    }
                }
            }
            return JsonConvert.SerializeObject(response, Formatting.Indented);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetProducts(int page, string search)
        {
            var itemsPerPage = 6;
            var skip = (page - 1) * itemsPerPage;

            var response = new ResponseObject();
            var data = new ProductsResponseModel();
            var list = new List<ProductModel>();

            var whereQuery = string.IsNullOrEmpty(search) ? "" : " and [Name] like @name ";

            var sqlQuery = @"SELECT [Id]
                            ,[Name]
                            ,[Price]
                            ,[Url]
                            ,[Discount]
                            ,[Details] 
                            FROM [Products] where [Active] = 1" + whereQuery + @"
                            ORDER BY Id OFFSET (" + skip.ToString() + ") ROWS FETCH NEXT (" + itemsPerPage.ToString() + ") ROWS ONLY";
            using (var sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                using (var cmd = new SqlCommand(sqlQuery, sqlConn))
                {
                    if (!string.IsNullOrEmpty(whereQuery))
                        cmd.Parameters.AddWithValue("@name", "%" + search + "%");
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new ProductModel()
                            {
                                Id = dr.GetInt32(0),
                                Name = dr.IsDBNull(1) ? "" : dr.GetString(1),
                                Price = dr.IsDBNull(2) ? 0 : dr.GetDecimal(2),
                                Url = dr.IsDBNull(3) ? "" : dr.GetString(3),
                                Discount = dr.IsDBNull(4) ? 0 : dr.GetInt32(4),
                                Details = dr.IsDBNull(5) ? "" : dr.GetString(5)

                            });
                        }
                        data.Products = list;
                    }
                }
            }

            var sqlQueryCount = @"SELECT COUNT([Id])
                            FROM [Products] where [Active] = 1" + whereQuery;
            using (var sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                using (var cmd = new SqlCommand(sqlQueryCount, sqlConn))
                {
                    if (!string.IsNullOrEmpty(whereQuery))
                        cmd.Parameters.AddWithValue("@name", search);
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            data.Pages = (dr.GetInt32(0) + itemsPerPage - 1) / itemsPerPage;
                        }

                        response.status = 200;
                        response.data = data;
                    }
                }
            }

            return JsonConvert.SerializeObject(response, Formatting.Indented);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetOrders(int page, string token)
        {
            var itemsPerPage = 20;
            var skip = (page - 1) * itemsPerPage;
            var response = new ResponseObject();
            var check = CheckToken(token);

            var personalInfo = JsonConvert.DeserializeObject<ResponseObject>(check);
            if (personalInfo.status != 200)
                return JsonConvert.SerializeObject(personalInfo, Formatting.Indented);

            var userId = JsonConvert.DeserializeObject<PersonalInfoModel>(personalInfo.data.ToString()).Id;
            if (userId <= 0)
            {
                response.status = 500;
                response.data = "Incorrect token";
                return JsonConvert.SerializeObject(response, Formatting.Indented);
            }

            UpdateTokenExpireDate(token);
            var list = new List<OrderModel>();
            var data = new OrdersResponseModel();

            var whereQuery = personalInfo.role ? "" : " where o.[UserId] = @userId ";
            var sqlQuery = @"SELECT o.[Id]
                              ,u.Login
                              ,[TotalPrice]
                              ,[Status]
                              ,[Note]
                              ,o.[CreateDate]
                            FROM [Orders] o
                            inner join [Users] u on u.Id = o.UserId" + whereQuery + @"
                            ORDER BY o.Id OFFSET (" + skip.ToString() + ") ROWS FETCH NEXT (" + itemsPerPage.ToString() + ") ROWS ONLY";
            using (var sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                using (var cmd = new SqlCommand(sqlQuery, sqlConn))
                {
                    if (!string.IsNullOrEmpty(whereQuery))
                        cmd.Parameters.AddWithValue("@userId", userId);
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new OrderModel()
                            {
                                Id = dr.GetInt32(0),
                                User = dr.IsDBNull(1) ? "" : dr.GetString(1),
                                TotalPrice = dr.IsDBNull(2) ? 0 : dr.GetDecimal(2),
                                Status = dr.IsDBNull(3) ? "" : dr.GetString(3),
                                Note = dr.IsDBNull(4) ? "" : dr.GetString(4),
                                CreateDate = dr.IsDBNull(5) ? "" : dr.GetDateTime(5).ToShortDateString()

                            });
                        }
                        data.Orders = list;
                    }
                }
            }

            var sqlQueryCount = @"SELECT COUNT(o.[Id])
                                    FROM [Orders] o
                                    inner join [Users] u on u.Id = o.UserId" + whereQuery;
            using (var sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                using (var cmd = new SqlCommand(sqlQueryCount, sqlConn))
                {
                    if (!string.IsNullOrEmpty(whereQuery))
                        cmd.Parameters.AddWithValue("@userId", userId);
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            data.Pages = (dr.GetInt32(0) + itemsPerPage - 1) / itemsPerPage;
                        }

                        response.status = 200;
                        response.data = data;
                    }
                }
            }

            return JsonConvert.SerializeObject(response, Formatting.Indented);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetMailings(string token)
        {
            var response = new ResponseObject();
            var check = CheckToken(token);

            var personalInfo = JsonConvert.DeserializeObject<ResponseObject>(check);
            if (personalInfo.status != 200)
                return JsonConvert.SerializeObject(personalInfo, Formatting.Indented);

            var userId = JsonConvert.DeserializeObject<PersonalInfoModel>(personalInfo.data.ToString()).Id;
            if (userId <= 0 || !personalInfo.role)
            {
                response.status = 500;
                response.data = "Incorrect token";
                return JsonConvert.SerializeObject(response, Formatting.Indented);
            }

            UpdateTokenExpireDate(token);
            var list = new List<MailingModel>();

            var sqlQuery = @"SELECT  m.[Id]
                          ,MIN([Name])
                          ,MIN([Mail])
                          ,MIN(m.[CreateDate])
                          ,MIN(u.[Login])
	                      ,COUNT(mu.id)
                      FROM [Mailing] m
                      inner join [MailingUsers] mu on mu.MailingId = m.Id
                      inner join [Users] u on u.Id = m.CreateUser
                      group by m.[Id]
                      order by m.[Id]";
            using (var sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                using (var cmd = new SqlCommand(sqlQuery, sqlConn))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new MailingModel()
                            {
                                Id = dr.GetInt32(0),
                                Name = dr.IsDBNull(1) ? "" : dr.GetString(1),
                                Mail = dr.IsDBNull(2) ? "" : dr.GetString(2),
                                CreateDate = dr.IsDBNull(3) ? "" : dr.GetDateTime(3).ToShortDateString(),
                                CreateUser = dr.IsDBNull(4) ? "" : dr.GetString(4),
                                UsersCount = dr.IsDBNull(5) ? 0 : dr.GetInt32(5)

                            });
                        }
                    }
                }
            }

            response.status = 200;
            response.data = list;

            return JsonConvert.SerializeObject(response, Formatting.Indented);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetUsersByMailing(string token, int mailingId)
        {
            var response = new ResponseObject();
            var check = CheckToken(token);

            var personalInfo = JsonConvert.DeserializeObject<ResponseObject>(check);
            if (personalInfo.status != 200)
                return JsonConvert.SerializeObject(personalInfo, Formatting.Indented);

            var userId = JsonConvert.DeserializeObject<PersonalInfoModel>(personalInfo.data.ToString()).Id;
            if (userId <= 0 || !personalInfo.role)
            {
                response.status = 500;
                response.data = "Incorrect token";
                return JsonConvert.SerializeObject(response, Formatting.Indented);
            }

            UpdateTokenExpireDate(token);
            var list = new List<PersonalInfoModel>();

            var sqlQuery = @"SELECT u.[Id]
                          ,u.[FirstName]
                          ,u.[LastName]
                          ,u.[Email]
                      FROM [MailingUsers] mu  
                      inner join [Users] u on u.Id = mu.UserId
                      WHERE mu.MailingId = @mailingId";
            using (var sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                using (var cmd = new SqlCommand(sqlQuery, sqlConn))
                {
                    cmd.Parameters.AddWithValue("@mailingId", mailingId);
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new PersonalInfoModel()
                            {
                                Id = dr.GetInt32(0),
                                FirstName = dr.IsDBNull(1) ? "" : dr.GetString(1),
                                LastName = dr.IsDBNull(2) ? "" : dr.GetString(2),
                                Email = dr.IsDBNull(3) ? "" : dr.GetString(3),

                            });
                        }
                    }
                }
            }

            response.status = 200;
            response.data = list;

            return JsonConvert.SerializeObject(response, Formatting.Indented);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetProductsByIds(List<int> ids)
        {
            var response = new ResponseObject();
            var list = new List<ProductModel>();

            foreach (var id in ids)
            {
                var product = GetProductById(id);
                if (product != null)
                    list.Add(product);
            }

            response.status = 200;
            response.data = list;
            return JsonConvert.SerializeObject(response, Formatting.Indented);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetProductsByOrder(int orderId, string token)
        {
            var response = new ResponseObject();
            var list = new List<ProductModel>();

            var check = CheckToken(token);
            var personalInfo = JsonConvert.DeserializeObject<ResponseObject>(check);
            var userId = JsonConvert.DeserializeObject<PersonalInfoModel>(personalInfo.data.ToString()).Id;
            if (userId <= 0)
            {
                response.status = 500;
                response.data = "Incorrect token";
                return JsonConvert.SerializeObject(response, Formatting.Indented);
            }

            var sqlQuery = @"SELECT op.ProductId,
                                    p.[Name],
                                    op.[Price],
                                    p.[Url],
                                    op.[Discount],
                                    p.[Details],
                                    o.UserId
                              FROM [Orders] o
                              inner join [OrdersProducts] op on op.OrderId = o.Id
                              inner join [Products] p on p.Id =op.ProductId
                              where o.Id = @orderId";
            using (var sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                using (var cmd = new SqlCommand(sqlQuery, sqlConn))
                {
                    cmd.Parameters.AddWithValue("@orderId", orderId);
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (dr.GetInt32(6) != userId && !personalInfo.role)
                            {
                                response.status = 500;
                                response.data = "Not allowed";
                                return JsonConvert.SerializeObject(response, Formatting.Indented);
                            }

                            list.Add(new ProductModel()
                            {
                                Id = dr.GetInt32(0),
                                Name = dr.IsDBNull(1) ? "" : dr.GetString(1),
                                Price = dr.IsDBNull(2) ? 0 : dr.GetDecimal(2),
                                Url = dr.IsDBNull(3) ? "" : dr.GetString(3),
                                Discount = dr.IsDBNull(4) ? 0 : dr.GetInt32(4),
                                Details = dr.IsDBNull(5) ? "" : dr.GetString(5)

                            });
                        }
                    }
                }
            }

            response.status = 200;
            response.data = list;
            return JsonConvert.SerializeObject(response, Formatting.Indented);
        }

        [WebMethod]
        public string CreateOrder(List<int> ids, string token, decimal price)
        {
            var response = new ResponseObject();
            var list = new List<ProductModel>();

            var check = CheckToken(token);
            var personalInfo = JsonConvert.DeserializeObject<ResponseObject>(check);
            var userId = JsonConvert.DeserializeObject<PersonalInfoModel>(personalInfo.data.ToString()).Id;
            if (userId <= 0 || personalInfo.role)
            {
                response.status = 500;
                response.data = "Incorrect token";
                return JsonConvert.SerializeObject(response, Formatting.Indented);
            }

            UpdateTokenExpireDate(token);
            foreach (var id in ids)
            {
                var product = GetProductById(id);
                if (product != null)
                    list.Add(product);
            }

            var total = list.Sum(element => Math.Round(element.Discount != 0 ? element.Price * (100 - element.Discount) / 100 : element.Price, 2));
            if (total != price)
            {
                response.status = 500;
                response.data = "The price of some products changed, please refresh the page";
                return JsonConvert.SerializeObject(response, Formatting.Indented);
            }

            var orderId = CreateOrder(userId, total);
            if (orderId > 0)
            {
                foreach (var product in list)
                {
                    if (!AddProductToOrder(product, orderId))
                    {
                        response.status = 500;
                        response.data = "The problem occurred when try to add product to order";
                        return JsonConvert.SerializeObject(response, Formatting.Indented);

                    }
                }
            }

            response.status = 200;
            response.data = "Your order successfully registered";
            return JsonConvert.SerializeObject(response, Formatting.Indented);
        }

        [WebMethod]
        public string ChangeOrderStatus(int orderId, string status, string token)
        {
            var response = new ResponseObject();
            var check = CheckToken(token);
            var personalInfo = JsonConvert.DeserializeObject<ResponseObject>(check);
            var userId = JsonConvert.DeserializeObject<PersonalInfoModel>(personalInfo.data.ToString()).Id;
            if ((!status.Equals("Canceled") && !status.Equals("Approved")) || (!personalInfo.role && !status.Equals("Canceled")))
            {
                response.status = 500;
                response.data = "Incorrect status";
                return JsonConvert.SerializeObject(response, Formatting.Indented);
            }

            UpdateTokenExpireDate(token);
            using (var sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                var sqlQuery = "UPDATE [Orders] SET [Status] = @status, [ChangeStatusDate] = GETDATE(), [ChangedById] = @userId WHERE [Id] = @id";
                using (var cmd = new SqlCommand(sqlQuery, sqlConn))
                {
                    cmd.Parameters.AddWithValue("@id", orderId);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    var obj = cmd.ExecuteNonQuery();
                    if (obj > 0)
                    {
                        response.status = 200;
                        response.data = "Succesfuly updated";
                    }
                    else
                    {
                        response.status = 500;
                        response.data = "Nothing to update";
                    }
                }
            }
            return JsonConvert.SerializeObject(response, Formatting.Indented);
        }

        [WebMethod]
        public string Register(RegisterModel model)
        {
            var response = new ResponseObject { status = 500 };

            if (!ValidateLogin(model.Login))
            {
                response.data = "Incorect Login";
                return JsonConvert.SerializeObject(response, Formatting.Indented);
            }

            if (!ValidateEmail(model.Email))
            {
                response.data = "Incorect Email";
                return JsonConvert.SerializeObject(response, Formatting.Indented);
            }

            if (!ValidatePassword(model.Password))
            {
                response.data = "Incorect Password";
                return JsonConvert.SerializeObject(response, Formatting.Indented);
            }

            using (var sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();

                var sqlQuery = "SELECT * FROM [Users] where [Login] = @login";
                using (var cmd = new SqlCommand(sqlQuery, sqlConn))
                {
                    cmd.Parameters.AddWithValue("@login", model.Login);
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            response.data = "Current login already used";
                            return JsonConvert.SerializeObject(response, Formatting.Indented);
                        }
                    }
                }

                sqlQuery = "SELECT * FROM [Users] where [Email] = @email";
                using (var cmd = new SqlCommand(sqlQuery, sqlConn))
                {
                    cmd.Parameters.AddWithValue("@email", model.Email);
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            response.status = 500;
                            response.data = "Current email already used";
                            return JsonConvert.SerializeObject(response, Formatting.Indented);
                        }
                    }
                }

                sqlQuery = "INSERT INTO [Users] ([Login],[Password],[Email],[FirstName],[LastName]) VALUES (@login,@password,@email,@firstName,@lastName)";
                using (var cmd = new SqlCommand(sqlQuery, sqlConn))
                {
                    cmd.Parameters.AddWithValue("@login", model.Login);
                    cmd.Parameters.AddWithValue("@password", HashPassword(model.Password));
                    cmd.Parameters.AddWithValue("@email", model.Email);
                    cmd.Parameters.AddWithValue("@firstName", model.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", model.LastName);
                    var obj = cmd.ExecuteNonQuery();
                    if (obj < 1)
                    {
                        response.data = "Registration error, please try egain";
                        return JsonConvert.SerializeObject(response, Formatting.Indented);
                    }
                }
            }

            response.status = 200;
            response.data = GenerateToken(model.Login);
            return JsonConvert.SerializeObject(response, Formatting.Indented);
        }


        public int CreateOrder(int userId, decimal total)
        {
            var orderId = 0;
            using (var sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                var sqlQuery = @"INSERT INTO [Orders] ([UserId],[TotalPrice],[Status]) 
                                OUTPUT Inserted.Id
                                VALUES (@userId,@totalPrice,'New')";
                using (var cmd = new SqlCommand(sqlQuery, sqlConn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@totalPrice", total);
                    orderId = int.Parse(cmd.ExecuteScalar().ToString());
                }
            }
            return orderId;
        }

        public int CreateMailing(string name, int userId, string mail)
        {
            var mailingId = 0;
            using (var sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                var sqlQuery = @"INSERT INTO [Mailing] ([Name]
                                ,[Mail]
                                ,[CreateUser]) 
                                OUTPUT Inserted.Id
                                VALUES (@name,@mail,@createUser)";
                using (var cmd = new SqlCommand(sqlQuery, sqlConn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@mail", mail);
                    cmd.Parameters.AddWithValue("@createUser", userId);
                    mailingId = int.Parse(cmd.ExecuteScalar().ToString());
                }
            }
            return mailingId;
        }

        public bool AddUserToMailing(int userId, int mailingId)
        {
            using (var sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                var sqlQuery = @"INSERT INTO [MailingUsers] ([MailingId],[UserId]) 
                                VALUES (@mailingId,@userId)";
                using (var cmd = new SqlCommand(sqlQuery, sqlConn))
                {
                    cmd.Parameters.AddWithValue("@mailingId", mailingId);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    var obj = cmd.ExecuteNonQuery();
                    if (obj > 0)
                        return true;
                }
            }
            return false;
        }
        public bool AddProductToOrder(ProductModel product, int orderId)
        {
            using (var sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                var sqlQuery = @"INSERT INTO [OrdersProducts] ([OrderId],[ProductId],[Price],[Discount]) 
                                OUTPUT Inserted.Id
                                VALUES (@orderId,@productId,@price,@discount)";
                using (var cmd = new SqlCommand(sqlQuery, sqlConn))
                {
                    cmd.Parameters.AddWithValue("@orderId", orderId);
                    cmd.Parameters.AddWithValue("@productId", product.Id);
                    cmd.Parameters.AddWithValue("@price", product.Price);
                    cmd.Parameters.AddWithValue("@discount", product.Discount);
                    var obj = cmd.ExecuteNonQuery();
                    if (obj > 0)
                        return true;
                }
            }
            return false;
        }

        public ProductModel GetProductById(int id)
        {
            var response = new ResponseObject();
            var list = new List<ProductModel>();
            var sqlQuery = @"SELECT [Id]
                            ,[Name]
                            ,[Price]
                            ,[Url]
                            ,[Discount]
                            ,[Details] 
                            FROM [Products] where [Active] = 1 and [Id] = @id";
            using (var sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                using (var cmd = new SqlCommand(sqlQuery, sqlConn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return new ProductModel()
                            {
                                Id = dr.GetInt32(0),
                                Name = dr.IsDBNull(1) ? "" : dr.GetString(1),
                                Price = dr.IsDBNull(2) ? 0 : dr.GetDecimal(2),
                                Url = dr.IsDBNull(3) ? "" : dr.GetString(3),
                                Discount = dr.IsDBNull(4) ? 0 : dr.GetInt32(4),
                                Details = dr.IsDBNull(5) ? "" : dr.GetString(5)

                            };
                        }
                    }
                }
            }
            return null;
        }

        private void UpdateTokenExpireDate(string token)
        {
            using (var sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                var sqlQuery = "UPDATE [Users] SET [TokenExpireDate] = @expireDate  WHERE [token] = @token";
                using (var cmd = new SqlCommand(sqlQuery, sqlConn))
                {
                    cmd.Parameters.AddWithValue("@token", token);
                    cmd.Parameters.AddWithValue("@expireDate", DateTime.Now.AddMinutes(10));
                    var obj = cmd.ExecuteNonQuery();
                }
            }
        }

        private string GenerateToken(string login)
        {
            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            using (var sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                var sqlQuery = "UPDATE [Users] SET [Token] = @token, [TokenExpireDate] =@expireDate  WHERE [Login] = @login";
                using (var cmd = new SqlCommand(sqlQuery, sqlConn))
                {
                    cmd.Parameters.AddWithValue("@login", login);
                    cmd.Parameters.AddWithValue("@token", token);
                    cmd.Parameters.AddWithValue("@expireDate", DateTime.Now.AddMinutes(10));
                    var obj = cmd.ExecuteNonQuery();
                }
            }
            return token;
        }

        private bool ValidatePassword(string password)
        {
            var reg = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}");
            return !string.IsNullOrEmpty(password) && reg.IsMatch(password);
        }

        private bool ValidateEmail(string email)
        {
            var reg = new Regex(@"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$");
            return !string.IsNullOrEmpty(email) && reg.IsMatch(email);
        }

        private bool ValidateLogin(string login)
        {
            return !string.IsNullOrEmpty(login) && login.Length >= 6;
        }

        private string HashPassword(string password)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            return System.Text.Encoding.ASCII.GetString(data);
        }
    }

    public class LoginModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class ResponseObject
    {
        public int status { get; set; }
        public object data { get; set; }
        public bool role { get; set; }
    }

    public class PersonalInfoModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }


    public class ProductsResponseModel
    {
        public List<ProductModel> Products { get; set; }
        public int Pages { get; set; }
    }

    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Url { get; set; }
        public string Details { get; set; }
        public int Discount { get; set; }
    }

    public class OrderModel
    {
        public int Id { get; set; }
        public string User { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string CreateDate { get; set; }
    }

    public class MailingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public int UsersCount { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
    }

    public class OrdersResponseModel
    {
        public List<OrderModel> Orders { get; set; }
        public int Pages { get; set; }
    }
}
