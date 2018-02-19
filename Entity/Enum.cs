
namespace Entity
{
    public enum DALMessage
    {
        Failed,
        Success,
        Exception,
    }

    public enum FileType : int
    {
        File = 0,
        Folder = 1,
    }
    public enum Thumbnails : int
    {
        largeThumbnail = 0,
        smallThumbnail = 1,
    }


    public enum ErrorCode
    {
        Success = 0,
        DataBaseError = 2023,
        AuthenticationFailed = 4040,
        InternalServerError = 501,
        NotImplem­ented = 502,
        BadGateway = 503,
        ServiceUnavai­lable = 504,
        GatewayTimeout = 505,
        HTTPVersionNotSupported = 506,
        VariantAlsoNegotiates2295 = 507,
        Insuff­icientStorageWebDAV = 508,
        LoopDetectedWebDAV = 509,
        BandwidthLimitExceedednostd = 510,
        NotExtended = 511,
        NetworkAuthen­tic­ationRequired = 598,
        NetworkReadTimeoutError = 599,
        FailedToUpdate = 1010,
        FailedToRname = 1111,
        FailedToListFiles = 1101,
        FailedToDelete = 1919,
        FailedToAddFolder = 1920,
        FailedToSerialize = 1921,
        FailedToDeleteFromStorage = 5050,
    }




}
