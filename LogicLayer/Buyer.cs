﻿using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class Buyer:IBuyer
    {
        InsurewaveContext db;
        public Buyer()
        {
            db = new InsurewaveContext();
        }
        public void AddAsset(BuyerAsset buyerasset)
        {
            db.BuyerAssets.Add(buyerasset);
            db.SaveChanges();
        }
        public bool AssetExists(int assetId)
        {
            return db.BuyerAssets.Any(e => e.AssetId == assetId);
        }
        public List<BuyerAsset> GetAllAssets(string id)
        {
            List<BuyerAsset> asset = db.BuyerAssets.Where(a => a.UserId == id).ToList();
            //List<BuyerAsset> asset = db.BuyerAssets.Where(t => t.AssetId==id);
            return asset;
        }
        public List<CurrencyConversion> GetAllCountry()
        {
            List<CurrencyConversion> country = db.CurrencyConversions.ToList();
            return country;
        }
        public List<string> GetAllCountryNames()
        {
            List<string> countryNames  =  db.CurrencyConversions.Select(a  =>  a.CountryName).ToList();
            return countryNames;
        }
        public List<int> GetAllCountryIds()
        {
            List<int> countryIds = db.CurrencyConversions.Select(a => a.CountryId).ToList();
            return countryIds;
        }

        public BuyerAsset GetAssetById(int assetid)
        {
            BuyerAsset auth = db.BuyerAssets.Where(a => a.AssetId == assetid).FirstOrDefault();
            return auth;
        }
        public void DeleteAsset(int assetid)
        {
            BuyerAsset b_asset = db.BuyerAssets.Where(a => a.AssetId == assetid).FirstOrDefault();
            db.BuyerAssets.Remove(b_asset);
            db.SaveChanges();
        }
        public void EditAsset(BuyerAsset b)
        {
            db.BuyerAssets.Update(b);
            db.SaveChanges();
        }
       public void EditAssetRequest(int assetid)
         {
            BuyerAsset b_asset = db.BuyerAssets.Where(a => a.AssetId == assetid).FirstOrDefault();
            b_asset.Request = "yes";
            db.BuyerAssets.Update(b_asset);
            db.SaveChanges();
        }
        /*public void GetAssets(string buyerId)
        {
            List<BuyerAsset> b  =  GetAllAssets(buyerId);
            return var;
        }*/
        public void ChangePaymentStatus(int policyid)
        {
            PaymentBuyer b_pay = db.PaymentBuyers.Where(a => a.PolicyId== policyid).FirstOrDefault();
            b_pay.PaidStatus= "true";
            db.PaymentBuyers.Update(b_pay);
            db.SaveChanges();
        }
        public List<Pay> GetPayments(string userId)
        {
            var q = (from ba in db.BuyerAssets
                     join pa in db.PolicyDetails on ba.AssetId equals pa.AssetId
                     join id in db.UserDetails on ba.UserId equals id.UserId
                     join payb in db.PaymentBuyers on pa.PolicyId equals payb.PolicyId
                     where ba.UserId == userId
                     orderby id.UserId
                     select new
                     {
                         PolID = payb.PolicyId,
                         Name = ba.AssetName,
                         LS = pa.LumpSum,
                         P = pa.Premium,
                         Status = payb.PaidStatus
                     }).ToList();
            List<Pay> result = new List<Pay>();
            foreach (var x in q)
            {
                Pay p = new Pay()
                {
                    PolicyId = x.PolID,
                    AssetName = x.Name,
                    LumpSum = x.LS,
                    Premium = x.P,
                    PaidStatus = x.Status
                };
                result.Add(p);
            }
            return result;
        }
    }
}
