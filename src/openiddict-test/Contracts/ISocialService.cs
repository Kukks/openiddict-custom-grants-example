namespace openiddict_test.Contracts
{
    public interface ISocialService
    {
        bool VerifyAccessToken(string accessToken, out object response);
    }
}