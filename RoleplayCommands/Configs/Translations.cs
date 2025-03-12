using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleplayCommands.Configs
{
   public class Translations : ITranslation
    {
        public string CooldownMessage { get; set; } = "Bu komutu terkar kullanmak için biraz beklemelisin!";
        public string NoArguments { get; set; } = "Bir mesaj yazmalısın!";

        public string SpectatorsCantUseMessage { get; set; } = "Bu komutu izleyiciler kullanamaz!";

        public string MessageSent { get; set; } = "Mesaj başarıyla gönderildi!";
    }
}
