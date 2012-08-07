using System;
using System.IO;
using System.Xml.Linq;
using TweetSharp;

namespace Cardio.UI.Twitter
{
    public class TwitterProxy : ITwitter
    {
        public const int TweetMaxLength = 140;

        private const string ConsumerKey = "ebtssBBHyOSzdtJJUS7Q";

        private const string ConsumerSecret = "EiLwlSB2YyeT22DrDCiOWWll95sFZwpCiCaCujvAJn4";

        private TwitterService _service;

        private OAuthAccessToken _accessToken;

        private OAuthRequestToken _requestToken;

        public TwitterProxy()
        {
            TweetTag = "#cardiogame";
        }

        private TwitterProxy(string accessToken, string secretToken) : this()
        {
            _accessToken = new OAuthAccessToken {Token = accessToken, TokenSecret = secretToken};
        }

        public string TweetTag { get; set; }

        public bool IsInitialized { get; private set; }

        public string AuthenticationUri
        {
            get
            {
                return IsInitialized ? _service.GetAuthenticationUrl(_requestToken).ToString() : string.Empty;
            }
        }

        public bool AuthenticationRequired
        {
            get
            {
                return (_accessToken == null || _accessToken.Token.Length < 3);
            }
        }

        public bool Initialize()
        {
            if (IsInitialized)
            {
                return true;
            }

            try
            {
                _service = AuthenticationRequired
                               ? new TwitterService(ConsumerKey, ConsumerSecret)
                               : new TwitterService(ConsumerKey, ConsumerSecret, _accessToken.Token,
                                                    _accessToken.TokenSecret);
                _requestToken = _service.GetRequestToken();
                IsInitialized = true;
                return true;
            }
            catch
            {
                IsInitialized = false;
                return false;
            }
        }

        public bool AuthenticateWith(string verifier)
        {
            try
            {
                _accessToken = _service.GetAccessToken(_requestToken, verifier);
                _service.AuthenticateWith(_accessToken.Token, _accessToken.TokenSecret);
                return true;
            }
            catch
            {
                IsInitialized = false;
                return false;
            }
        }

        public bool SendTweet(string tweet)
        {
            try
            {
                if (IsInitialized && !AuthenticationRequired && CheckInternetConnection())
                {
                    _service.SendTweet(PrepareForTweeting(tweet));
                }
                return true;
            }
            catch
            {
                IsInitialized = false;
                return false;
            }
        }

        public void SaveTo(string filename)
        {
            if (AuthenticationRequired)
            {
                return;
            }

            var encryptor = new Encryptor(ConsumerKey);
            var root = new XElement("twitter-info");
            root.Add(new XElement("token", encryptor.Encrypt(_accessToken.Token)),
                     new XElement("secret-token", encryptor.Encrypt(_accessToken.TokenSecret)),
                     new XElement("tag", TweetTag ?? string.Empty));
            root.Save(filename);
        }

        private string PrepareForTweeting(string sourceText)
        {
            sourceText = sourceText.Replace("\r\n", " ");
            if (sourceText.Length > TweetMaxLength)
            {
                return sourceText.Remove(TweetMaxLength - 1);
            }

            if (sourceText.Length + TweetTag.Length > TweetMaxLength)
            {
                return sourceText;
            }

            return sourceText + TweetTag;
        }

        public static ITwitter From(string filename)
        {
            try
            {
                if (File.Exists(filename))
                {
                    var doc = XDocument.Load(filename);
                    var settings = doc.Element("twitter-info");
                    var encryptor = new Encryptor(ConsumerKey);
                    string token = encryptor.Decrypt(settings.Element("token").Value);
                    string secretToken = encryptor.Decrypt(settings.Element("secret-token").Value);

                    return new TwitterProxy(token, secretToken);
                }
                return new TwitterProxy();
            }
            catch (Exception)
            {
                return new TwitterProxy();
            }
        }

        private bool CheckInternetConnection()
        {
            try
            {
                System.Net.Dns.GetHostEntry("www.twitter.com");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
