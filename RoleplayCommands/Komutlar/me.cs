﻿using CommandSystem;
using Exiled.API.Features;
using HintServiceMeow.Core.Enum;
using HintServiceMeow.Core.Models.Hints;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RoleplayCommands.Komutlar
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class me : ICommand
    {


        public string Command { get; } = "me";
        public string[] Aliases { get; } = { "Me", "ME" };
        public string Description { get; } = "Me command";
        private static RoleplayCommands.config Config => main.Instance.Config;

        private static Dictionary<Player, float> playerCooldowns = new Dictionary<Player, float>();
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);

            var message = string.Join(" ", arguments);

            List<Player> nearbyPlayers = Komutlar.@do.GetPlayersInRange(player, Config.CommandRadius);

            if (message.Length > main.Instance.Config.MaxMessageLength)
            {
                message = message.Substring(0, main.Instance.Config.MaxMessageLength);
            }

            if (player.Role == RoleTypeId.Spectator || player.Role == RoleTypeId.Overwatch)
            {
                response = main.Instance.Translation.SpectatorsCantUseMessage;
                return false;
            }

            if (playerCooldowns.ContainsKey(player) && Time.time < playerCooldowns[player] + Config.CooldownTime)
            {
                response = main.Instance.Translation.CooldownMessage;
                return false;
            }
        
            if (arguments.Count == 0)
            {

                response = main.Instance.Translation.NoArguments;
                return false;
            }

            if (message.Contains("<") || message.Contains(">"))
            {
                response = "Geçersiz karakterler (< veya >) kullanılamaz!";
                return false;
            }

            foreach (Player p in nearbyPlayers)
            {
                string displayName = player.DisplayNickname;
                string nickname = player.Nickname;
                string displayNameText = player.Nickname;

                if (main.Instance.Config.ShowDisplayName)
                {
                    if (!string.Equals(nickname, displayName, StringComparison.Ordinal))
                    {
                        displayNameText = $"({nickname}) {displayName}";
                    }
                }

                HintServiceMeow.Core.Models.Hints.DynamicHint hint = new HintServiceMeow.Core.Models.Hints.DynamicHint()
                {
                    Text = $"<b><color=#6a6964ff> <size=14>{Config.ServerName}</size></color><color=#fade04ff> {displayNameText}</color><color=#6a6964ff> /</color> <b><color=#e420eeff>ME</color></b>: <color=#fade04ff>{message}</color></b>",
                    BottomBoundary = 950f,
                    RightBoundary = -1100,
                    LeftBoundary = -1100,
                    TargetX = -1100f,
                    TargetY = 950
                };

                HintServiceMeow.Core.Utilities.PlayerDisplay playerDisplay = HintServiceMeow.Core.Utilities.PlayerDisplay.Get(p);
                playerDisplay.AddHint(hint);
                Timing.CallDelayed(Config.HintDuration, () =>
                {
                    playerDisplay.RemoveHint(hint);
                });

                playerCooldowns[player] = Time.time;

            }



            response = main.Instance.Translation.MessageSent;
            return true;
        }

        
    }

}