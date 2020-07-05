using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AirmenMod
{
    public static class Ships
    {
        private static List<Ship> _ships = new List<Ship>();
        private static List<Action<Ship>> _onShipCreate = new List<Action<Ship>>();
        private static List<Action<Ship>> _onShipDestroy = new List<Action<Ship>>();
        
        public static void OnShipCreate(Ship s)
        {
            AddShip(s);

            foreach (Action<Ship> f in _onShipCreate)
            {
                try
                {
                    f(s);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ShipCreate Function Error: {f.Method}");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.Source);
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        public static void ListenOnShipCreate(Action<Ship> f)
        {
            _onShipCreate.Add(f);
        }

        public static void OnShipDestory(Ship s)
        {
            RemoveShip(s);

            foreach (Action<Ship> f in _onShipDestroy)
            {
                try
                {
                    f(s);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"OnShipDestroy Function Error: {f.Method}");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.Source);
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        public static void ListenOnPlayerJoin(Action<Ship> f)
        {
            _onShipDestroy.Add(f);
        }

        private static void AddShip(Ship s)
        {

        }

        private static void RemoveShip(Ship s)
        {

        }

        public static List<Ship> GetShips()
        {
            List<Ship> ships = new List<Ship>();

            foreach (var sm in GMData.GMDataScript.allAirships)
            {
                ships.Add(new Ship(sm));
            }

            return ships;
        }

        public static int StringToInt(string id)
        {
            int parsedId = 0;
            int.TryParse(id, out parsedId);
            return parsedId;
        }

        public static Ship GetShipById(string id)
        {
            return GetShipById(StringToInt(id));
        }

        public static Ship GetShipById(int id)
        {
            foreach (var sm in GMData.GMDataScript.allAirships)
            {
                if (sm && sm.gameObject.GetInstanceID() == id)
                {
                    return new Ship(sm);
                }
            }

            return null;
        }

    }
}
