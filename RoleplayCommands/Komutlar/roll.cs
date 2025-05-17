using CommandSystem;
using Exiled.API.Features;
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
    public class roll : ICommand
    {
        public string Command { get; } = $"{Config.RollCommandName}";
        public string[] Aliases { get; } = { "roll", "rolldice" };

        System.Random rnd = new System.Random();
        public string Description { get; } = "Roll command";
        private static RoleplayCommands.config Config => main.Instance.Config;
        private static Dictionary<Player, float> playerCooldowns = new Dictionary<Player, float>();
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);

            var message = rnd.Next(1, 7).ToString();

            if (message.Length > main.Instance.Config.MaxMessageLength)
            {
                message = message.Substring(0, main.Instance.Config.MaxMessageLength);
            }

            List<Player> nearbyPlayers = Komutlar.@do.GetPlayersInRange(player, Config.CommandRadius);

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

            if (message.Contains("<") || message.Contains(">"))
            {
                response = "Geçersiz karakterler (< veya >) kullanılamaz!";
                return false;
            }

            foreach (Player p in nearbyPlayers)
            {
                HintServiceMeow.Core.Models.Hints.DynamicHint hint = new HintServiceMeow.Core.Models.Hints.DynamicHint()
                {
                    Text = $"<b><color=#6a6964ff> <size=14>{Config.ServerName}</size></color><color=#fade04ff> {player.Nickname}</color><color=#6a6964ff> /</color> <b><color=#200ae7ff>ROLL</color></b>: <color=#fade04ff>{message}</color></b>",
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



            response = "Mesaj başarıyla gönderildi!";
            return true;
        }

    }
}
