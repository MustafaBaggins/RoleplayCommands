﻿using CommandSystem;
using CommandSystem.Commands.RemoteAdmin;
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
    public class @do : ICommand
    {
     
        public string Command { get; } = "do";
        public string[] Aliases { get; } = { "Do", "DO" };
        public string Description { get; } = "do command";

        private static RoleplayCommands.config Config => main.Instance.Config;
        private static Dictionary<Player, float> playerCooldowns = new Dictionary<Player, float>();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);

            var message = string.Join(" ", arguments);

            List<Player> nearbyPlayers = GetPlayersInRange(player, Config.CommandRadius);

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
                HintServiceMeow.Core.Models.Hints.DynamicHint hint = new HintServiceMeow.Core.Models.Hints.DynamicHint()
                {
                    Text = $"<b><color=#6a6964ff> <size=14>{Config.ServerName}</size></color><color=#fade04ff> {player.Nickname}</color><color=#6a6964ff> /</color> <b><color=#2ac9c2ff>DO</color></b>: <color=#fade04ff>{message}</color></b>",
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


            }

            playerCooldowns[player] = Time.time;

            response = main.Instance.Translation.MessageSent;
            return true;
        }

        public static List<Player> GetPlayersInRange(Player player, float radius)
        {
            List<Player> nearbyPlayers = new List<Player>();
            Vector3 playerPosition = player.Position;

            foreach (Player p in Player.List)
            {
                if (p != player && Vector3.Distance(p.Position, playerPosition) <= radius)
                {
                    nearbyPlayers.Add(p);
                }
            }
            nearbyPlayers.Add(player);

            return nearbyPlayers;
        }
    }
}

