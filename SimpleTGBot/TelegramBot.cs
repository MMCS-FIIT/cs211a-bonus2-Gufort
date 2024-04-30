using System.Reflection.Metadata.Ecma335;

namespace SimpleTGBot;

using System.ComponentModel.Design;
using Telegram.Bot;
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

    /// <summary>
    /// Обработчик события получения сообщения.
    /// </summary>
    /// <param name="botClient">Клиент, который получил сообщение</param>
    /// <param name="update">Событие, произошедшее в чате. Новое сообщение, голос в опросе, исключение из чата и т. д.</param>
    /// <param name="cancellationToken">Служебный токен для работы с многопоточностью</param>
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
                            " Если ли у вас какие-нибудь предпочтения на этот счет?."+
                            " Постараюсь что-нибудь придумать.",
                            replyMarkup: GetBreakfastButtons());
                        await botClient.SendStickerAsync(chatId, sticker_think);
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

    // Обработчик событий нажатия на кнопки
    /// <summary>
    /// Обработчик исключений, возникших при работе бота
    /// </summary>
    /// <param name="botClient">Клиент, для которого возникло исключение</param>
    /// <param name="exception">Возникшее исключение</param>
    /// <param name="cancellationToken">Служебный токен для работы с многопоточностью</param>
    /// <returns></returns>
    Task OnErrorOccured(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        // В зависимости от типа исключения печатаем различные сообщения об ошибке
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

        // Завершаем работу
        return Task.CompletedTask;
    }
}