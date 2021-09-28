// #if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// // You must obfuscate your secrets using Window > Unity IAP > Receipt Validation Obfuscator
// // before receipt validation will compile in this sample.
// // #define RECEIPT_VALIDATION
// #endif


// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Purchasing;

// // #if RECEIPT_VALIDATION
// using UnityEngine.Purchasing.Security;
// // #endif
// // Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
// public class Purchaser : MonoBehaviour, IStoreListener
// {
//     public static Purchaser Instance { set; get; }

//     private static IStoreController m_StoreController;          // The Unity Purchasing system.
//     private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

//     // Product identifiers for all products capable of being purchased: 
//     // "convenience" general identifiers for use with Purchasing, and their store-specific identifier 
//     // counterparts for use with and outside of Unity Purchasing. Define store-specific identifiers 
//     // also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)

//     // General product identifiers for the consumable, non-consumable, and subscription products.
//     // Use these handles in the code to reference which product to purchase. Also use these values 
//     // when defining the Product Identifiers on the store. Except, for illustration purposes, the 
//     // kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
//     // specific mapping to Unity Purchasing's AddProduct, below.

//     public const string kProductID_NoAds = "com.firefighterrush3d.remove.ads";

//     // //GEM PACK
//     // public const string kProductID_400gem = "gempack1"; // 400 gem = 2$
//     // public const string kProductID_1k2gem = "gempack2"; // 1200 gem = 5$
//     // public const string kProductID_2k5gem = "gempack3"; // 2500 gem = 5$
//     // public const string kProductID_5k4gem = "gempack4"; // 5400 gem = 20$
//     // public const string kProductID_14kgem = "gempack5"; // 14000 gem = 50$
//     // public const string kProductID_35kgem = "gempack6"; // 35k gem = 100$

//     // //BOX PACK
//     // public const string kProductID_10NBox = "normalbox1"; // 10 normal box = 3$
//     // public const string kProductID_5EBox = "epicbox1"; // 5 normal box = 5$
//     // public const string kProductID_40EBox = "epicbox2"; // 40 normal box = 30$

//     // //STARTER PACK
//     // public const string kProductID_StarterPack1 = "starterpack1";
//     // public const string kProductID_StarterPack2 = "starterpack2";
//     // public const string kProductID_StarterPack3 = "starterpack3";

//     // Apple App Store-specific product identifier for the subscription product.
//     //private static string kProductNameAppleSubscription = "com.unity3d.subscription.new";

//     // Google Play Store-specific product identifier subscription product.
//     //private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";

//     private bool m_IsGooglePlayStoreSelected;

//     // #if RECEIPT_VALIDATION
//     private CrossPlatformValidator validator;
//     // #endif

//     private void Awake()
//     {
//         Instance = this;

//         var module = StandardPurchasingModule.Instance();

//         m_IsGooglePlayStoreSelected =
//             Application.platform == RuntimePlatform.Android && module.appStore == AppStore.GooglePlay;

//         // #if RECEIPT_VALIDATION
//         string appIdentifier;
//         // #if UNITY_5_6_OR_NEWER
//         appIdentifier = Application.identifier;
//         // #else
//         // appIdentifier = Application.bundleIdentifier;
//         // #endif
//         validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), appIdentifier);
//         // #endif
//     }

//     private void Start()
//     {
//         // If we haven't set up the Unity Purchasing reference
//         if (m_StoreController == null)
//         {
//             // Begin to configure our connection to Purchasing
//             InitializePurchasing();
//         }
//     }

//     public void InitializePurchasing()
//     {
//         // If we have already connected to Purchasing ...
//         if (IsInitialized())
//         {
//             // ... we are done here.
//             return;
//         }

//         // Create a builder, first passing in a suite of Unity provided stores.
//         var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

//         // Add a product to sell / restore by way of its identifier, associating the general identifier
//         // with its store-specific identifiers.
//         builder.AddProduct(kProductID_NoAds, ProductType.Consumable);

//         //builder.AddProduct(kProductIDConsumable, ProductType.Consumable); -> buy 5gem // mua nhieu lan

//         // Continue adding the non-consumable product.
//         //builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable); -> examp no ads // mua 1 lan

//         // And finish adding the subscription product. Notice this uses store-specific IDs, illustrating
//         // if the Product ID was configured differently between Apple and Google stores. Also note that
//         // one uses the general kProductIDSubscription handle inside the game - the store-specific IDs 
//         // must only be referenced here. 
//         //builder.AddProduct(kProductIDSubscription, ProductType.Subscription, new IDs(){
//         //        { kProductNameAppleSubscription, AppleAppStore.Name },
//         //        { kProductNameGooglePlaySubscription, GooglePlay.Name },
//         //    });

//         // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
//         // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
//         UnityPurchasing.Initialize(this, builder);
//     }


//     private bool IsInitialized()
//     {
//         // Only say we are initialized if both the Purchasing references are set.
//         return m_StoreController != null && m_StoreExtensionProvider != null;
//     }

//     #region buy

//     public void BuyNoAds()
//     {
//         Helper.DebugLog("Purchaser buy no ads");
//         BuyProductID(kProductID_NoAds);
//     }

//     //------------------------------STARTER PACKS------------------------------------------

//     //public void BuyConsumable()
//     //{
//     //    // Buy the consumable product using its general identifier. Expect a response either 
//     //    // through ProcessPurchase or OnPurchaseFailed asynchronously.
//     //    BuyProductID(kProductIDConsumable);
//     //}


//     //public void BuyNonConsumable()
//     //{
//     //    // Buy the non-consumable product using its general identifier. Expect a response either 
//     //    // through ProcessPurchase or OnPurchaseFailed asynchronously.
//     //    BuyProductID(kProductIDNonConsumable);
//     //}


//     //public void BuySubscription()
//     //{
//     //    // Buy the subscription product using its the general identifier. Expect a response either 
//     //    // through ProcessPurchase or OnPurchaseFailed asynchronously.
//     //    // Notice how we use the general product identifier in spite of this ID being mapped to
//     //    // custom store-specific identifiers above.
//     //    BuyProductID(kProductIDSubscription);
//     //}


//     void BuyProductID(string productId)
//     {
//         // If Purchasing has been initialized ...
//         if (IsInitialized())
//         {
//             // ... look up the Product reference with the general product identifier and the Purchasing 
//             // system's products collection.
//             Product product = m_StoreController.products.WithID(productId);

//             // If the look up found a product for this device's store and that product is ready to be sold ... 
//             if (product != null && product.availableToPurchase)
//             {
//                 Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));

//                 //Debug.Log("tittle : " + product.metadata.localizedTitle);
//                 //Debug.Log("des : " + product.metadata.localizedDescription);
//                 //Debug.Log("price : " + product.metadata.localizedPriceString);

//                 // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
//                 // asynchronously.

//                 m_StoreController.InitiatePurchase(product);
//             }
//             // Otherwise ...
//             else
//             {
//                 // ... report the product look-up failure situation  
//                 Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
//             }
//         }
//         // Otherwise ...
//         else
//         {
//             // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
//             // retrying initiailization.
//             Debug.Log("BuyProductID FAIL. Not initialized.");
//         }
//     }

//     #endregion

//     // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
//     // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
//     public void RestorePurchases()
//     {
//         // If Purchasing has not yet been set up ...
//         if (!IsInitialized())
//         {
//             // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
//             Debug.Log("RestorePurchases FAIL. Not initialized.");
//             return;
//         }

//         // If we are running on an Apple device ... 
//         if (Application.platform == RuntimePlatform.IPhonePlayer ||
//             Application.platform == RuntimePlatform.OSXPlayer)
//         {
//             // ... begin restoring purchases
//             Debug.Log("RestorePurchases started ...");

//             // Fetch the Apple store-specific subsystem.
//             var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
//             // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
//             // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
//             apple.RestoreTransactions((result) =>
//             {
//                 // The first phase of restoration. If no more responses are received on ProcessPurchase then 
//                 // no purchases are available to be restored.
//                 Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
//             });
//         }
//         // Otherwise ...
//         else
//         {
//             // We are not running on an Apple device. No work is necessary to restore purchases.
//             Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
//         }
//     }


//     //  
//     // --- IStoreListener
//     //

//     public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
//     {
//         // Purchasing has succeeded initializing. Collect our Purchasing references.
//         Debug.Log("OnInitialized: PASS");

//         // Overall Purchasing system, configured with products for this application.
//         m_StoreController = controller;
//         // Store specific subsystem, for accessing device-specific store features.
//         m_StoreExtensionProvider = extensions;

//         //price local
//         InitLocalPrice();


//     }

//     public void InitLocalPrice()
//     {
//         // List<string> items = new List<string>();
//         // items.AddMany(GetPrice(kProductID_400gem), GetPrice(kProductID_1k2gem), GetPrice(kProductID_2k5gem), GetPrice(kProductID_5k4gem),
//         // GetPrice(kProductID_14kgem), GetPrice(kProductID_35kgem), GetPrice(kProductID_10NBox), GetPrice(kProductID_5EBox), GetPrice(kProductID_40EBox));

//         // UIManager.Instance.SetLocalPrice(items);

//         // List<string> items2 = new List<string>();
//         // items2.AddMany(GetPrice(kProductID_StarterPack1), GetPrice(kProductID_StarterPack2), GetPrice(kProductID_StarterPack3));
//         // UIManager.Instance.SetLocalPriceForStarterPack(items2);
//     }

//     public string GetPrice(string productID)
//     {
//         if (m_StoreController == null) InitializePurchasing();
//         return m_StoreController.products.WithID(productID).metadata.localizedPriceString;
//     }


//     public void OnInitializeFailed(InitializationFailureReason error)
//     {
//         // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
//         Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
//     }


//     public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
//     {
//         // bool validPurchase = true;
//         // #if RECEIPT_VALIDATION
//         if (m_IsGooglePlayStoreSelected || Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.tvOS)
//         {
//             try
//             {
//                 var result = validator.Validate(args.purchasedProduct.receipt);
//                 Debug.Log("Receipt is valid. Contents:");
//                 foreach (IPurchaseReceipt productReceipt in result)
//                 {
//                     Debug.Log(productReceipt.productID);
//                     Debug.Log(productReceipt.purchaseDate);
//                     Debug.Log(productReceipt.transactionID);

//                     GooglePlayReceipt google = productReceipt as GooglePlayReceipt;
//                     if (null != google)
//                     {
//                         Debug.Log(google.purchaseState);
//                         Debug.Log(google.purchaseToken);
//                     }

//                     AppleInAppPurchaseReceipt apple = productReceipt as AppleInAppPurchaseReceipt;
//                     if (null != apple)
//                     {
//                         Debug.Log(apple.originalTransactionIdentifier);
//                         Debug.Log(apple.subscriptionExpirationDate);
//                         Debug.Log(apple.cancellationDate);
//                         Debug.Log(apple.quantity);
//                     }

//                     // For improved security, consider comparing the signed
//                     // IPurchaseReceipt.productId, IPurchaseReceipt.transactionID, and other data
//                     // embedded in the signed receipt objects to the data which the game is using
//                     // to make this purchase.
//                 }
//             }
//             catch (IAPSecurityException ex)
//             {
//                 // Debug.Log("Invalid receipt, not unlocking content. " + ex);
//                 return PurchaseProcessingResult.Complete;
//                 // validPurchase = false;
//             }
//         }
//         // #endif

//         // A consumable product has been purchased by this user.
//         if (String.Equals(args.purchasedProduct.definition.id, kProductID_NoAds, StringComparison.Ordinal))
//         {
//             // AppFlyerManager.Instance.Purchase(1, args.purchasedProduct.metadata.localizedPrice.ToString(), args.purchasedProduct.metadata.isoCurrencyCode);

//             //Debug.Log( args.purchasedProduct.metadata.localizedPrice + "___" + args.purchasedProduct.metadata.isoCurrencyCode);

//             // DataManager.Instance.gameData.interAds = 0;
//             // DataManager.Instance.SaveGameData();
//             // DataManager.Instance.ClaimGem(400);
//             // UIManager.Instance.SetGem(FormatTextHelper.FormatText(DataManager.Instance.GetGem()));
//             // UIManager.Instance.OpenBoughtPopup(0);
//             // AdsManager.Instance.HideBanner();
//             // // The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
//             // AppFlyer.Instance.Purchase(1, args.purchasedProduct.metadata.localizedPrice.ToString(), args.purchasedProduct.metadata.isoCurrencyCode);
//             // FirebaseManager.Instance.Buy_IAP("gempack1", FirebaseManager.Instance.GetDayPlayGame(), FirebaseManager.Instance.GetLevel(), FirebaseManager.Instance.GetTotalTimePlay());

//             // PlayerPrefs.SetInt(ConfigKeys.noAds, 1);
//             ProfileManager.SetAds(0);
//             // GameManager.Instance.GetPanelInGame().g_NoAds.SetActive(false);
//             PlaySceneManager.Instance.btn_RemoveAds.gameObject.SetActive(false);
//             AdsManager.Instance.DestroyBanner();

//             AnalysticsManager.LogRemoveAds();

//             Helper.DebugLog("Purchase success!!!!!");
//         }

//         //// Or ... a non-consumable product has been purchased by this user.
//         //else if (String.Equals(args.purchasedProduct.definition.id, kProductIDNonConsumable, StringComparison.Ordinal))
//         //{
//         //    Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//         //    // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
//         //}
//         //// Or ... a subscription product has been purchased by this user.
//         //else if (String.Equals(args.purchasedProduct.definition.id, kProductIDSubscription, StringComparison.Ordinal))
//         //{
//         //    Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//         //    // TODO: The subscription item has been successfully purchased, grant this to the player.
//         //}
//         // Or ... an unknown product has been purchased by this user. Fill in additional products here....
//         else
//         {
//             Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
//             return PurchaseProcessingResult.Pending;
//         }

//         // Return a flag indicating whether this product has completely been received, or if the application needs 
//         // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
//         // saving purchased products to the cloud, and when that save is delayed. 


//         return PurchaseProcessingResult.Complete;

//     }

//     //    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
//     //    {
//     //        bool validPurchase = true; // Presume valid for platforms with no R.V.

//     //        // Unity IAP's validation logic is only included on these platforms.
//     //#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
//     //        // Prepare the validator with the secrets we prepared in the Editor
//     //        // obfuscation window.
//     //        //var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
//     //        //    AppleTangle.Data(), Application.identifier);

//     //        //try
//     //        //{
//     //        //    // On Google Play, result has a single product ID.
//     //        //    // On Apple stores, receipts contain multiple products.
//     //        //    var result = validator.Validate(args.purchasedProduct.receipt);
//     //        //    // For informational purposes, we list the receipt(s)
//     //        //    Debug.Log("Receipt is valid. Contents:");
//     //        //    foreach (IPurchaseReceipt productReceipt in result)
//     //        //    {
//     //        //        Debug.Log(productReceipt.productID);
//     //        //        Debug.Log(productReceipt.purchaseDate);
//     //        //        Debug.Log(productReceipt.transactionID);
//     //        //    }
//     //        //}
//     //        //catch (IAPSecurityException)
//     //        //{
//     //        //    Debug.Log("Invalid receipt, not unlocking content");
//     //        //    validPurchase = false;
//     //        //}
//     //#endif
//     //        if (true)
//     //        {
//     //            Debug.Log("Receipt " + args.purchasedProduct.receipt);
//     //            // Unlock the appropriate content here.
//     //            string productID = args.purchasedProduct.definition.id;

//     //            ProfileManager.Instance.OnBuyIAPSuccess(productID);
//     //            GameManager.Instance.m_AppsFlyerManager.TrackPurchaseEvent(args.purchasedProduct.definition.id, args.purchasedProduct.metadata.localizedPrice.ToString(), args.purchasedProduct.metadata.isoCurrencyCode);
//     //            ProfileManager.Instance.LogFBPurchaseEvent((float)args.purchasedProduct.metadata.localizedPrice, args.purchasedProduct.metadata.isoCurrencyCode, args.purchasedProduct.definition.id);
//     //            //TenjinManager.Instance.OnProcessPurchase(args);
//     //        }
//     //        // Return a flag indicating whether this product has completely been received, or if the application needs 
//     //        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
//     //        // saving purchased products to the cloud, and when that save is delayed. 
//     //        EventManager.TriggerEvent("TurnOffLoading");
//     //        return PurchaseProcessingResult.Complete;
//     //    }


//     public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
//     {
//         // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
//         // this reason with the user to guide their troubleshooting actions.
//         Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));

//         // UIManager.Instance.OpenNotice("Purchase Failed!!!");
//     }
// }
