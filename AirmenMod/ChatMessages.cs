using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AirmenMod
{
    public class ChatMessages
    {
        private static List<Action<string, Player, int, Vector3?>> _onChatMessage = new List<Action<string, Player, int, Vector3?>>();

        public static void OnChatMessage(string m, GameObject sender, int team, commands from = null)
        {
            Player p = null;

            if (sender != null)
            {
                p = Players.GetBySteamId(sender.GetComponent<lobbyPlayerManager>().steamID);
            }

            Vector3? fromPos = null;

            if (from != null)
            {
                fromPos = from.pc.transform.position;
            }

            foreach (var f in _onChatMessage)
            {
                try
                {
                    f(m, p, team, fromPos);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"OnChatMessage Function Error: {f.Method}");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.Source);
                    Console.WriteLine(ex.StackTrace);
                }
                
            }
        }

        public static void ListenOnChatMessage(Action<string, Player, int, Vector3?> f)
        {
            _onChatMessage.Add(f);
        }

        public static void SendServerMessage(string message, Player p, int team)
        {
            if (p == null)
            {
                var players = Players.GetPlayers();

                if (players.Count > 0)
                {
                    players[0].GetManager().CallRpcSendChatMessage(message, null, team);
                }
            } else
            {
                p.GetManager().CallRpcSendChatMessage(message, p.GetGameObject(), team);
            }
        }

        public static bool SendServerMessageToPlayer(Player target, string message, Player from, int team)
        {
            if (target == null || !target.IsReady())
            {
                return false;
            }

            if (from == null)
            {
                target.SendMessage(message, null, team);
                return true;
            }
            else
            {
                GameObject fromObj = null;

                if (from != null)
                {
                    fromObj = from.GetGameObject();
                }

                target.SendMessage(message, fromObj, team);
                return false;
            }
        }
    }
}
