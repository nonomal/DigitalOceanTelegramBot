﻿using System;
using DigitalOceanBot.Types.Enums;

namespace DigitalOceanBot.Types.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    internal sealed class BotCommandAttribute : Attribute
    {
        public BotCommandType BotCommandType { get; }

        public BotCommandAttribute(BotCommandType botCommandType)
        {
            BotCommandType = botCommandType;
        }
    }
}