// Copyright (c) Vlad_Den <vladden500@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace TGBot
{
    /// <summary>
    /// Bot configuration class.
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// Required field! Contains the bot's access token. You can get it from <see href="https://t.me/BotFather">@BotFather</see>.
        /// </summary>
        private static readonly string token = Environment.GetEnvironmentVariable("TG_TOKEN") ?? "token must be here";

        /// <summary>
        /// Read-only parameter.
        /// </summary>
        /// <returns>
        /// The bot's telegram token.
        /// </returns>
        public static string Token { get { return token; } }
    }
}