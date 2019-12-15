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

        Server1.HeadPhonesAPI server1 = new Server1.HeadPhonesAPI();
        Server2.HeadPhonesAPI server2 = new Server2.HeadPhonesAPI();
        Random random = new Random();

        [WebMethod]
        public string Logout(string token)
        {
            var r = random.Next(0, 2);
            switch (r)
            {
                case 0:
                    return server1.Logout(token);
                case 1:
                    return server2.Logout(token);
                default:
                    return server1.Logout(token);
            }
        }

        [WebMethod]
        public string RemoveProduct(int id, string token)
        {
            var r = random.Next(0, 2);
            switch (r)
            {
                case 0:
                    return server1.RemoveProduct(id, token);
                case 1:
                    return server2.RemoveProduct(id, token);
                default:
                    return server1.RemoveProduct(id, token);
            }
        }

        [WebMethod]
        public string SaveProduct(Server1.ProductModel model, string token)
        {
            var r = random.Next(0, 2);
            switch (r)
            {
                case 0:
                    return server1.SaveProduct(model, token);
                case 1:
                    return server2.SaveProduct(new Server2.ProductModel() {
                        Details = model.Details,
                        Discount = model.Discount,
                        Price = model.Price,
                        Url = model.Url,
                        Id = model.Id,
                        Name = model.Name
                    }, token);
                default:
                    return server1.SaveProduct(model, token);
            }
        }

        [WebMethod]
        public string CreateMailing(Server1.MailingModel model, string token)
        {
            var r = random.Next(0, 2);
            switch (r)
            {
                case 0:
                    return server1.CreateMailing(model, token);
                case 1:
                    return server2.CreateMailing(new Server2.MailingModel()
                    {
                        CreateUser = model.CreateUser,
                        CreateDate = model.CreateDate,
                        UsersCount = model.UsersCount,
                        Mail = model.Mail,
                        Id = model.Id,
                        Name = model.Name
                    }, token);
                default:
                    return server1.CreateMailing(model, token);
            }
        }

        [WebMethod]
        public string CheckToken(string token)
        {
            var r = random.Next(0, 2);
            switch (r)
            {
                case 0:
                    return server1.CheckToken(token);
                case 1:
                    return server2.CheckToken(token);
                default:
                    return server1.CheckToken(token);
            }
        }

        [WebMethod]
        public string Login(Server1.LoginModel model)
        {
            var r = random.Next(0, 2);
            switch (r)
            {
                case 0:
                    return server1.Login(model);
                case 1:
                    return server2.Login(new Server2.LoginModel()
                    {
                        Login = model.Login,
                        Password = model.Password
                    });
                default:
                    return server1.Login(model);
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetProducts(int page, string search)
        {
            var r = random.Next(0, 2);
            switch (r)
            {
                case 0:
                    return server1.GetProducts(page, search);
                case 1:
                    return server2.GetProducts(page, search);
                default:
                    return server1.GetProducts(page, search);
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetOrders(int page, string token)
        {
            var r = random.Next(0, 2);
            switch (r)
            {
                case 0:
                    return server1.GetOrders(page, token);
                case 1:
                    return server2.GetOrders(page, token);
                default:
                    return server1.GetOrders(page, token);
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetMailings(string token)
        {
            var r = random.Next(0, 2);
            switch (r)
            {
                case 0:
                    return server1.GetMailings(token);
                case 1:
                    return server2.GetMailings(token);
                default:
                    return server1.GetMailings(token);
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetUsersByMailing(string token, int mailingId)
        {
            var r = random.Next(0, 2);
            switch (r)
            {
                case 0:
                    return server1.GetUsersByMailing(token, mailingId);
                case 1:
                    return server2.GetUsersByMailing(token, mailingId);
                default:
                    return server1.GetUsersByMailing(token, mailingId);
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetProductsByIds(List<int> ids)
        {
            var r = random.Next(0, 2);
            switch (r)
            {
                case 0:
                    return server1.GetProductsByIds(ids.ToArray());
                case 1:
                    return server2.GetProductsByIds(ids.ToArray());
                default:
                    return server1.GetProductsByIds(ids.ToArray());
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetProductsByOrder(int orderId, string token)
        {
            var r = random.Next(0, 2);
            switch (r)
            {
                case 0:
                    return server1.GetProductsByOrder(orderId, token);
                case 1:
                    return server2.GetProductsByOrder(orderId, token);
                default:
                    return server1.GetProductsByOrder(orderId, token);
            }
        }

        [WebMethod]
        public string CreateOrder(List<int> ids, string token, decimal price)
        {
            var r = random.Next(0, 2);
            switch (r)
            {
                case 0:
                    return server1.CreateOrder(ids.ToArray(), token,price);
                case 1:
                    return server2.CreateOrder(ids.ToArray(), token, price);
                default:
                    return server1.CreateOrder(ids.ToArray(), token, price);
            }
        }

        [WebMethod]
        public string ChangeOrderStatus(int orderId, string status, string token)
        {
            var r = random.Next(0, 2);
            switch (r)
            {
                case 0:
                    return server1.ChangeOrderStatus(orderId,  status,  token);
                case 1:
                    return server2.ChangeOrderStatus(orderId, status, token);
                default:
                    return server1.ChangeOrderStatus(orderId, status, token);
            }
        }

        [WebMethod]
        public string Register(Server1.RegisterModel model)
        {
            var r = random.Next(0, 2);
            switch (r)
            {
                case 0:
                    return server1.Register(model);
                case 1:
                    return server2.Register(new Server2.RegisterModel()
                    {
                        Login = model.Login,
                        Password = model.Password,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                    });
                default:
                    return server1.Register(model);
            }
        }
    }
}
