using System;

namespace Cardio.UI.Exceptions
{
    /// <summary>
    /// Generic exception, thrown when the asset cannot be loaded.
    /// </summary>
    public class AssetLoadingException : Exception
    {
        public String AssetName { get; set; }

        public AssetLoadingException() {}
        public AssetLoadingException(String assetName): this(String.Empty, assetName) {}
        public AssetLoadingException(string message, String assetName) : this(message, assetName, null) {}
        public AssetLoadingException(string message, String assetName, Exception inner) : base(message, inner)
        {
            AssetName = assetName;
        }
    }
}