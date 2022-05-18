﻿using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer;
namespace LogicLayer
{
    public interface IBuyer
    {
        public void AddAsset(BuyerAsset buyerasset);
        public List<BuyerAsset> GetAllAssets(string id);
        public List<CurrencyConversion> GetAllCountry();
        public List<string> GetAllCountryNames();
        public List<int> GetAllCountryIds();
        public BuyerAsset GetAssetById(int assetid);
        public void DeleteAsset(int assetid);
        public void EditAsset(BuyerAsset b);
        public void EditAssetRequest(int assetid);
        public void ChangePaymentStatus(int policyid);
        public List<Pay> GetPayments(string userId);
        public bool AssetExists(int assetId);
    }
}
