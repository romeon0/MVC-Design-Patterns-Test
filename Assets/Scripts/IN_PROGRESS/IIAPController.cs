using System;
using System.Collections.Generic;

namespace UnityGame.IAP
{
    public enum ProductType
    {
        Consumable = 1,
        NonConsumable = 2,
        Subscription = 3
    }

    public interface IRestoreResult
    {

    }

    public interface IPurchaseResult
    {

    }

    public interface IIAPProduct
    {

    }

    public interface IIAPController<A,B,C> 
        where A : IPurchaseResult
        where B : IRestoreResult
        where C : IIAPProduct
    {
        public void Initialize(List<C> products, Action<bool> finished);
        public void Purchase(string productId, Action<A> onPurchaseFinished);
        public bool IsPurchased(string productId);
        public List<C> GetPurchasedProducts();
        public List<C> GetNotPurchasedProducts();
        public void Restore(Action<B> onRestoreFinished);
        public string GetPrice(string productId);
        public void Log(object message);
        public void LogError(object message);
    }
}
