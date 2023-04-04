//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;
////using Newtonsoft.Json.Linq;
////using Newtonsoft.Json;
////using UnityEngine.Purchasing;
////using UnityEngine.Purchasing.Extension;

//namespace Hello
//{
//    // TEMP classes --------------

//    //-------------------


//    /// <summary>
//    /// Controller to purchase products with real money.
//    /// </summary>
//    public class IAPController //: IStoreListener, IStoreCallback
//    {
//        public enum InitializeState
//        {
//            Initializing,
//            Success,
//            Fail
//        }

//        public enum PurchaseState
//        {
//            None,
//            Pending,
//            PurchaseFailed,
//            PurchaseSucces
//        }

//        public class PurchaseResult
//        {
//           // public PurchaseFailureReason? error;
//            public PurchaseState state;
//           // public RewardDataMeta reward;
//            public bool validationPassed;
//            public string validationError;
//            public bool isSlowCard;

//            public PurchaseResult(PurchaseState state, PurchaseFailureReason? error = null)
//            {
//                this.state = state;
//                this.error = error;
//            }
//        }

//        public class IAPProduct
//        {
//            public ProductType type;
//            public string id;
//        }

//        [SerializeField] private bool _useTransactionLog;
//        private InitializeState _state = InitializeState.Initializing;
//        private IStoreController _controller;
//        private IExtensionProvider _extensions;
//        private Dictionary<string, PurchaseResult> _tasks = new Dictionary<string, PurchaseResult>();
//        private UnityBackendComManager _backendCommunicationManager;
//        private ControllerRef<PlayerProfileController> _playerProfileController = new ControllerRef<PlayerProfileController>();
//        private ControllerRef<AnalyticsController> _analyticsController = new ControllerRef<AnalyticsController>();
//        private ControllerRef<UIController> _uiController = new ControllerRef<UIController>();
//        private ControllerRef<StoreController> _storeController = new ControllerRef<StoreController>();
//        private ControllerRef<PrizeWallController> _prizeWallController = new ControllerRef<PrizeWallController>();
//        /// <summary>
//        /// Use this dictionary to change price format. If not specified then the US format is used.
//        /// </summary>
//        private Dictionary<string, string> _priceFormats = new Dictionary<string, string>()
//        {
//            {"fr-CA","{price} {symbol}" },
//            {"fr","{price} {symbol}" },
//        };
//        private bool _purchasesAlreadyRestored = false;


//        public ProductCollection products => _controller.products;
//        public bool useTransactionLog { get => _useTransactionLog; set => _useTransactionLog = value; }
//        public InitializeState State => _state;
//        public string SuperSpyPassId { get; private set; }


//        private const float INIT_MAX_WAIT_TIME = 10f;
//        /// <summary>
//        /// How much seconds to wait until send callback that this is a purchase using Slow Card. 
//        /// </summary>
//        private const int SLOW_CARD_DETECT_TIMEOUT = 8;
//        private const string PURCHASES_ALREADY_RESTORED_KEY = "purchases_already_restored";
//        private const string BACKEND_ERROR_CODE_ALREADY_PURCHASED = "2002";
//        private const string BACKEND_ERROR_CODE_NO_ITEMS_TO_RESTORE = "1004";


//        #region Public & Private methods

//        public void Init()
//        {
            
//        }

//        /// <summary>
//        /// Initialize the controller and Unity Purchasing with given products.
//        /// </summary>
//        /// <param name="forceInitialize">If this is true then we will force the reinit of the IAP</param>
//        /// <returns></returns>
//        public async Task Initialize(List<IAPProduct> products, bool forceInitialize = false)
//        {
////#if !AMAZON_KIDS
////            LogWrapper.Log("[IAPController] Init started.");

////            if (forceInitialize)
////            {
////                _state = InitializeState.Initializing;
////            }

////            if (_state != InitializeState.Initializing)
////            {
////                LogWrapper.LogError("[IAPController] Init failed. Reason: Already initialized!");
////                return;
////            }

////            _backendCommunicationManager = GameManager.Instance.BackendCommunicationManager;

////            StandardPurchasingModule purchasingModule = StandardPurchasingModule.Instance();
////            ConfigurationBuilder configBuilder = ConfigurationBuilder.Instance(purchasingModule);
////            bool haveProducts = PopulateCatalogItems(products, ref configBuilder);

////            TimeSpan timePassed;
////            if (haveProducts)
////            {
////                UnityPurchasing.Initialize(this, configBuilder);
////                LogWrapper.Log("[IAPController] Initialize started.");
////                DateTime startTime = DateTime.Now;
////                while (true)
////                {
////                    timePassed = (DateTime.Now - startTime);

////                    if (_state != InitializeState.Initializing || timePassed.TotalSeconds >= INIT_MAX_WAIT_TIME)
////                    {
////                        break;
////                    }

////                    await Task.Yield();
////                }
////            }

////            if (_state == InitializeState.Initializing)
////            {
////                _state = InitializeState.Fail;
////            }

////            // Set as already restored for all platforms but not on Appstore
////            //      because on all platforms the restore is made automatically. 
////            if (StoreController.GetPlatform() != StorePlatform.APP_STORE)
////            {
////                _purchasesAlreadyRestored = true;
////            }
////            else
////            {
////                _purchasesAlreadyRestored = PlayerPrefs.GetInt(PURCHASES_ALREADY_RESTORED_KEY, 0) == 1;
////            }

////            LogWrapper.Log($"[IAPController] Init finished. State: {_state}; " +
////                    $"TimePassed:{timePassed.TotalSeconds}sec; " +
////                    $"Timeout:{INIT_MAX_WAIT_TIME}sec;");
////#else
////            _state = InitializeState.Fail;
////            LogWrapper.Log("[IAPController] Init finished. State: Not Supported on this platform.");
////#endif
//        }

//        /// <summary>
//        /// Initialize the controller and Unity Purchasing with products received from Store catalogs.
//        /// </summary>
//        /// <param name="forceInitialize">If this is true then we will force the reinit of the IAP</param>
//        public async Task Initialize(bool forceInitialize = false)
//        {
//            List<IAPProduct> products = GetProductListFromStoreCatalogs();
//            await Initialize(products, forceInitialize);
//        }

//        /// <summary>
//        /// Initialize the controller and Unity Purchasing with products received from Store catalogs.
//        /// </summary>
//        /// <param name="onFinished">Called when the initialization done.</param>
//        /// <param name="forceInitialize">If this is true then we will force the reinit of the IAP</param>
//        public async void Initialize(Action onFinished, bool forceInitialize = false)
//        {
//            List<IAPProduct> products = GetProductListFromStoreCatalogs();
//            await Initialize(products, forceInitialize);
//            onFinished?.Invoke();
//        }

//        /// <summary>
//        /// Populate configuration builder with products from <see cref="IAPShopList"/>
//        /// </summary>
//        /// <param name="shopList"></param>
//        /// <param name="configBuilder"></param>
//        /// <returns>If have products.</returns>
//        private bool PopulateCatalogItems(List<IAPProduct> products, ref ConfigurationBuilder configBuilder)
//        {
//            if (products != null)
//            {
//                StringBuilder productsJoined = new StringBuilder();
//                foreach (IAPProduct product in products)
//                {
//                    productsJoined.Append(string.Format("- Product. Id: {0}; Type: {1};", product.id, product.type));
//                    configBuilder.AddProduct(product.id, product.type);
//                }
//                LogWrapper.Log("[IAPController] Products: \n" + productsJoined.ToString());
//            }
//            else
//            {
//                LogWrapper.Log("[IAPController] Products: No products in list.");
//                return false;
//            }

//            return true;
//        }

//        /// <summary>
//        /// Check if product is purchased.
//        /// </summary>
//        /// <param name="productId"></param>
//        /// <returns></returns>
//        public bool IsPurchased(string productId)
//        {
//            if (_state != InitializeState.Success)
//            {
//                LogWrapper.LogError("[IAPController] IsPurchased failed. Reason: Controller is not initialized. State: " + _state);
//                return false;
//            }

//            if (productId == null)
//            {
//                LogWrapper.LogError("[IAPController] IsPurchased failed. Reason: productId is null.");
//                return false;
//            }

//            var products = _controller.products;
//            if (products == null)
//            {
//                LogWrapper.LogError("[IAPController] IsPurchased failed. Reason: products are null.");
//                return false;
//            }

//            Product product = products.WithID(productId);
//            if (product == null)
//            {
//                LogWrapper.LogError("[IAPController] IsPurchased failed. Reason: Product is null.");
//                return false;
//            }

//            bool purchased = product.definition.type != ProductType.Consumable && product.receipt != null;

//            if (purchased)
//                LogWrapper.Log($"[IAPController] IsPurchased success. Product '{productId}' purchased");
//            else
//                LogWrapper.Log($"[IAPController] IsPurchased success. Product '{productId}' NOT purchased");

//            return purchased;
//        }

//        /// <summary>
//        /// Get product price in format "[symbol][price]", eg RON500,$500
//        /// </summary>
//        /// <param name="productId"></param>
//        /// <returns></returns>
//        public string GetFormattedPrice(string productId)
//        {
//            if (_state != InitializeState.Success)
//            {
//                LogWrapper.LogError("[IAPController] GetFormattedPrice failed. Controller is not initialized. State: " + _state);
//                return "Error";
//            }


//            if (products == null)
//            {
//                LogWrapper.LogError("[IAPController] GetFormattedPrice failed. Products list is null.");
//                return "Error";
//            }

//            Product product = products.WithID(productId);

//            if (product == null)
//            {
//                LogWrapper.LogError($"[IAPController] GetFormattedPrice failed. Product with id='{productId}' not found.");
//                return "Error";
//            }

//            CultureInfo culture = GetStoreCulture();

//            if (_priceFormats.TryGetValue(culture.Name, out string format))
//            {
//                format = format.Replace("{symbol}", product.metadata.isoCurrencyCode);
//                format = format.Replace("{price}", product.metadata.localizedPrice.ToString());
//                LogWrapper.Log($"[IAPController] GetFormattedPrice success 1. Price:'{format}'; " +
//                    $"Symbol:{product.metadata.isoCurrencyCode}; " +
//                    $"Price:{product.metadata.localizedPrice}; " +
//                    $"Culture:{culture.Name};");
//                return format;
//            }

//            LogWrapper.Log($"[IAPController] GetFormattedPrice success 2. Price:'{product.metadata.localizedPriceString}'; Culture:{culture.Name};");
//            return product.metadata.localizedPriceString;
//        }

//        /// <summary>
//        /// Restore IAP purchases and call backend to get the reward.
//        /// </summary>
//        /// <param name="forceRestore"> If true then ignore <see cref="IAPController._purchasesAlreadyRestored"/> flag. </param>
//        /// <returns>
//        /// success - if restored or not<br></br>
//        /// alreadyRestored - if purchases already restored somewhen. Valid only on first boot, after app install.<br></br>
//        /// restoredProductsCount - how many products restored. 0 if not restored or nothing to restore. <br></br>
//        /// error - the error if restore failed, null in success .<br></br>
//        /// reward - what reward received from backend<br></br>
//        /// </returns>
//        public async Task<(bool success, bool alreadyRestored, int restoredProductsCount, string error, RewardDataMeta reward)>
//            RestoreIAP(bool forceRestore, List<IAPProduct> allProducts)
//        {
//            // 0 products received. Usually it is happening because no internet connection or an error on backend occured.
//            if (allProducts.Count == 0)
//            {
//                LogWrapper.Log($"[IAPController] Failed to restore purchases. Error: 0 products received.");
//                return (false, false, 0, "0 products received.", null);
//            }

//            if (!forceRestore && _purchasesAlreadyRestored)
//            {
//                return (false, true, 0, "Already restored", null);
//            }

//            //Debug.
//            string allProductsJoined = "";
//            allProducts.ForEach(p => allProductsJoined += string.Format("{0}:{1}\n", p.id, p.type));
//            LogWrapper.Log($"[IAPController] All Products({allProducts.Count}): {allProductsJoined}");

//            await Initialize(allProducts, true);

//            bool restored = await RestorePurchasesOnIOS();

//            if (!restored)
//            {
//                LogWrapper.Log($"[IAPController] Failed to restore purchases, user canceled sign in.");
//                return (false, false, 0, "User Canceled", null);
//            }

//            List<Product> purchasedProducts = GetPurchasedProducts();

//            if (purchasedProducts == null)
//            {
//                LogWrapper.Log($"[IAPController] Failed to restore purchases. Error: 0 purchased products received.");
//                return (false, false, 0, "0 purchased products received.", null);
//            }

//            //Debug.
//            string restoredProductsJoined = "";
//            purchasedProducts.ForEach(p => restoredProductsJoined += string.Format("{0}\n", p.definition.id));
//            LogWrapper.Log($"[IAPController] Products to restore({purchasedProducts.Count}): {restoredProductsJoined}");

//            if (purchasedProducts.Count != 0)
//            {
//                List<string> receipts = new List<string>();
//#if UNITY_ANDROID
//                foreach (var product in purchasedProducts)
//                {
//                    receipts.Add(product.receipt);
//                }
//#else
//                // All purchased products on iOS can be fetched from one receipt, so send only one receipt.
//                receipts.Add(purchasedProducts[0].receipt);
//#endif
//                IAPRestorePostRequest request = new IAPRestorePostRequest();
//                request.receipts = receipts;
//                request.platform = (int)StoreController.GetPlatform();

//                AnalyticsUtils.SetClientAnalyticsHeader(request, _playerProfileController.Value.PlayerProfile.id, _analyticsController.Value.GetStandardParameters(), _uiController.Value.LastScreenName);

//                var backendCommManager = GameManager.Instance.BackendCommunicationManager;
//                IAPRestorePostResponse response = (IAPRestorePostResponse)await backendCommManager.Send(request);

//                if (string.IsNullOrEmpty(response.Error)) // Purchases restored.
//                {
//                    _purchasesAlreadyRestored = true;
//                    PlayerPrefs.SetInt(PURCHASES_ALREADY_RESTORED_KEY, 1);
//                    _playerProfileController.Value.UpdateProfileWithReward(response.rewards);

//                    // Restore Super Pass.
//                    if (IsPurchased(SuperSpyPassId))
//                    {
//                        bool unlocked = await _prizeWallController.Value.UnlockPremium();
//                        LogWrapper.Log($"[IAPController] Super Pass is purchased. Unlocked premium: {unlocked}");
//                    }
//                    else
//                    {
//                        LogWrapper.Log($"[IAPController] Super Pass isn't purchased.");
//                    }

//                    LogWrapper.Log($"[IAPController] Restore finished. Success.");
//                    return (true, false, purchasedProducts.Count, null, response.rewards);
//                }
//                else if (response.ErrorCode == BACKEND_ERROR_CODE_NO_ITEMS_TO_RESTORE) // Nothing to restore.
//                {
//                    _purchasesAlreadyRestored = true;
//                    PlayerPrefs.SetInt(PURCHASES_ALREADY_RESTORED_KEY, 1);
//                    LogWrapper.Log($"[IAPController] Restore finished. Success(nothing to restore).");
//                    return (true, false, 0, "Nothing to restore", null);
//                }
//                else // Purchases not restored.
//                {
//                    LogWrapper.Log($"[IAPController] Restore finished. Failed. Error:{response.Error};");
//                    return (false, false, purchasedProducts.Count, response.Error, null);
//                }
//            }
//            else// No products to restore.
//            {
//                return (false, true, 0, "No products", null);
//            }
//        }


//        /// <summary>
//        /// Get the count of purchased products.
//        /// </summary>
//        public int GetPurchasesCount()
//        {
//            if (_state != InitializeState.Success)
//            {
//                LogWrapper.LogError("[IAPController] GetPurchasesCount failed. Reason: Controller is not initialized. State: " + _state);
//                return 0;
//            }

//            var products = _controller.products;
//            if (products == null)
//            {
//                LogWrapper.LogError("[IAPController] GetPurchasesCount failed. Reason: Products are null.");
//                return 0;
//            }

//            int result = 0;
//            foreach (var elem in _controller.products.all)
//            {
//                if (IsPurchased(elem.definition.id))
//                {
//                    ++result;
//                }
//            }

//            return result;
//        }

//        /// <summary>
//        /// Restores purchases on iOS. 
//        /// Valid only on iOS because on another platforms the items are restored automatically.
//        /// </summary>
//        /// <returns></returns>
//        private async Task<bool> RestorePurchasesOnIOS()
//        {
//#if UNITY_IOS
//            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
//            {
//                if (_state != InitializeState.Success)
//                {
//                    LogWrapper.LogError("[IAPController] Restore purchases failed. Reason: Controller is not initialized. Controller State: " + _state);
//                    return false;
//                }

//                TaskCompletionSource<bool> result = new TaskCompletionSource<bool>();
//                var apple = _extensions.GetExtension<IAppleExtensions>();
//                apple.RestoreTransactions((restored) =>
//                {
//                    LogWrapper.Log("[IAPController] Purchases " + (restored ? "restored." : "NOT restored."));

//                    result.SetResult(restored);
//                });

//                await result.Task;

//                return result.Task.Result;
//            }
//            else
//            {
//                LogWrapper.LogError("[IAPController] Purchases NOT restored. Reason: Not supported on this platform.");
//                return true;
//            }
//#else
//            await Task.Yield();
//            LogWrapper.Log("[IAPController] Purchases restored(fake).");
//            return true;
//#endif
//        }

//        /// <summary>
//        /// Purchase a product.
//        /// </summary>
//        /// <param name="productId">Product ID to purchase.</param>
//        /// <param name="onSlowCard">Callback called when detected that this purchase 
//        ///     made using Slow Card(valid only on Google Play).</param>
//        /// <returns></returns>
//        public async Task<PurchaseResult> Purchase(string productId, Action<PurchaseResult, TaskCompletionSource<bool>> onSlowCard)
//        {
//            if (_state != InitializeState.Success)
//            {
//                LogWrapper.LogError("[IAPController] Purchase failed. Reason: Controller is not initialized. Controller State: " + _state);
//                return new PurchaseResult(PurchaseState.PurchaseFailed, PurchaseFailureReason.Unknown);
//            }

//            if (productId == null)
//            {
//                LogWrapper.LogError("[IAPController] Purchase failed. Reason: productId=null.");
//                return new PurchaseResult(PurchaseState.PurchaseFailed, PurchaseFailureReason.Unknown);
//            }

//            if (_tasks.ContainsKey(productId))
//            {
//                LogWrapper.LogError($"[IAPController] Purchase failed. Product '{productId}' is in pending state.");
//                return new PurchaseResult(PurchaseState.Pending, PurchaseFailureReason.ExistingPurchasePending);
//            }

//            try
//            {
//                if (IsPurchased(productId))
//                {
//                    return new PurchaseResult(PurchaseState.PurchaseFailed, PurchaseFailureReason.DuplicateTransaction);
//                }

//                var product = _controller.products.WithID(productId);

//                if (product != null)
//                {
//                    string itemData = "";
//                    itemData += string.Format("ProductId: {0}\n", productId);
//                    itemData += string.Format("HasReceipt: {0}\n", product.hasReceipt);
//                    itemData += string.Format("Receipt: {0}\n", product.receipt);
//                    itemData += string.Format("TransactionID: {0}\n", product.transactionID);
//                    itemData += string.Format("AvailableToPurchase: {0}\n", product.availableToPurchase);
//                    itemData += "Metadata:\n";
//                    itemData += string.Format("-> localizedPrice:\n", product.metadata.localizedPrice);
//                    itemData += string.Format("-> isoCurrencyCode:\n", product.metadata.isoCurrencyCode);
//                    LogWrapper.Log("[IAPController] Product Data:\n" + itemData);
//                }

//                if (product == null || product.availableToPurchase == false)
//                {
//                    LogWrapper.Log(string.Format("[IAPController] Purchase failed. ProductId: {0}; Reason: {1}",
//                        productId,
//                        PurchaseFailureReason.ProductUnavailable));
//                    return new PurchaseResult(PurchaseState.PurchaseFailed, PurchaseFailureReason.ProductUnavailable);
//                }
//                else
//                {
//                    PurchaseResult purchaseResult = new PurchaseResult(PurchaseState.None);
//                    _tasks.Add(productId, purchaseResult);

//                    _controller.InitiatePurchase(product);

//                    // Here we wait until purchase processed. 
//                    // On android we have slow card option that delays the purchase to few minutes. 
//                    // If this is a purchase using Slow Card then send callback to inform the user.  
//                    TaskCompletionSource<bool> processWaiter = new TaskCompletionSource<bool>();
//                    int secondsWaited = 0;
//                    while (purchaseResult.state == PurchaseState.None || purchaseResult.state == PurchaseState.Pending)
//                    {
//                        await Task.Delay(1000);
//                        // Slow card is valid only on google play.
//                        if (StoreController.GetPlatform() == StorePlatform.GOOGLE_PLAY)
//                        {
//                            ++secondsWaited;
//                            if (secondsWaited == SLOW_CARD_DETECT_TIMEOUT)
//                            {
//                                LogWrapper.Log($"[IAPController] Slow card detected. ProductId: {productId}");
//                                purchaseResult.isSlowCard = true;
//                                onSlowCard?.Invoke(purchaseResult, processWaiter);
//                            }
//                        }

//                    }
//                    processWaiter.SetResult(true);

//                    _tasks.Remove(productId);

//                    if (purchaseResult.state == PurchaseState.PurchaseSucces)
//                    {
//                        SendAnalytics(product);
//                        LogWrapper.Log(string.Format("[IAPController] Purchase succes. ProductId: {0}", productId));
//                    }
//                    else
//                    {
//                        LogWrapper.Log(string.Format("[IAPController] Purchase failed. ProductId: {0}; Reason: {1}",
//                            productId,
//                            purchaseResult.error));
//                    }

//                    return purchaseResult;
//                }
//            }
//            catch (Exception e)
//            {
//                LogWrapper.Log($"[IAPController] Purchase failed for product '{productId}'. Exception: " + e.Message);
//            }

//            return new PurchaseResult(PurchaseState.PurchaseFailed, PurchaseFailureReason.Unknown);
//        }

//        public string GetErrorLocalizationKeyFromResult(PurchaseResult result)
//        {
//            if (result.error.HasValue)
//            {
//                PurchaseFailureReason error = result.error.Value;
//                switch (error)
//                {
//                    case PurchaseFailureReason.DuplicateTransaction: return LocalizationKeys.IAP_ERROR_DUPLICATE_TRANSACTION;
//                    case PurchaseFailureReason.ExistingPurchasePending: return LocalizationKeys.IAP_ERROR_EXISTING_PURCHASE_PENDING;
//                    case PurchaseFailureReason.PaymentDeclined: return LocalizationKeys.IAP_ERROR_PAYMENT_DECLINED;
//                    case PurchaseFailureReason.ProductUnavailable: return LocalizationKeys.IAP_ERROR_PRODUCT_UNAVAILABLE;
//                    case PurchaseFailureReason.PurchasingUnavailable: return LocalizationKeys.IAP_ERROR_PURCHASING_UNAVAILABLE;
//                    case PurchaseFailureReason.UserCancelled: return LocalizationKeys.IAP_ERROR_USER_CANCELED;
//                    case PurchaseFailureReason.Unknown:
//                        if (!result.validationPassed)
//                        {
//                            return LocalizationKeys.IAP_ERROR_VALIDATION_FAILED;
//                        }
//                        else
//                        {
//                            return LocalizationKeys.IAP_ERROR_SOMETHING_WENT_WRONG;
//                        }
//                    default: return LocalizationKeys.IAP_ERROR_SOMETHING_WENT_WRONG;
//                }
//            }

//            return "";
//        }

//        public string GetErrorStringFromResult(PurchaseResult result)
//        {
//            if (result.error.HasValue)
//            {
//                PurchaseFailureReason error = result.error.Value;
//                switch (error)
//                {
//                    case PurchaseFailureReason.DuplicateTransaction: return "Item already bought!";
//                    case PurchaseFailureReason.ExistingPurchasePending: return "This item already is in pending state!";
//                    case PurchaseFailureReason.PaymentDeclined: return "Payment declined. Please contanct your bank.";
//                    case PurchaseFailureReason.ProductUnavailable: return "This product is unavailable.";
//                    case PurchaseFailureReason.PurchasingUnavailable: return "You can't purchase. Please contact our support.";
//                    case PurchaseFailureReason.UserCancelled: return "Item not bought because you canceled the purchase.";
//                    case PurchaseFailureReason.Unknown:
//                        if (!result.validationPassed)
//                        {
//                            string validationError = "Your order not passed validation. We will return your money in few minutes.";
//                            if (!string.IsNullOrEmpty(result.validationError))
//                            {
//                                validationError += string.Format(" Error: {0}", result.validationError);
//                            }
//                            return validationError;
//                        }
//                        else
//                        {
//                            return "Something went wrong. Please try again later.";
//                        }
//                    default: return "Something went wrong. Please try again later.";
//                }
//            }

//            return null;
//        }

//        /// <summary>
//        /// Get store culture, for eg jp-JP, ro-RO, fr-CA.
//        /// </summary>
//        /// <returns></returns>
//        public CultureInfo GetStoreCulture()
//        {
//            if (_state != InitializeState.Success)
//            {
//                LogWrapper.LogError("[IAPController] GetStoreRegion failed. Reason: Controller is not initialized. State: " + _state);
//                return null;
//            }

//            string currencyCode = GetCurrencyCode();

//            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
//            foreach (CultureInfo culture in cultures)
//            {
//                RegionInfo region;
//                try
//                {
//                    region = new RegionInfo(culture.LCID);
//                }
//                catch
//                {
//                    region = null;
//                }

//                if (region != null)
//                {
//                    if (region.ISOCurrencySymbol == currencyCode)
//                    {
//                        LogWrapper.Log($"[IAPController] GetRegion. Currency:{currencyCode}; "
//                            + $"DisplayName:{region.DisplayName};"
//                            + $"EnglishName:{region.EnglishName};"
//                            + $"Name:{region.Name};"
//                            + $"NativeName:{region.NativeName};"
//                            );
//                        return culture;
//                    }
//                }
//            }

//            LogWrapper.Log($"[IAPController] GetRegion. Not found!");
//            return null;
//        }


//        /// <summary>
//        /// Get currency code from Store. (RON/$/MDL/etc)
//        /// </summary>
//        public string GetCurrencyCode()
//        {
//            if (_state != InitializeState.Success)
//            {
//                LogWrapper.LogError("[IAPController] GetCurrencyCode failed. Reason: Controller is not initialized. State: " + _state);
//                return null;
//            }

//            if (_controller == null || _controller.products == null || _controller.products.all == null || _controller.products.all.Length == 0)
//            {
//                LogWrapper.Log("[IAPController] GetCurrencyCode failed.");
//                return null;
//            }

//            Product product = _controller.products.all[0];
//            string currencyCode = product.metadata.isoCurrencyCode;
//            LogWrapper.Log("[IAPController] GetCurrencyCode success. CurrencyCode: " + currencyCode);
//            return currencyCode;
//        }

//        public List<Product> GetPurchasedProducts()
//        {
//            if (_controller == null || _state != InitializeState.Success)
//            {
//                LogWrapper.Log("[IAPController] GetPurchasedProducts fail. Controller not initialized. State: " + _state);
//                return null;
//            }

//            // Add restored products.
//            List<Product> purchasedItems = new List<Product>();
//            foreach (var product in products.all)
//            {
//                if (IsPurchased(product.definition.id))
//                {
//                    purchasedItems.Add(product);
//                }
//            }

//            return purchasedItems;
//        }

//        /// <summary>
//        /// Check if product with given ID exists.
//        /// </summary>
//        public bool HaveProduct(string productId)
//        {
//            if (_controller == null || _state != InitializeState.Success)
//            {
//                LogWrapper.Log("[IAPController] GetPurchasedProducts fail. Controller not initialized. State: " + _state);
//                return false;
//            }

//            if (string.IsNullOrEmpty(productId))
//            {
//                return false;
//            }

//            if (products == null || products.all == null)
//            {
//                return false;
//            }

//            foreach (var product in products.all)
//            {
//                if (product.definition.id == productId)
//                {
//                    return true;
//                }
//            }
//            return false;
//        }

//        /// <summary>
//        /// Get first found subscription.
//        /// </summary>
//        /// <returns></returns>
//        public string GetFirstSubscriptionProductId()
//        {
//            if (_controller == null || _state != InitializeState.Success)
//            {
//                LogWrapper.Log("[IAPController] GetFirstSubscriptionProductId fail. Controller not initialized. State: " + _state);
//                return null;
//            }

//            foreach (var product in products.all)
//            {
//                if (product.definition.type == ProductType.Subscription)
//                {
//                    return product.definition.id;
//                }
//            }

//            return null;
//        }

//        public override void Cleanup()
//        {

//        }

//        public override void DeltaUpdate(float dt)
//        {

//        }

//        public override void FixedDeltaUpdate(float dt, int ticks)
//        {

//        }

//        /// <summary>
//        /// Send request to Backend to validate the purchase. 
//        /// </summary>
//        /// <param name="product"></param>
//        /// <param name="cachedReceipt">Cached receipt. Can be non null if this is pending purchase.</param>
//        private async void ValidateOnBackend(Product product, string cachedReceipt = null)
//        {
//            LogWrapper.Log($"[IAPController] Validation for product '{product.definition.id}' started.");

//            string productId = product.definition.id;
//            string receipt = product.receipt ?? cachedReceipt;

//            StorePostRequest request = new StorePostRequest();
//            request.platform = (int)StoreController.GetPlatform();
//            request.receipt = InjectPrice(product, receipt);
//            request.shop_item_id = null;//Must be null for IAP products.

//            LogWrapper.Log($"[IAPController] ValidateOnBackend InjectedReceipt: {request.receipt}");

//            try
//            {
//                AnalyticsUtils.SetClientAnalyticsHeader(request, _playerProfileController.Value.PlayerProfile.id, _analyticsController.Value.GetStandardParameters(), _uiController.Value.LastScreenName);
//            }
//            catch (Exception ex)
//            {
//                LogWrapper.Log("[IAPController] Analytics failed. Message: " + ex.Message);
//            }

//            StorePostResponse response = (StorePostResponse)await _backendCommunicationManager.Send(request);

//            string rewardsJoined = "";
//            if (response.rewards != null)
//            {
//                rewardsJoined += string.Format("items:{0};\n", string.Join(",", response.rewards.items));
//                rewardsJoined += string.Format("level_up:{0};\n", response.rewards.level_up);
//                rewardsJoined += string.Format("prize_wall_tokens:{0};\n", response.rewards.prize_wall_tokens);
//                rewardsJoined += string.Format("soft_currency:{0};\n", response.rewards.soft_currency);
//                rewardsJoined += string.Format("xp:{0};\n", response.rewards.xp);
//            }

//            LogWrapper.Log(string.Format("[IAPController] Validation for product '{2}' finished. Error: {0}; Rewards:\n{1}",
//                response.Error, rewardsJoined, productId));

//            PurchaseResult purchaseResult;
//            if (_tasks.TryGetValue(productId, out purchaseResult))
//            {
//                if (IsValidationPassed(response))
//                {
//                    LogWrapper.Log($"[IAPController] Purchase state for '{productId}' changed to {PurchaseState.PurchaseSucces}");
//                    purchaseResult.reward = response.rewards;
//                    purchaseResult.validationPassed = true;

//                    if (productId == SuperSpyPassId)
//                    {
//                        await _prizeWallController.Value.UnlockPremium();
//                    }

//                    _controller.ConfirmPendingPurchase(product);

//                    RemoveCachedReceipt(productId);

//                    purchaseResult.state = PurchaseState.PurchaseSucces;
//                }
//                else
//                {
//                    LogWrapper.Log($"[IAPController] Purchase state for '{productId}' changed to {PurchaseState.PurchaseFailed}");
//                    purchaseResult.error = PurchaseFailureReason.Unknown;
//                    purchaseResult.validationPassed = false;
//                    purchaseResult.validationError = response.Error;
//                    purchaseResult.state = PurchaseState.PurchaseFailed;
//                }
//            }
//            else
//            {
//                LogWrapper.LogError("[IAPController] PurchaseResult not found in dictionary!");
//            }
//        }

//        private List<IAPProduct> GetProductListFromStoreCatalogs()
//        {
//            List<IAPProduct> products = new List<IAPProduct>();
//            foreach (ShopCatalogData catalog in _storeController.Value.Catalogs)
//            {
//                foreach (ShopItemData item in catalog.items)
//                {
//                    if (item.ActiveBundleId == null)
//                    {
//                        continue;
//                    }
//                    products.Add(new IAPProduct
//                    {
//                        id = item.bundle_id,
//                        type = GetProductType(item)
//                    });

//                    if (item.discount_bundle_id != null) // add products for discounts also
//                    {
//                        products.Add(new IAPProduct
//                        {
//                            id = item.discount_bundle_id,
//                            type = GetProductType(item)
//                        });
//                    }
//                }
//            }

//            foreach (ShopCatalogData catalog in _storeController.Value.Catalogs)
//            {
//                if (catalog.type == ShopCatalogType.SUPER_PASS)
//                {
//                    SuperSpyPassId = catalog.items[0].bundle_id;
//                    break;
//                }
//            }

//            return products;
//        }

//        private string InjectPrice(Product product, string receipt)
//        {
//            var json = JObject.Parse(receipt);
//            json.Add("price", $"{product.metadata.localizedPrice}");
//            json.Add("price_currency_code", product.metadata.isoCurrencyCode);
//            return json.ToString();
//        }

//        private void SendAnalytics(Product product)
//        {
//            string iapName = product.definition.id;
//            if (iapName == SuperSpyPassId)
//            {
//                iapName = "premium_pass";
//            }
//            Dictionary<string, string> stringParams = new Dictionary<string, string>
//            {
//                { AnalyticsController.ParamName.IAP_NAME, iapName }
//            };

//            _analyticsController.Value.LogEventOnlyToExtra(AnalyticsController.CategoryName.IAPPURCHASE, stringParams, null, true);
//        }

//        /// <summary>
//        /// Check if validation on Backend finished with succes or fail.
//        /// </summary>
//        /// <param name="response"></param>
//        /// <returns></returns>
//        private bool IsValidationPassed(StorePostResponse response)
//        {
//            if (response.ErrorCode == BACKEND_ERROR_CODE_ALREADY_PURCHASED)
//            {
//                return true;
//            }

//            if (!string.IsNullOrEmpty(response.Error))
//            {
//                return false;
//            }

//            RewardDataMeta reward = response.rewards;

//            if (reward == null)
//            {
//                return false;
//            }

//            return true;
//        }

//        /// <summary>
//        /// Get key of cached receipt in PlayerPrefs by product id.
//        /// </summary>
//        /// <param name="productId"></param>
//        /// <returns></returns>
//        private string GetCachedReceiptKey(string productId)
//        {
//            return productId + "_CachedReceipt";
//        }

//        /// <summary>
//        /// Cache the receipt in PlayerPrefs.
//        /// </summary>
//        /// <param name="productId"></param>
//        /// <param name="receipt"></param>
//        private void CacheReceipt(string productId, string receipt)
//        {
//            string key = GetCachedReceiptKey(productId);
//            EncryptedPlayerPrefs.Save(key, receipt);
//        }

//        /// <summary>
//        /// Get cached receipt from PlayerPrefs.
//        /// </summary>
//        /// <param name="productId"></param>
//        /// <returns></returns>
//        private string GetCachedReceipt(string productId)
//        {
//            string key = GetCachedReceiptKey(productId);
//            return EncryptedPlayerPrefs.Load(key);
//        }

//        /// <summary>
//        /// Remove cached in PlayerPrefs product's receipt.
//        /// </summary>
//        /// <param name="productId"></param>
//        private void RemoveCachedReceipt(string productId)
//        {
//            string key = GetCachedReceiptKey(productId);
//            LogWrapper.Log($"[IAPController] RemoveCachedReceipt. Key: {key}; HasKey: {PlayerPrefs.HasKey(key)}; Value: {GetCachedReceipt(productId)}");
//            if (PlayerPrefs.HasKey(key))
//            {
//                PlayerPrefs.DeleteKey(key);
//            }
//        }

//        /// <summary>
//        /// Get all existing IAP products from Backend.
//        /// </summary>
//        /// <returns></returns>
//        public async Task<List<IAPProduct>> GetAllIAPProductsFromBackend()
//        {
//            AllIAPBundleIdsGetRequest request = new AllIAPBundleIdsGetRequest();

//            var backendCommManager = GameManager.Instance.BackendCommunicationManager;
//            AllIAPBundleIdsGetResponse response = (AllIAPBundleIdsGetResponse)await backendCommManager.Send(request);

//            List<IAPProduct> products = new List<IAPProduct>();
//            foreach (IAPProductData data in response.bundle_ids)
//            {
//                products.Add(new IAPProduct
//                {
//                    id = data.bundle_id,
//                    type = GetProductType(data)
//                });
//            }
//            foreach (IAPProductData data in response.discount_bundle_ids)
//            {
//                products.Add(new IAPProduct
//                {
//                    id = data.bundle_id,
//                    type = GetProductType(data)
//                });
//            }
//            return products;
//        }

//        private ProductType GetProductType(ShopItemData data)
//        {
//            if (data.meta.is_subscription)
//                return ProductType.Subscription;
//            if (data.consumable)
//                return ProductType.Consumable;
//            return ProductType.NonConsumable;
//        }

//        private ProductType GetProductType(IAPProductData data)
//        {
//            if (data.meta.is_subscription)
//                return ProductType.Subscription;
//            if (data.consumable)
//                return ProductType.Consumable;
//            return ProductType.NonConsumable;
//        }

//        #endregion


//        #region Callbacks 

//        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
//        {
//            _controller = controller;
//            _extensions = extensions;
//            LogWrapper.Log("[IAPController] OnInitialized.");
//            _state = InitializeState.Success;

//            string productsJoined = "";
//            foreach (var product in controller.products.all)
//            {
//                bool hasReceipt = product.hasReceipt;
//                bool availableToPurchase = product.availableToPurchase;
//                bool bought = IsPurchased(product.definition.id);
//                bool transactionEmpty = string.IsNullOrEmpty(product.transactionID);
//                bool receiptEmpty = string.IsNullOrEmpty(product.receipt);
//                ProductType type = product.definition.type;
//                productsJoined += $"\nId:{product.definition.id}; Bought:{bought}; HasReceipt:{hasReceipt}; Available:{availableToPurchase}; " +
//                    $"TransactionEmpty:{transactionEmpty}; ReceiptEmpty:{receiptEmpty}; Type:{type}";
//            }

//            LogWrapper.Log(string.Format("[IAPController] OnInitialized 2. Products: {0};", productsJoined));

//        }

//        public void OnInitializeFailed(InitializationFailureReason error)
//        {
//            LogWrapper.LogError("[IAPController] Initialize failed. Error: " + error);
//            _state = InitializeState.Fail;
//        }

//        [UnityEngine.Scripting.Preserve]
//        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
//        {
//            Product product = args.purchasedProduct;
//            string productId = args.purchasedProduct.definition.id;

//            LogWrapper.Log(string.Format("[IAPController] ProcessPurchase start. Product: {0}; Receipt: {1}; ",
//                productId, product.receipt, product.transactionID));

//            // Purchase is in dictionary if is fresh request, launched just now.
//            // Purchase isn't in dictionary if this is a restored product OR pending product(so failed validation).
//            PurchaseResult purchaseResult;
//            if (!_tasks.TryGetValue(productId, out purchaseResult))
//            {
//                purchaseResult = new PurchaseResult(PurchaseState.None, null);
//                _tasks[productId] = purchaseResult;
//                LogWrapper.Log("[IAPController] Purchase not found in dictionary. This is pending/restored purchase.");
//            }

//            string cachedReceipt = GetCachedReceipt(productId);

//            // Cache receipt to continue the purchase if app crashed/user exit or some similar cases.
//            CacheReceipt(productId, product.receipt);


//#if AMAZON_KIDS
//            // If transaction Id is null then the item is already bought.
//            if (product.receipt != null)
//            {
//                JObject receipt = JsonConvert.DeserializeObject<JObject>(product.receipt);
//                if (string.IsNullOrEmpty(receipt["TransactionID"].ToString()))
//                {
//                    purchaseResult.state = PurchaseState.PurchaseFailed;
//                    purchaseResult.error = PurchaseFailureReason.DuplicateTransaction;
//                    LogWrapper.Log($"[IAPController] Item with id '{productId}' already bought!");
//                    LogWrapper.Log($"[IAPController] Purchase state for '{productId}' changed to {PurchaseState.PurchaseFailed}");
//                }
//            }
//#endif

//            // Set the purchase in Pending state and wait for validation from Backend.
//            if (purchaseResult.state == PurchaseState.None)
//            {
//                purchaseResult.state = PurchaseState.Pending;
//                LogWrapper.Log($"[IAPController] Purchase state for '{productId}' changed to {PurchaseState.Pending}");
//                ValidateOnBackend(product, cachedReceipt);
//            }

//            // Validation passes. The item bought.
//            if (purchaseResult.state == PurchaseState.PurchaseSucces)
//            {
//                return PurchaseProcessingResult.Complete;
//            }

//            return PurchaseProcessingResult.Pending;
//        }


//        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
//        {
//            LogWrapper.Log(string.Format("[IAPController] Purchase failed. ProductId: {0}; Reason: {1}",
//                product.definition.id, failureReason));

//            PurchaseResult result;
//            if (_tasks.TryGetValue(product.definition.id, out result))
//            {
//                result.error = failureReason;
//                result.state = PurchaseState.PurchaseFailed;
//            }
//        }

//        public void OnPurchaseComplete(Product product)
//        {
//            LogWrapper.LogError(string.Format("[IAPController] OnPurchaseComplete. Product: {0}", product.metadata.localizedTitle));
//            if (product != null)
//            {
//                string itemData = "";
//                itemData += string.Format("HasReceipt: {0}\n", product.hasReceipt);
//                itemData += string.Format("Receipt: {0}\n", product.receipt);
//                itemData += string.Format("TransactionID: {0}\n", product.transactionID);
//                itemData += string.Format("AvailableToPurchase: {0}\n", product.availableToPurchase);
//                itemData += "Metadata:\n";
//                itemData += string.Format("-> localizedPrice:\n", product.metadata.localizedPrice);
//                itemData += string.Format("-> isoCurrencyCode:\n", product.metadata.isoCurrencyCode);
//                LogWrapper.Log("[IAPController] Purchase Completed. Product Data:\n" + itemData);
//            }
//        }

//        public void OnSetupFailed(InitializationFailureReason reason)
//        {
//            LogWrapper.LogError(string.Format("[IAPController] Setup Failed. Reason: {0}", reason));
//        }

//        public void OnProductsRetrieved(List<ProductDescription> products)
//        {
//            LogWrapper.Log(string.Format("[IAPController] OnProductsRetrieved. Products: {0}; Count: {1}", string.Join(",", products), products.Count));
//        }

//        public void OnPurchaseSucceeded(string storeSpecificId, string receipt, string transactionIdentifier)
//        {
//            LogWrapper.Log(string.Format("[IAPController] Purchase succes! StoreSpecificId: {0}; Transaction: {1}; Receipt: {2}",
//                storeSpecificId, transactionIdentifier, receipt));
//        }

//        public void OnPurchasesRetrieved(List<Product> purchasedProducts)
//        {
//            string productsJoined = "";
//            foreach (var product in purchasedProducts)
//            {
//                productsJoined += string.Format("Product. Id: {0}; Transaction: {1};\n", product.definition.id, product.transactionID);
//            }
//            LogWrapper.Log(string.Format("[IAPController] Purchased products retrieved. Products: \n{0}", productsJoined));
//        }

//        public void OnAllPurchasesRetrieved(List<Product> purchasedProducts)
//        {
//            string productsJoined = "";
//            foreach (var product in purchasedProducts)
//            {
//                productsJoined += string.Format("Product. Id: {0}; Transaction: {1};\n", product.definition.id, product.transactionID);
//            }
//            LogWrapper.Log(string.Format("[IAPController] Purchases retrieved. Products: \n{0}", productsJoined));
//        }

//        public void OnPurchaseFailed(PurchaseFailureDescription desc)
//        {
//            LogWrapper.LogError(string.Format("[IAPController] Purchase failed. Message:{0}; ProductId:{1}; Reason:{2};",
//                desc.message, desc.productId, desc.reason));
//        }

//        #endregion
//    }
//}
