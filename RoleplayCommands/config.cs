using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Interfaces;
namespace RoleplayCommands
{
    public class config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        public bool Debug { get; set; } = false;

        public float HintDuration { get; set; } = 10f;

        public float CommandRadius { get; set; } = 10f;

        public float CooldownTime { get; set; } = 8f;

        public string ServerName { get; set; } = " ServerName ";

        public string RollCommandName { get; set; } = "zarat";

        public int MaxMessageLength { get; set; } = 30;
    }
}
