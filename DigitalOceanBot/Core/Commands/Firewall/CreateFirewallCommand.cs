﻿using System.Threading.Tasks;
using DigitalOceanBot.Messages;
using DigitalOceanBot.Services;
using DigitalOceanBot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DigitalOceanBot.Core.Commands.Firewall
{
    public class CreateFirewallCommand : ICommand
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly StorageService _storageService;

        public CreateFirewallCommand(ITelegramBotClient telegramBotClient, StorageService storageService)
        {
            _telegramBotClient = telegramBotClient;
            _storageService = storageService;
        }

        public async Task ExecuteCommandAsync(Message message)
        {
            _storageService.AddOrUpdate(StorageKeys.BotCurrentState, StateType.FirewallWaitEnterCreationData);
            
            await _telegramBotClient.SendTextMessageAsync(
                chatId:message.Chat.Id, 
                text:FirewallMessage.GetEnterCreationDataFirewallMessage(), 
                parseMode:ParseMode.Html);
        }
    }
}