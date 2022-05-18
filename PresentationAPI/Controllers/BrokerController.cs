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
        IUser user;
        IBroker obj;
        IPolicy policy;
        IRequest ir;
        IBuyer buyer;
        public BrokerController(IBroker _obj, IPolicy ip1, IRequest _ir, IUser _iu, IBuyer buyer)
        {
            obj = _obj;
            policy = ip1;
            ir = _ir;
            user = _iu;
            this.buyer = buyer;
        }

        [HttpGet("{brokerId}")]
        public ActionResult<List<PolicyDetail>> GetAllPolicies(string brokerId)
        {
            if (!user.UserDetailExists(brokerId))
            {
                return null;
            }
            List<PolicyDetail> bd = obj.GetAllPolicies(brokerId);
            return bd;
        }

        [HttpPost]
        public ActionResult<string> AddPolicy(int assetid, PolicyModel p)//query id and parameter id different gives error
        {
            if (assetid != p.AssetId)
                return NotFound();
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
                policy.AddPolicy(policyDetail);
                return "success";
            }

            return "notFound";
        }
        
        [HttpPut]
        public ActionResult<string> EditPolicy(int policyid, PolicyModel p)
        {

            if (!policy.PolicyExists(policyid))
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
                policy.EditPolicy(policyDetail);
            }
            catch (DbUpdateConcurrencyException)
            {
                return "notFound";
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
        public ActionResult<string> DeleteRequest(int assetId, string brokerId)//wrong assetId or brokerId gives exception
        {
            if(!buyer.AssetExists(assetId) || !user.UserDetailExists(brokerId))
                return NoContent();
            ir.ChangeStatus(assetId, brokerId);
            return "success";
        }
    }
}