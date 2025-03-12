using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
using Exiled.Loader;
using player = Exiled.Events.Handlers.Player;
using PluginAPI.Core;
namespace RoleplayCommands
{
    public class main : Plugin<config, RoleplayCommands.Configs.Translations>
    {
        public static main Instance;
        public override string Name => "RoleplayCommands";
        public override string Author => "Baggins (@haci33)";

        public override void OnEnabled()
        {
            base.OnEnabled();
            Instance = this;
        }

        public override void OnDisabled()
        {
            base.OnDisabled();

            Instance = null;
        }
    }
}
