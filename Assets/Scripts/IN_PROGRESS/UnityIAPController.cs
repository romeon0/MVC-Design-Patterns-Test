using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGame.IAP
{
    public class PurchaseReward : IPurchaseReward
    {

    }

    public class UnityIAPController : IAPController<PurchaseReward>
    {
        public override List<IAPProduct> GetNotPurchasedProducts()
        {
            throw new NotImplementedException();
        }

        public override string GetPrice(string productId)
        {
            throw new NotImplementedException();
        }

        public override List<IAPProduct> GetPurchasedProducts()
        {
            throw new NotImplementedException();
        }

        public override void Initialize(List<IAPProduct> products, Action<bool> finished)
        {
            throw new NotImplementedException();
        }

        public override bool IsPurchased(string productId)
        {
            throw new NotImplementedException();
        }

        public override void Purchase(string productId, Action<PurchaseResult<PurchaseReward>> onPurchaseFinished)
        {
            throw new NotImplementedException();
        }

        public override void Restore(Action<RestoreResult> onRestored)
        {
            throw new NotImplementedException();
        }
    }
}
