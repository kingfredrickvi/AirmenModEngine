using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AirmenMod
{
    public static class Players
    {
        private static List<Player> _players = new List<Player>();
        private static List<Action<Player>> _onPlayerJoin = new List<Action<Player>>();
        private static List<Action<Player>> _onPlayerLeave = new List<Action<Player>>();
        private static List<Action<Player>> _onPlayerReady = new List<Action<Player>>();

        public static void OnPlayerJoin(ulong steamId, string alias)
        {
            lobbyPlayerManager pm = null;

            foreach (var _pm in DMTeamManager.DMTM.lobbyPlayerManagers) {
                if (_pm.steamID == steamId)
                {
                    pm = _pm;
                    break;
                }
            }

            if (pm == null)
            {
                Console.WriteLine($"OnPlayerJoin: Failed to find ({steamId}) {alias} in lobby players.");
            }

            var p = new Player(pm);

            AddPlayer(p);

            Console.WriteLine($"OnPlayerJoin: ({steamId}) {alias}");

            foreach (var f in _onPlayerJoin)
            {
                try
                {
                    f(p);
                } catch (Exception ex)
                {
                    Console.WriteLine($"OnPlayerJoin Function Error: {f.Method}");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.Source);
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        public static void ListenOnPlayerJoin(Action<Player> f)
        {
            _onPlayerJoin.Add(f);
        }

        public static void OnPlayerReady(GameObject ply)
        {
            var hp = ply.GetComponent<playerHP>();
            var p = GetBySteamId(hp.steamId);

            if (p == null)
            {
                Console.WriteLine($"OnPlayerReady: Didn't find ({hp.steamId}) {hp.alias} in list of players.");
                OnPlayerJoin(hp.steamId, hp.alias);
                p = GetBySteamId(hp.steamId);

                if (p == null)
                {
                    Console.WriteLine("OnPlayerReady: Failed to add player to join.");
                    return;
                }
            }

            p.Ready(ply);

            Console.WriteLine($"OnPlayerReady: {p.GetAlias()}");

            foreach (var f in _onPlayerReady)
            {
                try
                {
                    f(p);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"OnPlayerReady Function Error: {f.Method}");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.Source);
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        public static void ListenOnPlayerReady(Action<Player> f)
        {
            _onPlayerReady.Add(f);
        }

        public static void OnPlayerLeave(ulong steamId)
        {
            var p = GetBySteamId(steamId);

            if (p == null)
            {
                Console.WriteLine($"OnPlayerLeave: Failed to find {steamId}.");
                return;
            }

            RemovePlayer(p);

            Console.WriteLine($"OnPlayerLeave: {p.GetAlias()}");

            foreach (var f in _onPlayerLeave)
            {
                try
                {
                    f(p);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"OnPlayerLeave Function Error: {f.Method}");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.Source);
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        public static void ListenOnPlayerLeave(Action<Player> f)
        {
            _onPlayerLeave.Add(f);
        }

        public static ulong StringToUlong(string id)
        {
            ulong parsedId = 0;
            ulong.TryParse(id, out parsedId);
            return parsedId;
        }

        public static Player GetBySteamId(string steamId)
        {
            return GetBySteamId(StringToUlong(steamId));
        }

        public static Player GetBySteamId(ulong steamId)
        {
            foreach (var player in _players)
            {
                if (player.GetSteamId() == steamId)
                {
                    return player;
                }
            }

            return null;
        }

        private static void AddPlayer(Player p)
        {
            _players.Add(p);
        }

        private static void RemovePlayer(Player p)
        {
            _players.Remove(p);
        }

        public static List<Player> GetPlayers()
        {
            return Players._players;
        }
    }
}
