using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AirmenMod
{
    public static class ServerSettings
    {
        public static ServerInfo GetSettings()
        {
            return ModEngine.TeamManager.serverInfo;
        }

        public static List<string> GetSteamAdminsString()
        {
            var admins = new List<string>();
            foreach (var item in GetSettings().steamAdmins) admins.Add(item.ToString());
            return admins;
        }

        public static List<string> GetSteamModeratorsString()
        {
            var mods = new List<string>();
            foreach (var item in GetSettings().steamModerators) mods.Add(item.ToString());
            return mods;
        }

        public static List<string> GetSteamBansString()
        {
            var banned = new List<string>();
            foreach (var item in GetSettings().steamBans) banned.Add(item.ToString());
            return banned;
        }

        public static Dictionary<string, dynamic> ToDict()
        {
            var info = GetSettings();

            return new Dictionary<string, dynamic>() {
                { "admins", info.admins },
                { "autoAI", info.autoAI },
                { "bannedParts", info.bannedParts },
                { "bonusTeamMass", info.bonusTeamMass },
                { "gameMode", info.gameMode },
                { "ipBans", info.ipBans },
                { "map", info.map },
                { "moderators", info.moderators },
                { "name", info.name },
                { "requireSteam", info.requireSteam },
                { "steamAPIKey", info.steamAPIKey },
                { "steamAdmins", GetSteamAdminsString() },
                { "steamBans", GetSteamBansString() },
                { "steamModerators", GetSteamModeratorsString() }
            };
        }

        public static void Save()
        {
            File.WriteAllText(Application.persistentDataPath + "/ServerSettings.txt",
                JsonConvert.SerializeObject(ModEngine.TeamManager.serverInfo));
        }

        public static void AddModerator(string ip)
        {
            var arr = new List<string>() { ip };
            foreach (var item in ModEngine.TeamManager.serverInfo.moderators) arr.Add(item);
            ModEngine.TeamManager.serverInfo.moderators = arr.ToArray();
            Save();
        }

        public static void AddAdmin(string ip)
        {
            var arr = new List<string>() { ip };
            foreach (var item in ModEngine.TeamManager.serverInfo.admins) arr.Add(item);
            ModEngine.TeamManager.serverInfo.admins = arr.ToArray();
            Save();
        }

        public static void AddBan(string ip)
        {
            var arr = new List<string>() { ip };
            foreach (var item in ModEngine.TeamManager.serverInfo.ipBans) arr.Add(item);
            ModEngine.TeamManager.serverInfo.ipBans = arr.ToArray();
            Save();
        }

        public static void RemoveModerator(string ip)
        {
            var arr = new List<string>();
            foreach (var item in ModEngine.TeamManager.serverInfo.moderators) if (item != ip) arr.Add(item);
            ModEngine.TeamManager.serverInfo.moderators = arr.ToArray();
            Save();
        }

        public static void RemoveAdmin(string ip)
        {
            var arr = new List<string>();
            foreach (var item in ModEngine.TeamManager.serverInfo.admins) if (item != ip) arr.Add(item);
            ModEngine.TeamManager.serverInfo.admins = arr.ToArray();
            Save();
        }

        public static void RemoveBan(string ip)
        {
            var arr = new List<string>();
            foreach (var item in ModEngine.TeamManager.serverInfo.ipBans) if (item != ip) arr.Add(item);
            ModEngine.TeamManager.serverInfo.ipBans = arr.ToArray();
            Save();
        }

        public static bool AddSteamModerator(string id)
        {
            ulong parsedId = 0;
            ulong.TryParse(id, out parsedId);

            if (parsedId == 0)
            {
                return false;
            }

            var arr = new List<ulong>() { parsedId };
            foreach (var item in ModEngine.TeamManager.serverInfo.steamModerators) arr.Add(item);
            ModEngine.TeamManager.serverInfo.steamModerators = arr.ToArray();
            Save();

            return true;
        }

        public static bool AddSteamAdmin(string id)
        {
            ulong parsedId = 0;
            ulong.TryParse(id, out parsedId);

            if (parsedId == 0)
            {
                return false;
            }

            var arr = new List<ulong>() { parsedId };
            foreach (var item in ModEngine.TeamManager.serverInfo.steamAdmins) arr.Add(item);
            ModEngine.TeamManager.serverInfo.steamAdmins = arr.ToArray();
            Save();

            return true;
        }

        public static bool AddSteamBan(string id)
        {
            ulong parsedId = 0;
            ulong.TryParse(id, out parsedId);

            if (parsedId == 0)
            {
                return false;
            }

            var arr = new List<ulong>() { parsedId };
            foreach (var item in ModEngine.TeamManager.serverInfo.steamBans) arr.Add(item);
            ModEngine.TeamManager.serverInfo.steamBans = arr.ToArray();
            Save();

            return true;
        }

        public static bool RemoveSteamModerator(string id)
        {
            ulong parsedId = 0;
            ulong.TryParse(id, out parsedId);

            if (parsedId == 0)
            {
                return false;
            }

            var arr = new List<ulong>();
            foreach (var item in ModEngine.TeamManager.serverInfo.steamModerators) if (item != parsedId) arr.Add(item);
            ModEngine.TeamManager.serverInfo.steamModerators = arr.ToArray();
            Save();

            return true;
        }

        public static bool RemoveSteamAdmin(string id)
        {
            ulong parsedId = 0;
            ulong.TryParse(id, out parsedId);

            if (parsedId == 0)
            {
                return false;
            }

            var arr = new List<ulong>();
            foreach (var item in ModEngine.TeamManager.serverInfo.steamAdmins) if (item != parsedId) arr.Add(item);
            ModEngine.TeamManager.serverInfo.steamAdmins = arr.ToArray();
            Save();

            return true;
        }

        public static bool RemoveSteamBan(string id)
        {
            ulong parsedId = 0;
            ulong.TryParse(id, out parsedId);

            if (parsedId == 0)
            {
                return false;
            }

            var arr = new List<ulong>();
            foreach (var item in ModEngine.TeamManager.serverInfo.steamBans) if (item != parsedId) arr.Add(item);
            ModEngine.TeamManager.serverInfo.steamBans = arr.ToArray();
            Save();

            return true;
        }

        public static bool AddBannedPart(string id)
        {
            int parsedId = 0;
            int.TryParse(id, out parsedId);

            if (parsedId == 0)
            {
                return false;
            }

            var arr = new List<int>() { parsedId };
            foreach (var item in ModEngine.TeamManager.serverInfo.bannedParts) arr.Add(item);
            ModEngine.TeamManager.serverInfo.bannedParts = arr.ToArray();
            Save();

            return true;
        }

        public static bool RemoveBannedPart(string id)
        {
            int parsedId = 0;
            int.TryParse(id, out parsedId);

            if (parsedId == 0)
            {
                return false;
            }
            var arr = new List<int>();
            foreach (var item in ModEngine.TeamManager.serverInfo.bannedParts) if (item != parsedId) arr.Add(item);
            ModEngine.TeamManager.serverInfo.bannedParts = arr.ToArray();
            Save();

            return true;
        }
    }
}
