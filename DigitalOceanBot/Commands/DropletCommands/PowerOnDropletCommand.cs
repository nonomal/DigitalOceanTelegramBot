﻿using System;
using DigitalOcean.API.Exceptions;
using DigitalOceanBot.Factory;
using DigitalOceanBot.MongoDb;
using DigitalOceanBot.MongoDb.Models;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DigitalOceanBot.Commands.DropletCommands
{
    internal sealed class PowerOnDropletCommand : DigitalOceanActionBase, IBotCommand
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<DigitalOceanWorker> _logger;

        public PowerOnDropletCommand(
            ILogger<DigitalOceanWorker> logger,
            ITelegramBotClient telegramBotClient,
            IRepository<DoUser> userRepo,
            IRepository<Session> sessionRepo,
            IDigitalOceanClientFactory digitalOceanClientFactory) : base(logger, telegramBotClient, userRepo, sessionRepo, digitalOceanClientFactory)
        {
            _telegramBotClient = telegramBotClient;
            _logger = logger;
        }

        public async void Execute(Message message, SessionState sessionState)
        {
            try
            {
                await _telegramBotClient.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

                switch (sessionState)
                {
                    case SessionState.SelectedDroplet:
                        PowerOnDroplet(message);
                        break;
                }
            }
            catch (ApiException ex)
            {
                _logger.LogError($"UserId={message.From.Id.ToString()}, Error={ex.Message}");
                await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"DigitalOcean API Error: {ex.Message.Replace(".", "\\.")}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"UserId={message.From.Id.ToString()}, Error={ex.Message}, StackTrace={ex.StackTrace}");
                await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, "Sorry, аn error has occurred \U0001F628");
            }
        }


        private void PowerOnDroplet(Message message)
        {
            StartActionWithoutConfirm(message, "Power on", async (digitalOceanApi, dropletId) => await digitalOceanApi.DropletActions.PowerOn(dropletId));
        }
    }
}
