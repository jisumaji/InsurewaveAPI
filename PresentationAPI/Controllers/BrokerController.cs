using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using DataLayer.Models;
using LogicLayer;
using PresentationAPI.Models;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost]
        public ActionResult<string> AddPolicy(int assetid, PolicyModel p)
        {
            if (ModelState.IsValid)
            {
                PolicyDetail policyDetail = new PolicyDetail()
                {
                    AssetId = assetid,
                    InsurerId = p.InsurerId,
                    BrokerId = p.BrokerId,
                    Duration = p.Duration,
                    Premium = p.Premium,
                    LumpSum = p.LumpSum,
                    StartDate = p.StartDate,
                    PremiumInterval = p.PremiumInterval,
                    MaturityAmount = p.MaturityAmount,
                    PolicyStatus = "pending",
                    ReviewStatus = "no",
                    Feedback = p.Feedback
                };
                Broker r = new();
                r.ChangeReviewStatus(p.AssetId, p.BrokerId);
                _context.Add(policyDetail);
                _context.SaveChanges();
                return "success";
            }
            return "failure";
        }
        private bool PolicyExists(int policyId)
        {
            return _context.PolicyDetails.Any(e => e.PolicyId == policyId);
        }
        [HttpPut]
        public ActionResult<string> EditPolicy(int policyid, PolicyModel p)
        {

            if (!PolicyExists(policyid))
                return "notFound";

            PolicyDetail policyDetail = new PolicyDetail()
            {
                AssetId = p.AssetId,
                InsurerId = p.InsurerId,
                BrokerId = p.BrokerId,
                Duration = p.Duration,
                Premium = p.Premium,
                LumpSum = p.LumpSum,
                StartDate = p.StartDate,
                PremiumInterval = p.PremiumInterval,
                MaturityAmount = p.MaturityAmount,
                PolicyStatus = "pending",
                ReviewStatus = "no",
                Feedback = p.Feedback,
            };
            try
            {
                _context.Update(policyDetail);
                _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return "failure";
            }
            return "success";
        }
        [HttpGet]
        public ActionResult<List<BrokerRequest>> CurrentRequests(string brokerId)
        {
            Request r = new();
            List<BrokerRequest> br = r.GetRequestList(brokerId);
            return br;
        }
        [HttpDelete]
        public ActionResult<string> DeleteRequest(int requestId)
        {
            BrokerRequest br = _context.BrokerRequests.Where(a => a.RequestId == requestId).FirstOrDefault();
            br.ReviewStatus = "yes";
            _context.SaveChanges();
            return "success";
        }
        private bool UserDetailExists(string id)
        {
            return (_context.UserDetails?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}