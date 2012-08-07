namespace Cardio.Phone.Shared.Twitter
{
    public interface ITwitter
    {
        bool IsInitialized { get; }

        string AuthenticationUri { get; }

        bool AuthenticationRequired { get; }

        bool Initialize();

        bool AuthenticateWith(string verifier);

        bool SendTweet(string tweet);

        void SaveTo(string filename);
    }
}