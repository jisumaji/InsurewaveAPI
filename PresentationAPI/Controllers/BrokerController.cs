﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;
using LogicLayer;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace PresentationLayer.Controllers
{
    public class BrokerController : Controller
    {
        InsurewaveContext _context;
        IBroker obj;
        IPolicy ip;
        public BrokerController( IBroker _obj,IPolicy ip1)
        {
            _context = new InsurewaveContext();
            obj = _obj;
            ip = ip1;
        }
        public IActionResult Index()
        {
            /*int login = (int)HttpContext.Session.GetInt32("LoggedIn");
            if (login != 1 && HttpContext.Session.GetString("Role")!="broker")
                return RedirectToAction("Index", "Home");*/
            return View();
        }
        public IActionResult GetDetails()
        {
            return RedirectToAction("Details","UserDetails");
        }
        public IActionResult GetAllPolicies()
        {
            string brokerId = HttpContext.Session.GetString("UserId");
            List<PolicyDetail> bd = obj.GetAllPolicies(brokerId);
            return View(bd);
        }
        public IActionResult AddPolicy(int assetId)
        {
            Request r = new();
            List<BrokerRequest> br = r.GetRequestList(HttpContext.Session.GetString("UserId"));
            ViewData["AssetId"] = new SelectList(br, "AssetId", "AssetId");
            ViewBag.assetId = assetId;
            ViewBag.brokerId = HttpContext.Session.GetString("UserId");
            TempData["AssetId"] = assetId;
            
            //ViewData["BrokerId"] = new SelectList(_context.BrokerDetails, "BrokerId", "BrokerId");
            ViewData["InsurerId"] = new SelectList(_context.InsurerDetails, "InsurerId", "InsurerId");
            TempData.Keep();
            return View();
        }
        /*[HttpPost]
        public IActionResult AddPolicy(PolicyDetail pd)
        {
            TempData.Keep();
            Policy p = new Policy();
            pd.BrokerId = TempData["UserId"] as string;
            pd.ReviewStatus = "no";
            p.AddPolicy(pd);
            return RedirectToAction("GetAllPolicies");
        }*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPolicy([Bind("PolicyId,AssetId,InsurerId,BrokerId,Duration,Premium,LumpSum,StartDate,PremiumInterval,MaturityAmount,PolicyStatus,ReviewStatus,Feedback")] PolicyDetail policyDetail)
        {
            if (ModelState.IsValid)
            {
                policyDetail.AssetId = (int)TempData["AssetId"];
                policyDetail.BrokerId = HttpContext.Session.GetString("UserId");
                policyDetail.ReviewStatus = "no";
                policyDetail.PolicyStatus = "pending";
                Broker r = new();
                r.ChangeReviewStatus(policyDetail.AssetId, policyDetail.BrokerId);

                _context.Add(policyDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CurrentRequests));
            }
            ViewData["AssetId"] = new SelectList(_context.BuyerAssets, "AssetId", "AssetName", policyDetail.AssetId);
            ViewData["BrokerId"] = new SelectList(_context.BrokerDetails, "BrokerId", "BrokerId", policyDetail.BrokerId);
            ViewData["InsurerId"] = new SelectList(_context.InsurerDetails, "InsurerId", "InsurerId", policyDetail.InsurerId);            
            return View(policyDetail);
        }
        public async Task<IActionResult> EditPolicy(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policyDetail = await _context.PolicyDetails.FindAsync(id);
            if (policyDetail == null)
            {
                return NotFound();
            }
            ViewData["AssetId"] = new SelectList(_context.BuyerAssets, "AssetId", "AssetName", policyDetail.AssetId);
            //ViewBag.AssetId = ip.GetAssetName(policyDetail.AssetId);
            //ViewData["AssetId"] = new SelectListItem(text=);
            ViewData["Name"] = ip.GetAssetName((int)id);
            ViewData["BrokerId"] = new SelectList(_context.BrokerDetails, "BrokerId", "BrokerId", policyDetail.BrokerId);
            ViewData["InsurerId"] = new SelectList(_context.InsurerDetails, "InsurerId", "InsurerId", policyDetail.InsurerId);
            return View(policyDetail);
        }

        // POST: PolicyDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPolicy(int id, [Bind("PolicyId,AssetId,InsurerId,BrokerId,Duration,Premium,LumpSum,StartDate,PremiumInterval,MaturityAmount,PolicyStatus,ReviewStatus,Feedback")] PolicyDetail policyDetail)
        {
            
            if (id != policyDetail.PolicyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                policyDetail.PolicyStatus = "pending";
                policyDetail.ReviewStatus = "no";
                try
                {
                    _context.Update(policyDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    Policy p = new();
                    if (!p.PolicyDetailExists(policyDetail))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(GetAllPolicies));
            }
            ViewData["AssetId"] = new SelectList(_context.BuyerAssets, "AssetId", "AssetName", policyDetail.AssetId);
            //ViewData["AssetId"] = ip.GetAssetName(policyDetail.AssetId);
            ViewData["BrokerId"] = new SelectList(_context.BrokerDetails, "BrokerId", "BrokerId", policyDetail.BrokerId);
            ViewData["InsurerId"] = new SelectList(_context.InsurerDetails, "InsurerId", "InsurerId", policyDetail.InsurerId);
            return View(policyDetail);
        }
        public async Task<IActionResult> CurrentRequests()
        {
            string brokerId = HttpContext.Session.GetString("UserId");
            /*Request r = new();
            List<BrokerRequest> br = r.GetRequestList(brokerId);*/
            ViewBag.Message = "Request has been deleted";
            var insurewaveContext = _context.BrokerRequests.Include(b => b.Asset).Include(b => b.Broker).Where(a=>a.BrokerId==brokerId && a.ReviewStatus=="no");
            return View(await insurewaveContext.ToListAsync());
        }
        public IActionResult DeleteRequest(int requestId)
        {
            BrokerRequest br=_context.BrokerRequests.Where(a=>a.RequestId==requestId).FirstOrDefault();
            br.ReviewStatus = "yes";
            _context.SaveChanges();
            return RedirectToAction("CurrentRequests");
        }
    }
}
