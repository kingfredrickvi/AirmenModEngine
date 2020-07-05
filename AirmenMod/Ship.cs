using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AirmenMod
{
    public enum BountyReason: int
    {
        AttackingShip,
        DestoryingShip,
        AirSpace,
        BlackFlag
    }

    public class Ship
    {
        private readonly shipManager _sm;

        public Ship(shipManager sm)
        {
            _sm = sm;
        }
        
        public int GetId()
        {
            return _sm.gameObject.GetInstanceID();
        }

        public void Teleport(Vector3 pos)
        {
            var pilot = _sm.pilot;

            if (pilot)
            {
                pilot.GetComponent<commands>().onlyThisUser.CallRpcTeleport(pos);
            }
        }

        public void RepairShip(int amount = 500000)
        {
            _sm.RepairShip(amount);
        }

        public void RefillTools(int amount = 500000)
        {
            _sm.RefillTools(amount);
        }

        public void RefillScrap(int amount = 500000)
        {
            _sm.RefillScrap(amount);
        }

        public bool DidPlayerDamageShip(Player p)
        {
            return _sm.DidPlayerDamageShip(p._getHp());
        }

        public HashSet<int> GetAttackerTeams()
        {
            HashSet<int> teams = new HashSet<int>();
            _sm.GrabAttackerTeams(teams);
            return teams;
        }

        public int GetStartMass()
        {
            return _sm.GetStartMass();
        }

        public bool IsAi()
        {
            return _sm.isAI;
        }

        public int GetTeam()
        {
            return _sm.team;
        }

        public void AddBounty(int amount = 5, BountyReason reason = 0)
        {
            SetBounty(GetBounty() + amount, reason);
        }

        public void RemoveBounty()
        {
            SetBounty(-GetBounty(), 0);
        }

        public void SetBounty(int amount, BountyReason reason = 0)
        {
            // _sm.IncreaseBounty(amount);
            _sm.CallRpcSetBounty(GetBounty(), (int) reason);
        }

        public int GetBounty()
        {
            return _sm.bounty;
        }

        public void DistributeBounty()
        {
            _sm.DistributeBounty();
        }

        public void DistributeSkullBounty()
        {
            _sm.DistributeSkullBounty();
        }

        public void AddCargo(CargoParcel.CargoItem cargo, int amount)
        {
            _sm.FillWithCargo((int)cargo, amount);
        }

        public bool IsFirepowerLegal()
        {
            return _sm.IsFirepowerLegal();
        }

        public bool WasRecentlyDamaged()
        {
            return _sm.IsRecentlyDamaged();
        }

        public static string[] GetBountyReasons()
        {
            return new string[]
            {
                "Wanted: Attacking a Bountyless Ship",
                "Wanted: Destroying a Ship",
                "Wanted: Air Space Violation",
                "Wanted: Raised the Black Flag"
            };
        }

        public shipManager.CrewOrders GetCrewOrders()
        {
            return _sm.crewOrders;
        }

        public bool HasEverHadBounty()
        {
            return _sm.everHadABounty;
        }

        public Vector3 GetGravity()
        {
            return _sm.gravityForce;
        }
        
        public bool HasPressure()
        {
            return _sm.hasPressure;
        }

        public bool IsAirship()
        {
            return _sm.airship;
        }

        public bool IsLandship()
        {
            return _sm.landship;
        }

        public bool IsWatercraft()
        {
            return _sm.waterCraft;
        }

        public ulong GetOriginalCreator()
        {
            return _sm.originalCreator;
        }

        public int GetPartCount()
        {
            return _sm.partCount;
        }

        public Player GetPilot()
        {
            if (!IsAi() && _sm.pilot)
            {
                return Players.GetBySteamId(GetPilotSteamId());
            }

            return null;
        }

        public ulong GetPilotSteamId()
        {
            if (!IsAi() && _sm.pilot)
            {
                return _sm.pilot.GetComponent<playerHP>().steamId;
            }

            return 0;
        }

        public bool IsPirateKing()
        {
            return _sm.pirateKing;
        }

        public bool IsPlayerFortress()
        {
            return _sm.playerFortress;
        }

        public float GetMass()
        {
            return _sm.rb.mass;
        }

        public shipManager.disabledStatus GetState()
        {
            return _sm.shipState;
        }

        public bool HasSkullBounty()
        {
            return _sm.skullBounty;
        }

        public int GetMaxPressure()
        {
            return _sm.pressureMax;
        }

        public int GetPressureReq()
        {
            return _sm.pressureReq;
        }

        public Vector3 GetPosition()
        {
            return _sm.positionSync.netPosition;
        }

        public Quaternion GetRotation()
        {
            return _sm.positionSync.netRotation;
        }

        public Vector3 GetEulerRotation()
        {
            return GetRotation().eulerAngles;
        }

        public Dictionary<string, dynamic> ToDict()
        {
            return new Dictionary<string, dynamic>()
            {
                { "uid", GetId().ToString() },
                { "isAi", IsAi().ToString() },
                { "team", GetTeam().ToString() },
                { "bounty", GetBounty().ToString() },
                { "isFirepowerLegal", IsFirepowerLegal().ToString() },
                { "wasRecentlyDamaged", WasRecentlyDamaged().ToString() },
                { "crewOrders", GetCrewOrders().ToString() },
                { "hasEverHadBounty", HasEverHadBounty().ToString() },
                { "gravity", GetGravity().ToString() },
                { "hasPressure", HasPressure().ToString() },
                { "isAirship", IsAirship().ToString() },
                { "isLandship", IsLandship().ToString() },
                { "isWatercraft", IsWatercraft().ToString() },
                { "originalCreator", GetOriginalCreator().ToString() },
                { "partCount", GetPartCount().ToString() },
                { "pilot", GetPilotSteamId().ToString() },
                { "isPirateKing", IsPirateKing().ToString() },
                { "isPlayerFortress", IsPlayerFortress().ToString() },
                { "mass", GetMass().ToString() },
                { "state", GetState().ToString() },
                { "hasSkullBounty", HasSkullBounty().ToString() },
                { "maxPressure", GetMaxPressure().ToString() },
                { "pressureReq", GetPressureReq().ToString() },
                { "position", GetPosition().ToString() },
                { "rotation", GetEulerRotation().ToString() }
            };
        }
    }
}
