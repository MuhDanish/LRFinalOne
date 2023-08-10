using LoftyRoomsDAL.DBContext;
using LoftyRoomsModel.Bookings;
using LoftyRoomsModel.Customers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace LoftyRoomsDAL.Setting
{
    public class PayitDAL
    {
        ApplicationDBContext db;
        private readonly IConfiguration configuration;
        private readonly HttpClient _httpClient;
        public PayitDAL(ApplicationDBContext _db, IConfiguration _configuration, HttpClient httpClient)
        {
            configuration = _configuration;
            db = _db;
            _httpClient = httpClient;
        }


        public async Task<TransactionDto> GetBillStatus(string ReferenceNumber, string BankAccountCode)
        {
            try
            {
                TransactionDto transaction = new TransactionDto();
                string apiUrl = "https://apisandbox.payit.pk/api/SandBox/GetBillStatus";
                string apiKey = "Prismaticwi-2392djk@28-dkole3";

                // Create the request body parameters
                var requestBody = new { ReferenceNumber = ReferenceNumber, BankAccountCode = BankAccountCode };

                // Add the API key to the request headers
                _httpClient.DefaultRequestHeaders.Add("X-API-KEY", apiKey);

                // Serialize the request body to JSON
                string requestBodyJson = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);

                // Create the HTTP request content
                var content = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

                // Send the POST request to the API
                var response = await _httpClient.PostAsync(apiUrl, content);

                // Check if the request was successful (status code 200-299)
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();
                    transaction = JsonConvert.DeserializeObject<TransactionDto>(responseContent);
                    return transaction;
                }
                else
                {
                    return transaction;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



    }
}
