using System;
using System.Collections.Generic;

namespace LiveForks.Admin.Providers.Positiv
{
    public class PositivException : Exception
    {
        private static readonly Dictionary<EXceptionMessageType, string> _messages = new Dictionary<EXceptionMessageType, string>()
    {
      {
        EXceptionMessageType.NotLogin,
        "Клиент не авторизован!"
      },
      {
        EXceptionMessageType.AlreadyLogin,
        "Пользователь уже авторизован!"
      },
      {
        EXceptionMessageType.NonSubscribe,
        "Нет подписки!"
      },
      {
        EXceptionMessageType.BetNotFound,
        "Ставка пропала!"
      },
      {
        EXceptionMessageType.BadAuthData,
        "Неверный логин/пароль!"
      },
      {
        EXceptionMessageType.NotFirstLogin,
        "Вход еще ниразу не выполнялся!"
      },
      {
        EXceptionMessageType.UserBaned,
        "Ваш аккаунт заблокирован!"
      },
      {
        EXceptionMessageType.Error8,
        "Ошибка 8! Не все данные для betid"
      },
      {
        EXceptionMessageType.ErrorCheckExSubscribe,
        "Ошибка проверки расширенной подписки"
      },
      {
        EXceptionMessageType.Unknown,
        ""
      }
    };

        public EXceptionMessageType MessageType { get; }

        public PositivException(EXceptionMessageType messageType)
          : base(PositivException.GetMessage(messageType))
        {
            this.MessageType = messageType;
        }

        public PositivException(string message)
          : base(message)
        {
        }

        private static string GetMessage(EXceptionMessageType messageType)
        {
            return PositivException._messages[messageType];
        }
    }
}
