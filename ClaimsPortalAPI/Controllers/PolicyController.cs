using ClaimsPortalAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ClaimsPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PolicyController : ControllerBase
    {
        private readonly ClaimsPortalDbContext _dbContext;
        public PolicyController(ClaimsPortalDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPolicies()
        {
            try
            {
                System.IO.File.AppendAllText("C:\\inetpub\\wwwroot\\ClaimsPortalAPI\\logs\\log.txt", "Fetchig Policies");
                var policies = await _dbContext.PolicyHolders
              .Include(p => p.Policies)
              .Include(v => v.Vehicles)
              .ToListAsync();
                if (policies == null)
                {
                    return NotFound();
                }
                return Ok(policies);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\inetpub\\wwwroot\\ClaimsPortalAPI\\logs\\log.txt", ex.Message);
                System.IO.File.AppendAllText("C:\\inetpub\\wwwroot\\ClaimsPortalAPI\\logs\\log.txt", ex.StackTrace);
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("{Id:int}")]
        public async Task<IActionResult> GetPolicyById(int Id)
        {
            var policy = await _dbContext.PolicyHolders
                .Include(p => p.Policies)
                .Include(v => v.Vehicles)
                .FirstOrDefaultAsync(p => p.Id == Id);
            if (policy == null)
            {
                return NotFound();
            }
            return Ok(policy);
        }
        [HttpPost]
        public async Task<IActionResult> AddPolicy(PolicyDto policy)
        {
            if (policy == null)
            {
                return BadRequest("Policy data is null.");
            }

            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // Create and add the PolicyHolder entity
                    var policyHolderEntity = new PolicyHolder()
                    {
                        Address = policy.AddPolicyHolder.Address,
                        City = policy.AddPolicyHolder.City,
                        Email = policy.AddPolicyHolder.Email,
                        FirstName = policy.AddPolicyHolder.FirstName,
                        LastName = policy.AddPolicyHolder.LastName,
                        PhoneNumber = policy.AddPolicyHolder.PhoneNumber,
                        State = policy.AddPolicyHolder.State,
                        Zip = policy.AddPolicyHolder.Zip,
                    };
                    await _dbContext.PolicyHolders.AddAsync(policyHolderEntity);
                    await _dbContext.SaveChangesAsync();

                    // Get the generated PolicyHolderId
                    int policyHolderId = policyHolderEntity.Id;

                    // Create and add the Vehicle entity, using the generated PolicyHolderId
                    var vehicleEntity = new Vehicle()
                    {
                        ChasisNumber = policy.AddVehicle.ChasisNumber,
                        EngineNumber = policy.AddVehicle.EngineNumber,
                        Make = policy.AddVehicle.Make,
                        Model = policy.AddVehicle.Model,
                        PolicyHolderId = policyHolderId,  // Use the correct foreign key
                        Vin = policy.AddVehicle.Vin,
                        Year = policy.AddVehicle.Year
                    };
                    await _dbContext.Vehicles.AddAsync(vehicleEntity);
                    await _dbContext.SaveChangesAsync();

                    // Get the generated VehicleId
                    int vehicleId = vehicleEntity.Id;

                    // Create and add the Policy entity, using the generated PolicyHolderId and VehicleId
                    var policyEntity = new Policy()
                    {
                        CoverageAmount = policy.AddPolicy.CoverageAmount,
                        CoverageEndDate = policy.AddPolicy.CoverageEndDate,
                        CoverageStartDate = policy.AddPolicy.CoverageStartDate,
                        PolicyHolderId = policyHolderId,  // Use the correct foreign key
                        PolicyNumber = policy.AddPolicy.PolicyNumber,
                        PolicyType = policy.AddPolicy.PolicyType,
                        PremiumAmount = policy.AddPolicy.PremiumAmount,
                        VehicleId = vehicleId // Use the correct foreign key
                    };
                    await _dbContext.Policies.AddAsync(policyEntity);
                    await _dbContext.SaveChangesAsync();

                    // Commit the transaction if all operations succeed
                    await transaction.CommitAsync();

                    return Ok(policy);
                }
                catch (Exception ex)
                {
                    // Rollback the transaction if any operation fails
                    await transaction.RollbackAsync();
                    return BadRequest(new { Message = "An error occurred while adding policy data.", Exception = ex.Message });
                }
            }
        }

        [HttpPut]
        [Route("{Id:int}")]
        public async Task<IActionResult> UpdatePolicy(int Id, PolicyDto policy)
        {
            if (policy == null)
            {
                return BadRequest("Policy data is null");
            }
            var policyHolder = await _dbContext.PolicyHolders.FirstOrDefaultAsync(p => p.Id == Id);

            if (policyHolder == null) { return BadRequest("Policy holder not found"); }
            policyHolder.State = policy.AddPolicyHolder.State;
            policyHolder.Address = policy.AddPolicyHolder.Address;
            policyHolder.PhoneNumber = policy.AddPolicyHolder.PhoneNumber;
            policyHolder.LastName = policy.AddPolicyHolder.LastName;
            policyHolder.FirstName = policy.AddPolicyHolder.FirstName;
            _dbContext.PolicyHolders.Update(policyHolder);
            await _dbContext.SaveChangesAsync();
            return Ok(policyHolder);
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeletePolicy(int id)
        {
            var policyHolder = await _dbContext.PolicyHolders.FirstOrDefaultAsync(p => p.Id == id);
            if (policyHolder == null) { return NotFound(); }
            _dbContext.PolicyHolders.Remove(policyHolder);
            await _dbContext.SaveChangesAsync();
            return Ok(policyHolder);
        }
    }
}
