﻿using DataLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Models;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers
{
    public class BuyerController : Controller
    {
        IBuyer obj;
        IRequest obj2;
        IBroker obj3;
        IPolicy obj4;
        InsurewaveContext db = new InsurewaveContext();
        public BuyerController(IBuyer _obj, IRequest _obj2, IBroker _obj3, IPolicy _obj4)
        {
            obj = _obj;
            obj2 = _obj2;
            obj3 = _obj3;
            obj4 = _obj4;
        }
        public IActionResult Index()
        {
            //userdetail  =  u;
            //ViewBag.name  =  userdetail.FirstName;
            //TempData.Keep();
            return View();
        }
        public IActionResult DisplayAssets()
        {
            string id1 = HttpContext.Session.GetString("UserId");
            TempData.Keep();
            List<BuyerAsset> result = obj.GetAllAssets(id1);
            return View(result);
        }
        public IActionResult AddAssets()
        {
            ViewData["CountryId"] = new SelectList(db.CurrencyConversions, "CountryId", "CountryName");
            ViewData["Types"] = new List<SelectListItem>
            {
                new SelectListItem { Value = "Engine", Text = "Engine" },
                new SelectListItem { Value = "Container", Text = "Container" },
                new SelectListItem { Value = "Ship", Text = "Ship" },
                new SelectListItem { Value = "Anchor", Text = "Anchor" },
                new SelectListItem { Value = "Crane", Text = "Crane" }
            };
            return View();
        }
        [HttpPost]
        public IActionResult AddAssets(BuyerAsset b)
        {
            string id2 = HttpContext.Session.GetString("UserId");
            BuyerAsset assetinsert = new BuyerAsset
            {
                UserId = id2,
                CountryId = b.CountryId,
                AssetName = b.AssetName,
                PriceUsd = b.PriceUsd,
                Type = b.Type
            };
            obj.AddAsset(assetinsert);
            return RedirectToAction("DisplayAssets");
        }

        public IActionResult DeleteOneAsset(int assetid)
        {
            BuyerAsset p = obj.GetAssetById(assetid);
            return View(p);
        }

        [HttpPost]
        [ActionName("DeleteOneAsset")]
        public IActionResult Delete(int assetid)
        {
            obj.DeleteAsset(assetid);
            return RedirectToAction("DisplayAssets");
        }
        public IActionResult Edit(int assetid)
        {
            BuyerAsset p = obj.GetAssetById(assetid);
            HttpContext.Session.SetString("Request", p.Request);
            ViewData["CountryId"] = new SelectList(db.CurrencyConversions, "CountryId", "CountryName");
            ViewData["Types"] = new List<SelectListItem> 
            { 
                new SelectListItem { Value = "Engine", Text = "Engine" },
                new SelectListItem { Value = "Container", Text = "Container" },
                new SelectListItem { Value = "Ship", Text = "Ship" },
                new SelectListItem { Value = "Anchor", Text = "Anchor" },
                new SelectListItem { Value = "Crane", Text = "Crane" }
            };
            return View(p);
        }
        [HttpPost]
        public IActionResult Edit(BuyerAsset b)
        {
            string userid = HttpContext.Session.GetString("UserId");
            string request = HttpContext.Session.GetString("Request");
            HttpContext.Session.SetInt32("AssetId", b.AssetId);
            BuyerAsset asset = new BuyerAsset
            {
                AssetId = b.AssetId,
                UserId = userid,
                CountryId = b.CountryId,
                AssetName = b.AssetName,
                PriceUsd = b.PriceUsd,
                Type = b.Type,
                Request = request
            };
            obj.EditAsset(asset);
            return RedirectToAction("DisplayAssets");
        }

        public IActionResult ViewPolicyForOneAsset(int assetid)
        {

            List<PolicyDetail> result = obj4.GetAllPoliciesAsset(assetid);
            return View(result);
        }
        public IActionResult RequestToBroker(int assetid)
        {
            HttpContext.Session.SetInt32("AssetId2",assetid);
            List<BrokerDetail> result = obj3.GetAllBrokers();
            return View(result);
        }
        public IActionResult AddRequest1(string brokerid)
        {
           
            BrokerRequest br = new BrokerRequest();
            br.AssetId = (int)HttpContext.Session.GetInt32("AssetId2");
            br.BrokerId = brokerid;
            br.ReviewStatus = "no";
            obj2.AddRequest(br);
            int assetid =(int) HttpContext.Session.GetInt32("AssetId2");
            obj.EditAssetRequest(assetid);
            return RedirectToAction("DisplayAssets");    
        }
        
        public IActionResult Payment()
        {
            var q =(from ba in db.BuyerAssets
                     join pa in db.PolicyDetails on ba.AssetId equals pa.AssetId
                     join id in db.UserDetails on ba.UserId equals id.UserId
                     join payb in db.PaymentBuyers on pa.PolicyId equals payb.PolicyId
                     where ba.UserId  ==  HttpContext.Session.GetString("UserId")
                     orderby id.UserId
                     select new
                     {
                         PolID=payb.PolicyId,
                         Name=ba.AssetName,
                         LS=pa.LumpSum,
                         P=pa.Premium,
                         Status=payb.PaidStatus
                     }).ToList();
            List<Pay> result  =  new List<Pay>();
            foreach(var x in q)
            {
                Pay p  =  new Pay();
                p.PolicyId = x.PolID;
                p.AssetName  =  x.Name;
                p.LumpSum  =  x.LS;
                p.Premium  =  x.P;
                p.PaidStatus  =  x.Status;
                result.Add(p);
            }
            ViewBag.abc =q.Count;
            return View(result);
        }
        public IActionResult Gateway(int policyid,string paidStatus)
        {
            ViewBag.paidStatus = paidStatus;
            PolicyDetail p = obj4.GetPolicyByPolId(policyid); 
            return View(p);
        }
        //[HttpPost]
        //[ActionName("Gateway1")]
        public IActionResult Gateway1(int policyid)
        //public static void Gateway1(int policyid)
        {
            IBuyer obj = new Buyer();
            obj.ChangePaymentStatus(policyid);
            return RedirectToAction("Payment");
        }
    }
}
