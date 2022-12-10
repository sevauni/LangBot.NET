using System.Text;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System;
using System.IO;
using Telegram.Bot.Types.ReplyMarkups;
using System.Threading;
using LanguageLearnBot_.NET;
using Entity_Framework_Test;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Threading;
using LanguageLearnBot_.NET.Contexts;

internal class Program
{

    public static string ImagesURL { get; private set; }  //HTTP Server with Images
    public static int ImageAmount { get; private set; }  // How many Images there are for every word
    public static int WordCount { get; private set; }  //Amount of Words in DB




    private static async Task Main(string[] args)
    {
        Console.WriteLine("Starting LangBot...");

        //start database engine
        ApplicationContext db = new ApplicationContext();
        ApplicationContextWords wordsDb = new ApplicationContextWords();

        var wordsList = wordsDb.WordEnt.ToList();

        Program.WordCount = wordsList.Last().Id;
        Program.ImageAmount = 21;
        Program.ImagesURL = @"http://example.com/imgs/"; // http://example.com/imgs/


        var users = db.UserLanguageLearner.ToList();




        TokenFetcher.OpenTokenFile();
        using CancellationTokenSource cts = new();
        var botClient = new TelegramBotClient(TokenFetcher.Token);


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



        var me = await botClient.GetMeAsync();
        Console.WriteLine($"Start listening for @{me.Username}");



        if (users.Count > 0)
        {
            foreach (var user in users)
            {

                if (user.TheEnd) continue; //If user has recieved all words - then skip him

                var chosenWord = wordsList.Find(x => x.Id == user.WasSent);

                Random rnd = new Random();  // chose one of the image from the folder. There should be at least different 21 images.
                int picRnd = rnd.Next(1,  Program.ImageAmount);

                var exampleString = "Example:"; // If there is null examle - don't add it to finall string

                if (String.IsNullOrEmpty(chosenWord.Example)) exampleString = "";


                Console.WriteLine($"Sent to {user.Id}");
                Message sentMessage = await botClient.SendPhotoAsync(
                chatId: user.Id,
                photo: $"{Program.ImagesURL}{user.WasSent}/{picRnd}.jpg",
                caption: $"<b>Today's word is: </b>{chosenWord.Word}\n\n{chosenWord.Gender}\n{chosenWord.HebrewWord} \n\n<b>{exampleString}</b>  {chosenWord.Example}\n\n{chosenWord.ExampleTranslation}",
                parseMode: ParseMode.Html,
                cancellationToken: cts.Token);
                user.NextWord();
                db.UserLanguageLearner.Attach(user).Property(x => x.WasSent).IsModified = true;
            }
        }




        db.SaveChanges();
        Console.WriteLine("DB Updated");

        while (true) ;


        cts.Cancel();
        Console.WriteLine("Shutdown...");
        Environment.Exit(0);



        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {

            if (update.Message is not { } message)
                return;
            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;
            var TgName = message.Chat.FirstName;


            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            if (!db.UserLanguageLearner.Any(o => o.Id == chatId))
            {
                db.UserLanguageLearner.Add(new UserLanguageLearner { Name = TgName, Id = chatId });
                db.SaveChanges();
                Console.WriteLine($"User {chatId} was succsesfully saved to DB");
            }
            else
            {
                Console.WriteLine($"User  {TgName} found!");
            }

            if (messageText == "/start")
            {
                Message sentMessage = await botClient.SendPhotoAsync(
                chatId: chatId,
                photo: "http://tutix.ru/imgs/welcome.jpg",
                caption: "<b>Welcome Stranger!</b>.\nThis Bot will send you a new word every day and add some random pictures from photostock websites!\n<i>Let's learn some Hebrew together!</i>",
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
            }
            else
            {

                Message sentMessage = await botClient.SendPhotoAsync(
                chatId: chatId,
				photo: "${Program.ImagesURL}gitcounter.png",
                caption: "This bot is OpenSource and you can use it to learn any language you like!\nThis code and more you can find on my github:\n https://github.com/sevauni\n\n<b>If you don't want to recieve wordcards you can just use \"Stop and Block Bot\" in the chat menu.</b> \nIf you like this bot you can support me and star its repo 🥺.",
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);

            }



        }

        Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
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