using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGame.IAP
{
    public interface IPurchaseReward
    {

    }

    public class IAPProduct : IIAPProduct
    {
        public ProductType type;
        public string id;
    }

    public class PurchaseResult<T> : IPurchaseResult
        where T: IPurchaseReward
    {
        public string error;
        public PurchaseState state;
        public T reward;

        public PurchaseResult(PurchaseState state, string error = null)
        {
            this.state = state;
            this.error = error;
        }
    }

    public enum PurchaseState
    {
        None,
        Pending,
        PurchaseFailed,
        PurchaseSucces
    }

    public class RestoreResult : IRestoreResult
    {
        public RestoreState state;
        public string error;
        public List<string> restoredProducts = new List<string>();

        public RestoreResult(RestoreState state, string error, List<string> restoredProducts)
        {
            this.state = state;
            this.error = error;
            this.restoredProducts = restoredProducts;
        }
    }

    public enum RestoreState
    {
        RestoreFailed,
        RestoreSucces
    }

    public enum IAPControllerStateType
    {
        NotInitialized = 0,
        Initializing,
        Success,
        Fail
    }

    public class IAPControllerState
    {
        public IAPControllerStateType state;
        public string error;

        public IAPControllerState(IAPControllerStateType state, string error)
        {
            this.state = state;
            this.error = error;
        }
    }

    public abstract class IAPController<T> : MonoBehaviour, IIAPController<PurchaseResult<T>, RestoreResult, IAPProduct>
        where T: IPurchaseReward
    {
        protected IAPControllerState _state = new IAPControllerState(IAPControllerStateType.NotInitialized, string.Empty);

        public abstract void Initialize(List<IAPProduct> products, Action<bool> finished);
        
        public abstract bool IsPurchased(string productId);

        public abstract void Purchase(string productId, Action<PurchaseResult<T>> onPurchaseFinished);

        public abstract List<IAPProduct> GetPurchasedProducts();

        public abstract List<IAPProduct> GetNotPurchasedProducts();

        public abstract void Restore(Action<RestoreResult> onRestored);

        public abstract string GetPrice(string productId);

        public virtual void Log(object message)
        {
            Debug.Log(message);
        }

        public virtual void LogError(object message)
        {
            Debug.LogError(message);
        }

        #region Events
        //private void OnInitSuccess()
        //{

        //}
        //private void OnInitFailed()
        //{

        //}
        //private void OnPurchaseSuccess(string productId)
        //{

        //}
        //private void OnPurchaseFailed(string productId)
        //{

        //}

        //private void OnRestoreSuccess()
        //{

        //}

        //private void OnRestoreFailed()
        //{

        //}
        #endregion
    }
}
