using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using DataLayer.Models;
using LogicLayer;
namespace PresentationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class BrokerController : ControllerBase
    {
        InsurewaveContext _context;
        IBroker obj;
        IPolicy ip;
        public BrokerController(IBroker _obj, IPolicy ip1)
        {
            _context = new InsurewaveContext();
            obj = _obj;
            ip = ip1;
        }
        [HttpGet("{brokerId}")]
        public ActionResult<List<PolicyDetail>> GetAllPolicies(string brokerId)
        {
             if (!UserDetailExists(brokerId))
             {
                 return null;
             }
            List<PolicyDetail> bd = obj.GetAllPolicies(brokerId);
            return bd;
        }
        /*[HttpPost]
        public ActionResult<PolicyDetail> AddPolicy(PolicyModel p)
        {
            if (ModelState.IsValid)
            {
                PolicyDetail policyDetail = new PolicyDetails()
                {
                    PolicyId=p.PolicyId,
                    AssetId=p.AssetId,
                    InsurerId=p.AssetId,
                    BrokerId=p.BrokerId,
                    Duration=p.Duration,

                    Premium=p.Premium,
        LumpSum=p.LumpSum,
        StartDate=p.StartDate,

        PremiumInterval=p.
        public decimal MaturityAmount { get; set; }
        public string? PolicyStatus { get; set; }
        public string? ReviewStatus { get; set; }
        public string? Feedback { get; set; }
        policyDetail.ReviewStatus = "no";
                    policyDetail.PolicyStatus = "pending";
                }
                Broker r = new();
                r.ChangeReviewStatus(policyDetail.AssetId, policyDetail.BrokerId);
                _context.Add(policyDetail);
                _context.SaveChanges();
                return RedirectToAction(nameof(CurrentRequests));
            }

            return View(policyDetail);
        }*/
        private bool UserDetailExists(string id)
        {
            return (_context.UserDetails?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
