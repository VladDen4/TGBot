// Copyright (c) Vlad_Den <vladden500@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TGBot
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Bot launch...");

            await BotInitialize();
        }

        private static async Task BotInitialize()
        {
            TelegramBotClient botClient = new(Config.Token);

            CancellationTokenSource cts = new();

            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            User me = await botClient.GetMeAsync();

            Console.WriteLine($"Bot @{me.Username} is running.");
            Console.ReadKey();

            cts.Cancel();
        }

        private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message) return;
            if (message.Text is not { } messageText) return;
            if (message.From is not { } user) return;

            Console.WriteLine($"[{DateTime.Now.ToString(new CultureInfo("ru-RU"))}] User ID: {user.Id} | Username: {user.Username} | Text: {messageText}");

            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                new KeyboardButton[] { "Help me" },
                new KeyboardButton[] { "Call me ☎️" },
            })
            {
                ResizeKeyboard = true
            };

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: messageText,
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: cancellationToken
            );
        }

        private static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
