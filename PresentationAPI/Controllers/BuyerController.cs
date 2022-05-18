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
    public class BuyerController : ControllerBase
    {
        IBuyer buyer;
        IRequest request;
        IBroker broker;
        IPolicy policy;
        InsurewaveContext db;
        public BuyerController(IBuyer _buyer, IRequest _request, IBroker _broker, IPolicy _policy, InsurewaveContext _db)
        {
            buyer = _buyer;
            request = _request;
            broker = _broker;
            policy = _policy;
            db = _db;
        }
        private bool UserDetailExists(string id)
        {
            return (db.UserDetails?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
        private bool AssetExists(int assetId)
        {
            return db.BuyerAssets.Any(e => e.AssetId == assetId);
        }
        [HttpGet("{userId}")]
        public ActionResult<List<BuyerAsset>> DisplayAssets(string userId)
        {
            List<BuyerAsset> assets = buyer.GetAllAssets(userId);
            return assets;
        }
        [HttpPost]
        public ActionResult<string> AddAssets(AssetModel buyerAsset)
        {
            if (UserDetailExists(buyerAsset.UserId))
            {
                BuyerAsset asset = new BuyerAsset()
                {
                    UserId = buyerAsset.UserId,
                    CountryId = buyerAsset.CountryId,
                    AssetName = buyerAsset.AssetName,
                    PriceUsd = buyerAsset.PriceUsd,
                    Type = buyerAsset.Type,
                    Request = buyerAsset.Request
                };
                buyer.AddAsset(asset);
                return "success";
            }
            else
                return "invalidUserId";
        }
        [HttpDelete("{assetId}")]
        public ActionResult<string> DeleteAsset(int assetId)
        {
            buyer.DeleteAsset(assetId);
            return "deleted";
        }
        [HttpPut]
        public ActionResult<string> EditAsset(AssetModel asset)
        {
            if (!AssetExists(asset.AssetId))
                return "notFound";
            BuyerAsset buyerAsset = new BuyerAsset()
            {
                AssetId = asset.AssetId,
                UserId = asset.UserId,
                CountryId = asset.CountryId,
                AssetName = asset.AssetName,
                PriceUsd = asset.PriceUsd,
                Type = asset.Type,
                Request = asset.Request
            };
            try
            {
                buyer.EditAsset(buyerAsset);
            }
            catch (DbUpdateConcurrencyException)
            {
                return "failure";
            }
            return "success";
        }
        [HttpGet("{assetIdForPolicy}")]
        public ActionResult<List<PolicyDetail>> ViewPolicyForAsset(int assetIdForPolicy)
        {
            List<PolicyDetail> policyDetails = policy.GetAllPoliciesAsset(assetIdForPolicy);
            return policyDetails;
        }
        [HttpGet]
        public ActionResult<List<BrokerDetail>> BrokersAvalable()
        {
            return broker.GetAllBrokers();
        }
        [HttpGet("assetId+brokerId")]
        public ActionResult<string> SendRequest(int assetId, string brokerId)
        {
            BrokerRequest brokerRequest = new BrokerRequest();
            brokerRequest.AssetId = assetId;
            brokerRequest.BrokerId = brokerId;
            brokerRequest.ReviewStatus = "no";
            try
            {
                request.AddRequest(brokerRequest);
                buyer.EditAssetRequest(assetId);
            }
            catch
            {
                return "failure";
            } 
            return "success";
        }
        [HttpGet("id")]
        public ActionResult<List<Pay>> Payments(string userId)
        {
            return buyer.GetPayments(userId);
        }
        [HttpGet("policyid")]
        public ActionResult<PolicyDetail> GetPolicyById(int policyId)
        {
            return policy.GetPolicyById(policyId);
        }
        [HttpGet("policyId")]
        public ActionResult<string> Payment(int policyId)
        {
            try
            {
                buyer.ChangePaymentStatus(policyId);
            }
            catch
            {
                return "failure";
            }
            return "success";
        }
    }
}
