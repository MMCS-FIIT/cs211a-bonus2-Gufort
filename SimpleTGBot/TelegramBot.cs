﻿using System.Reflection.Metadata.Ecma335;

namespace SimpleTGBot;

using System.ComponentModel.Design;
using Telegram.Bot;
using System.IO;
using System.Text.RegularExpressions;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

public class TelegramBot
{
    private const string BotToken = "7145548269:AAG0PUEp-0ZDS_QSsLPg8KuKUY0ChTPkwdg";  
    public async Task Run()
    {
        var botClient = new TelegramBotClient(BotToken);

        using CancellationTokenSource cts = new CancellationTokenSource();
        ReceiverOptions receiverOptions = new ReceiverOptions()
        {
            AllowedUpdates = new [] { UpdateType.Message }
        };

        botClient.StartReceiving(
            updateHandler: OnMessageReceived,
            pollingErrorHandler: OnErrorOccured,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );
        var me = await botClient.GetMeAsync(cancellationToken: cts.Token);
        Console.WriteLine($"Бот @{me.Username} запущен.\nДля остановки нажмите клавишу Esc...");

        while (Console.ReadKey().Key != ConsoleKey.Escape){}

        cts.Cancel();
    }

    public static async Task OnMessageReceived(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var message = update.Message;
        var chatId = message.Chat.Id;
        var sticker_hello = new InputFileUrl("https://chpic.su/_data/stickers/c/chefvk/chefvk_004.webp?v=1708211404");
        var sticker_ok = new InputFileUrl("https://chpic.su/_data/stickers/c/chefvk/chefvk_009.webp?v=1708211404");
        var sticker_complete = new InputFileUrl("https://chpic.su/_data/stickers/c/chefvk/chefvk_007.webp?v=1708211404");
        var sticker_great = new InputFileUrl("https://chpic.su/_data/stickers/c/chefvk/chefvk_046.webp?v=1708211404");
        var sticker_think = new InputFileUrl("https://chpic.su/_data/stickers/c/chef_vk/chef_vk_023.webp?v=1708182606");

        if (message.Text != null)
        {
            if (message.Text == "/start")
            {
                await botClient.SendTextMessageAsync(chatId, "Приветсвую! Я тот самый знаменитый шеф-повар из Италии. " +
                    "Всмысле вы меня не знаете? Ну и ладно(. Тем не менее, я могу помочь вам с выбором блюда, а также с его готовкой. " +
                    "Какие у вас будут предпочтения?", replyMarkup: GetButtons());
                await botClient.SendStickerAsync(chatId, sticker_hello);
            }
            else
            {
                switch (message.Text)
                {
                    case "Завтрак":
                        await botClient.SendTextMessageAsync(chatId, "Так, значит мы будем готовить завтрак."+
                            " Если ли у вас какие-нибудь предпочтения на этот счет?"+
                            " Постараюсь что-нибудь придумать.",
                            replyMarkup: GetBreakfastButtons());
                        await botClient.SendStickerAsync(chatId, sticker_think);
                        break;
                    case "Блинчики":
                        await botClient.SendTextMessageAsync(chatId,"О, это мы запросто!");
                        await botClient.SendStickerAsync(chatId, sticker_ok);
                        Thread.Sleep(1500);
                        await botClient.SendPhotoAsync(chatId, new InputFileUrl("https://gas-kvas.com/grafic/uploads/posts/2023-10/1696590503_gas-kvas-com-p-kartinki-blinchik-2.jpg"));
                        var result_1 = WriteRecipe(1,2);
                        await botClient.SendTextMessageAsync(chatId,result_1);
                        await botClient.SendTextMessageAsync(chatId, "Скажите, я могу еще чем-то помочь?", replyMarkup: GetEndButtons());
                        break;
                    case "Каша":
                        var result_2 = WriteRecipe(2, 3);
                        await botClient.SendTextMessageAsync(chatId, "Конечно, un momento, дорогой друг!");
                        await botClient.SendStickerAsync(chatId, sticker_ok); 
                        Thread.Sleep(1500);
                        await botClient.SendPhotoAsync(chatId, new InputFileUrl("https://img1.russianfood.com/dycontent/images_upl/478/big_477420.jpg"));
                        await botClient.SendTextMessageAsync(chatId, result_2);
                        await botClient.SendTextMessageAsync(chatId, "Скажите, я могу еще чем-то помочь?", replyMarkup: GetEndButtons());
                        break;
                    case "Что-нибудь из яиц":
                        var result_3 = WriteRecipe(3,4);
                        await botClient.SendTextMessageAsync(chatId, "Сию минуту, caro amico!");
                        await botClient.SendStickerAsync(chatId, sticker_ok);
                        Thread.Sleep(1500);
                        await botClient.SendPhotoAsync(chatId, new InputFileUrl("https://static.1000.menu/res/640/img/content-v2/60/4e/28288/omlet-na-kefire-na-skovorode_1657259528_21_max.jpg"));
                        await botClient.SendTextMessageAsync(chatId, result_3);
                        await botClient.SendTextMessageAsync(chatId, "Скажите, я могу еще чем-то помочь?", replyMarkup: GetEndButtons());
                        break;
                    case "Быстрый завтрак":
                        var result_4 = WriteRecipe(4,5);
                        await botClient.SendTextMessageAsync(chatId, "Сейчас придумаем что-нибудь, amico!");
                        await botClient.SendStickerAsync(chatId, sticker_ok);
                        Thread.Sleep(1500);
                        await botClient.SendPhotoAsync(chatId, new InputFileUrl("https://static.1000.menu/img/content-v2/7d/f0/66724/tvorog-s-yaicom-zavtrak-pp-za-5-minut_1659036125_9_max.jpg"));
                        await botClient.SendTextMessageAsync (chatId, result_4);
                        await botClient.SendTextMessageAsync(chatId, "Скажите, я могу еще чем-то помочь?", replyMarkup: GetEndButtons());
                        break;
                    case "Обед":
                        await botClient.SendTextMessageAsync(chatId, "Хм, займемся приготовлением обеденных блюд." +
                            " Возможно, у вас есть какие-либо пожелания на этот счет?" +
                            " Буду стараться придумать что-нибудь interessante.",
                            replyMarkup: GetDinnerButtons());
                        await botClient.SendStickerAsync(chatId, sticker_think);
                        break;
                    case "Суп":
                        await botClient.SendTextMessageAsync(chatId, "Легко! Un momento, дорогой друг!");
                        await botClient.SendStickerAsync(chatId, sticker_ok);
                        Thread.Sleep(1500);
                        await botClient.SendPhotoAsync(chatId, new InputFileUrl("https://static.1000.menu/res/640/img/content-v2/41/60/32006/borsch-so-svejei-kapustoi-svekloi-i-myasom_1630127663_4_max.jpg"));
                        var result_5 = WriteRecipe(5, 6);
                        await botClient.SendTextMessageAsync(chatId, result_5);
                        await botClient.SendTextMessageAsync(chatId, "Скажите, я могу еще чем-то помочь?", replyMarkup: GetEndButtons());
                        break;
                    case "Из курицы":
                        await botClient.SendTextMessageAsync(chatId, "Nessun problema, сейчас сделаем!");
                        await botClient.SendStickerAsync(chatId, sticker_ok);
                        Thread.Sleep(1500);
                        var result_6 = WriteRecipe(6, 7);
                        await botClient.SendPhotoAsync(chatId, new InputFileUrl("https://static.1000.menu/res/640/img/content-v2/dd/22/19030/lapsha-vok-s-kuricei-i-ovoshchami_1613557344_14_max.jpg"));
                        await botClient.SendTextMessageAsync(chatId, result_6);
                        await botClient.SendTextMessageAsync(chatId, "Скажите, я могу еще чем-то помочь?", replyMarkup: GetEndButtons());
                        break;
                    case "Что-то попроще":
                        await botClient.SendTextMessageAsync(chatId, "Ну попроще так попроще, будет сделано!");
                        await botClient.SendStickerAsync(chatId, sticker_ok);
                        Thread.Sleep(1500);
                        await botClient.SendPhotoAsync(chatId, new InputFileUrl("https://static.1000.menu/res/640/img/content/28024/jarenye-draniki-na-skovorode_1532114706_9_max.jpg"));
                        var result_7 = WriteRecipe(7, 8);
                        await botClient.SendTextMessageAsync(chatId, result_7);
                        await botClient.SendTextMessageAsync(chatId, "Скажите, я могу еще чем-то помочь?", replyMarkup: GetEndButtons());
                        break;
                    case "Из крупы":
                        await botClient.SendTextMessageAsync(chatId, "Это я мигом, mio amico!");
                        await botClient.SendStickerAsync(chatId, sticker_ok);
                        Thread.Sleep(1500);
                        await botClient.SendPhotoAsync(chatId, new InputFileUrl("https://static.1000.menu/res/640/img/content-v2/22/45/39422/ris-na-skovorode_1613550003_8_max.jpg"));
                        var result_8 = WriteRecipe(8, 9);
                        await botClient.SendTextMessageAsync(chatId, result_8);
                        await botClient.SendTextMessageAsync(chatId, "Скажите, я могу еще чем-то помочь?", replyMarkup: GetEndButtons());
                        break;
                    case "Ужин":
                        await botClient.SendTextMessageAsync(chatId, "Отличная идея! По такому поводу у месть несколько интересных блюд. "+
                            "Будут ли у вас какие-то предпочтения на этот счет, amico?", replyMarkup: GetEveningMealButtons());
                        await botClient.SendStickerAsync(chatId, sticker_great);
                        break;
                    case "Мясное":
                        await botClient.SendTextMessageAsync(chatId, "Meraviglioso! Это мы можем.");
                        await botClient.SendStickerAsync(chatId, sticker_ok);
                        Thread.Sleep(1500);
                        var result_9 = WriteRecipe(9, 10);
                        await botClient.SendPhotoAsync(chatId, new InputFileUrl("https://static.1000.menu/res/640/img/content-v2/e7/82/7979/sochnye-otbivnye-iz-svininy-nejnye-myagkie_1603951843_19_max.jpg"));
                        await botClient.SendTextMessageAsync(chatId, result_9);
                        await botClient.SendTextMessageAsync(chatId, "Скажите, я могу еще чем-то помочь?", replyMarkup: GetEndButtons());
                        break;
                    case "С грибами":
                        await botClient.SendTextMessageAsync(chatId, "Запросто, un momento!");
                        await botClient.SendStickerAsync(chatId, sticker_ok);
                        Thread.Sleep(1500);
                        var result_10 = WriteRecipe(10, 11);
                        await botClient.SendPhotoAsync(chatId, new InputFileUrl("https://static.1000.menu/img/content-v2/91/40/71965/farsh-s-gribami-na-skovorode_1675971327_0_max.jpg"));
                        await botClient.SendTextMessageAsync(chatId, result_10);
                        await botClient.SendTextMessageAsync(chatId, "Скажите, я могу еще чем-то помочь?", replyMarkup: GetEndButtons());
                        break;
                    case "Легкое":
                        await botClient.SendTextMessageAsync(chatId, "Сейчас сделаем!");
                        await botClient.SendStickerAsync(chatId, sticker_ok);
                        Thread.Sleep(1500);
                        var result_11 = WriteRecipe(11, 12);
                        await botClient.SendPhotoAsync(chatId, new InputFileUrl("https://static.1000.menu/res/640/img/content-v2/67/16/3363/baklajany-tushenye-s-ovoshchami-bystro-i-prosto_1602740303_17_max.jpg"));
                        await botClient.SendTextMessageAsync(chatId, result_11);
                        await botClient.SendTextMessageAsync(chatId, "Скажите, я могу еще чем-то помочь?", replyMarkup: GetEndButtons());
                        break;
                    case "На твое усмотрение":
                        await botClient.SendTextMessageAsync(chatId, "Oh, mio Dio! Сделаю в лучшем виде!");
                        await botClient.SendStickerAsync(chatId, sticker_great);
                        Thread.Sleep(1500);
                        var result_12 = WriteRecipe(12, 13);
                        await botClient.SendPhotoAsync(chatId, new InputFileUrl("https://static.1000.menu/res/640/img/content-v2/f7/3c/18410/pasta-s-moreproduktami-v-slivochnom-souse_1614501210_19_max.jpg"));
                        await botClient.SendTextMessageAsync(chatId, result_12);
                        await botClient.SendTextMessageAsync(chatId, "Скажите, я могу еще чем-то помочь?", replyMarkup: GetEndButtons());
                        break;
                    case "Праздничные блюда":
                        await botClient.SendTextMessageAsync(chatId, "Прекрасно! Займемся приготовлением праздничных блюд."+
                            " К какому празднику мы готовимся?", replyMarkup: GetCelebrationButtons());
                        await botClient.SendStickerAsync(chatId, sticker_think);
                        break;
                    case "Новый год":
                        await botClient.SendTextMessageAsync(chatId, "Намечается веселье, надо подготовиться!");
                        await botClient.SendStickerAsync(chatId, sticker_great);
                        Thread.Sleep(1500);
                        var result_13 = WriteRecipe(13, 14);
                        await botClient.SendPhotoAsync(chatId, new InputFileUrl("https://static.1000.menu/res/640/img/content-v2/be/c8/40643/krabovyi-salat-klassicheskii-bez-risa_1611409506_9_max.jpg"));
                        await botClient.SendTextMessageAsync(chatId, result_13);
                        await botClient.SendTextMessageAsync(chatId, "Скажите, я могу еще чем-то помочь?", replyMarkup: GetEndButtons());
                        break;
                    case "Пасха":
                        await botClient.SendTextMessageAsync(chatId, "Отлично, mio amico! Сейчас разберемся.");
                        await botClient.SendStickerAsync(chatId, sticker_ok);
                        Thread.Sleep(1500);
                        var result_14 = WriteRecipe(14, 15);
                        await botClient.SendPhotoAsync(chatId, new InputFileUrl("https://static.1000.menu/res/640/img/content-v2/41/29/46897/kulich-prostoi-legkii_1589530927_22_max.jpg"));
                        await botClient.SendTextMessageAsync(chatId, result_14);
                        await botClient.SendTextMessageAsync(chatId, "Скажите, я могу еще чем-то помочь?", replyMarkup: GetEndButtons());
                        break;
                    case "Майские":
                        await botClient.SendTextMessageAsync(chatId, "У меня даже есть идея!");
                        await botClient.SendStickerAsync(chatId, sticker_great);
                        Thread.Sleep(1500);
                        var result_15 = WriteRecipe(15, 16);
                        await botClient.SendPhotoAsync(chatId, new InputFileUrl("https://static.1000.menu/res/640/img/content-v2/44/9c/34176/shashlyk-iz-svininy-na-uglyax_1625648787_0_max.jpg"));
                        await botClient.SendTextMessageAsync(chatId, result_15);
                        await botClient.SendTextMessageAsync(chatId, "Скажите, я могу еще чем-то помочь?", replyMarkup: GetEndButtons());
                        break;
                    case "Просто семейный":
                        await botClient.SendTextMessageAsync(chatId, "Хорошее идея, un momento!");
                        await botClient.SendStickerAsync(chatId, sticker_ok);
                        Thread.Sleep(1500);
                        var result_16 = WriteRecipe(16, 17);
                        await botClient.SendPhotoAsync(chatId, new InputFileUrl("https://static.1000.menu/img/content-v2/ad/9f/6917/kuritsa-na-soli_1610949660_9_max.jpg"));
                        await botClient.SendTextMessageAsync(chatId, result_16);
                        await botClient.SendTextMessageAsync(chatId, "Скажите, я могу еще чем-то помочь?", replyMarkup: GetEndButtons());
                        break;
                    case "Назад в каталог":
                        await botClient.SendTextMessageAsync(chatId, "С чем еще вам помочь, mio amico?", replyMarkup: GetButtons());
                        await botClient.SendStickerAsync(chatId, sticker_ok);
                        break;
                    case "Нет, спасибо":
                        await botClient.SendTextMessageAsync(chatId, "В таком случае было приятно иметь с вами дело! Ci vediamo, мой друг!");
                        await botClient.SendStickerAsync(chatId, sticker_complete);
                        break;
                    default:
                        await botClient.SendTextMessageAsync(chatId, "Неизвестная команда. "+
                            "Попробуйте ввести /start, чтобы начать работу бота");
                        break;
                }
            }
        }

        using (StreamWriter sr = new StreamWriter("Files\\logs.txt", true))
        {
            sr.WriteLine($"Получено сообщение в чате {chatId}: '{message.Text}'");
        }
    }

    public static string WriteRecipe(int i, int j, string path = "Files\\recipe.txt")
    {
        string text = System.IO.File.ReadAllText(path);
        if (!text.Contains($"[{i}]") || !text.Contains($"[{j}]")) return "";
        int startIndex = text.IndexOf($"[{i}]") + $"[{i}]".Length;
        int endIndex = text.IndexOf($"[{j}]");
        string recipe = text.Substring(startIndex, endIndex - startIndex).Trim();

        return recipe;
    }

    private static IReplyMarkup? GetEndButtons()
    {
        var keydoard_start = new ReplyKeyboardMarkup(new KeyboardButton("Старт"));
        var keyboard = new ReplyKeyboardMarkup(new List<List<KeyboardButton>>
        {
            new List<KeyboardButton> {new KeyboardButton("Назад в каталог"), new KeyboardButton("Нет, спасибо")},
        });

        return keyboard;
    }

    private static IReplyMarkup? GetCelebrationButtons()
    {
        var keydoard_start = new ReplyKeyboardMarkup(new KeyboardButton("Старт"));
        var keyboard = new ReplyKeyboardMarkup(new List<List<KeyboardButton>>
        {
            new List<KeyboardButton> {new KeyboardButton("Новый год"), new KeyboardButton("Пасха")},
            new List<KeyboardButton> {new KeyboardButton("Майские"), new KeyboardButton("Просто семейный")}
        });

        return keyboard;
    }

    private static IReplyMarkup? GetEveningMealButtons()
    {
        var keydoard_start = new ReplyKeyboardMarkup(new KeyboardButton("Старт"));
        var keyboard = new ReplyKeyboardMarkup(new List<List<KeyboardButton>>
        {
            new List<KeyboardButton> {new KeyboardButton("Мясное"), new KeyboardButton("С грибами")},
            new List<KeyboardButton> {new KeyboardButton("Легкое"), new KeyboardButton("На твое усмотрение")}
        });

        return keyboard;
    }

    private static IReplyMarkup? GetDinnerButtons()
    {
        var keydoard_start = new ReplyKeyboardMarkup(new KeyboardButton("Старт"));
        var keyboard = new ReplyKeyboardMarkup(new List<List<KeyboardButton>>
        {
            new List<KeyboardButton> {new KeyboardButton("Суп"), new KeyboardButton("Из курицы")},
            new List<KeyboardButton> {new KeyboardButton("Что-то попроще"), new KeyboardButton("Из крупы")}
        });

        return keyboard;
    }

    private static IReplyMarkup? GetBreakfastButtons()
    {
        var keydoard_start = new ReplyKeyboardMarkup(new KeyboardButton("Старт"));
        var keyboard = new ReplyKeyboardMarkup(new List<List<KeyboardButton>>
        {
            new List<KeyboardButton> {new KeyboardButton("Каша"), new KeyboardButton("Что-нибудь из яиц")},
            new List<KeyboardButton> {new KeyboardButton("Блинчики"), new KeyboardButton("Быстрый завтрак")}
        });

        return keyboard;
    }

    private static IReplyMarkup GetButtons()
    {
        var keydoard_start = new ReplyKeyboardMarkup(new KeyboardButton("Старт"));
        var keyboard = new ReplyKeyboardMarkup(new List<List<KeyboardButton>>
        {
            new List<KeyboardButton> {new KeyboardButton("Завтрак"), new KeyboardButton("Обед")},
            new List<KeyboardButton> {new KeyboardButton("Ужин"), new KeyboardButton("Праздничные блюда")}
        });

        return keyboard;
    }

    Task OnErrorOccured(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            
            _ => exception.ToString()
        };

        using(StreamWriter sr = new StreamWriter("Files\\logs.txt", true))
        {
            sr.WriteLine(errorMessage+"\n");
        }
        return Task.CompletedTask;
    }
}