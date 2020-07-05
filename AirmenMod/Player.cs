using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AirmenMod
{
    public class Player
    {
        private GameObject _player;
        private lobbyPlayerManager _manager;
        private bool _ready = false;

        public Player(lobbyPlayerManager manager)
        {
            _manager = manager;
        }

        public void Ready(GameObject p)
        {
            _player = p;
            _ready = true;
        }

        public bool IsReady()
        {
            return _ready;
        }

        public lobbyPlayerManager GetLobbyManager()
        {
            return _manager;
        }

        public void Ban()
        {

        }

        public void SetPosition(Vector3 pos, Quaternion? rot = null)
        {
            if (!IsReady()) return;

            var pd = _getHp();

            if (rot == null) rot = GetRotation();

            pd.pc.cmds.positionSync.SnapToPosition(pos, (Quaternion) rot);
            pd.GetComponent<commands>().onlyThisUser.CallRpcSummonPlayer(pos);
        }

        public void ChangeHp(int amount)
        {
            if (!IsReady()) return;

            SetHp(GetHp() + amount);
        }

        public void Kill()
        {
            if (!IsReady()) return;

            var pd = _getHp();

            pd.hp = -99999;
            commands.cmds.CallRpcServerDamagePlayer(99999, pd.gameObject);
        }

        public void Kick()
        {
            if (!IsReady()) return;

            _getHp().pc.cmds.onlyThisUser.KickFromServer();
        }

        public GameObject GetGameObject()
        {
            if (!IsReady()) return null;
            return _player;
        }

        public void Teleport(Vector3 pos)
        {
            var pd = _getHp();
            pd.pc.cmds.positionSync.SnapToPosition(pos, pd.pc.transform.rotation);
            pd.GetComponent<commands>().onlyThisUser.CallRpcSummonPlayer(pos);
        }

        public void SetHp(int value)
        {
            if (!IsReady()) return;

            var pd = _getHp();
            pd.hp = value;
            commands.cmds.CallRpcSyncHP(pd.gameObject, pd.hp);
        }

        public void ResetHp()
        {
            if (!IsReady()) return;

            var pd = _getHp();
            pd.hp = 100;
            commands.cmds.CallRpcSyncHP(pd.gameObject, pd.hp);
        }

        public playerHP _getHp()
        {
            if (!IsReady()) return null;

            return _player.GetComponent<playerHP>();
        }

        public string GetName()
        {
            if (!IsReady()) return null;

            return _getHp().name;
        }

        public int GetCrew()
        {
            if (!IsReady()) return -1;
            return _getHp().crew;
        }

        public int GetHp()
        {
            if (!IsReady()) return 0;

            return _getHp().hp;
        }

        public int GetTeam()
        {
            if (!IsReady()) return -1;

            return _getHp().team;
        }

        public string GetAlias()
        {
            if (!IsReady()) return _manager.alias;

            return _getHp().alias;
        }

        public bool IsDead()
        {
            if (!IsReady()) return true;

            return _getHp().dead;
        }

        public int GetDeaths()
        {
            if (!IsReady()) return -1;

            return _getHp().deaths;
        }

        public int GetMaxHp()
        {
            if (!IsReady()) return -1;

            return _getHp().hpMax;
        }

        public bool IsCaptain()
        {
            if (!IsReady()) return false;

            return _getHp().isCaptain;
        }

        public bool IsMod()
        {
            if (!IsReady()) return false;

            return _getHp().isMod;
        }

        public ulong GetSteamId()
        {
            if (!IsReady()) return _manager.steamID;

            return _getHp().steamId;
        }

        public lobbyPlayerManager GetManager()
        {
            return _manager;
        }

        public void SendMessage(string message, GameObject sender, int team)
        {
            //_manager.CallRpcSendChatMessage(message, sender, team);
        }

        public Player LastDamagedBy()
        {
            if (!IsReady()) return null;

            var ld = _getHp().lastDamagedBy;

            if (ld)
            {
                foreach (var p in Players.GetPlayers())
                {
                    if (p.GetSteamId() == ld.steamId)
                    {
                        return p;
                    }
                }
            }

            return null;
        }

        public void LastPilotedShip()
        {

            if (!IsReady()) return;
        }

        public bool IsPermaDead()
        {
            if (!IsReady()) return false;

            return _getHp().permaDead;
        }

        public bool IsPiloting()
        {
            if (!IsReady()) return false;

            return _getHp().piloting;
        }

        public Vector3 GetPosition()
        {
            if (!IsReady()) return Vector3.zero;

            return _getHp().pc.transform.position;
        }

        public Quaternion GetRotation()
        {
            if (!IsReady()) return Quaternion.identity;

            return _getHp().pc.transform.rotation;
        }

        public Dictionary<string, dynamic> ToDict()
        {
            return new Dictionary<string, dynamic>()
            {
                { "steamId", GetSteamId().ToString() },
                { "dead", IsDead().ToString() },
                { "isCaptain", IsCaptain().ToString() },
                { "piloting", IsPiloting().ToString() },
                { "deaths", GetDeaths().ToString() },
                { "team", GetTeam().ToString() },
                { "crew", GetCrew().ToString() },
                { "hp", GetHp().ToString() },
                { "position", GetPosition().ToString() },
                { "rotation", GetRotation().ToString() },
                { "isReady", IsReady().ToString() },
                { "name", GetAlias().ToString() }
            };
        }
    }
}
