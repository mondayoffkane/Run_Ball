namespace MondayOFF
{
    public enum IAPStatus
    {
        Success,
        PurchaseFailed,
        ProductDoesNotExist,
        ProductNotRegistered,
        StoreListenerNotInitialized,
    }

    public enum PurchaseProcessStatus
    {
        VALID,
        INVALID_RECEIPT,
        PURCHASE_FAILED
    }
}