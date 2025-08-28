using Api.Models;
using Api.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Api.Functions
{
    public class RateRulesFunction
    {
        private readonly IRateRulesService _rateRulesService;
        private readonly ILogger<RateRulesFunction> _logger;

        public RateRulesFunction(IRateRulesService rateRulesService, ILogger<RateRulesFunction> logger)
        {
            _rateRulesService = rateRulesService;
            _logger = logger;
        }

        [Function("GetUserRateRules")]
        public async Task<HttpResponseData> GetUserRateRules(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "raterules/user/{userId:int}")] HttpRequestData req,
            int userId)
        {
            var result = await _rateRulesService.GetUserRateRulesAsync(userId);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);
            return response;
        }

        [Function("GetRateRuleById")]
        public async Task<HttpResponseData> GetRateRuleById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "raterules/{id:int}")] HttpRequestData req,
            int id)
        {
            var rule = await _rateRulesService.GetByIdAsync(id);

            if (rule is null)
            {
                var notFound = req.CreateResponse(HttpStatusCode.NotFound);
                await notFound.WriteStringAsync($"RateRule {id} not found");
                return notFound;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(rule);
            return response;
        }

        [Function("AddRateRule")]
        public async Task<HttpResponseData> AddRateRule(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "raterules")] HttpRequestData req)
        {
            var body = await JsonSerializer.DeserializeAsync<RateRule>(req.Body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (body is null)
            {
                var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequest.WriteStringAsync("Invalid request body");
                return badRequest;
            }

            await _rateRulesService.AddAsync(body);

            var response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteAsJsonAsync(body);
            return response;
        }

        [Function("UpdateRateRule")]
        public async Task<HttpResponseData> UpdateRateRule(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "raterules/{id:int}")] HttpRequestData req,
            int id)
        {
            var body = await JsonSerializer.DeserializeAsync<RateRule>(req.Body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (body is null || body.RateRuleId != id)
            {
                var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequest.WriteStringAsync("Invalid RateRule data");
                return badRequest;
            }

            await _rateRulesService.UpdateAsync(body);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(body);
            return response;
        }

        [Function("DeleteRateRule")]
        public async Task<HttpResponseData> DeleteRateRule(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "raterules/{id:int}")] HttpRequestData req,
            int id)
        {
            await _rateRulesService.DeleteAsync(id);

            var response = req.CreateResponse(HttpStatusCode.NoContent);
            return response;
        }
    }
}
