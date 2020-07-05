
# Airmen Mod Engine

This is a C# application to add plugin functionality to the game Airmen.

https://store.steampowered.com/app/647740/Airmen/

This has only been tested on the Linux server.

## Install Server

Example script

```
sudo apt-get update && sudo apt-get upgrade -y
sudo apt-get install -y steamcmd

# Not needed but could be handy ;)
sudo apt-get install htop screen

mkdir steam
mkdir -p $HOME/.config/unity3d/Airborne\ Games/Airmen/

cat <<EOT >> $HOME/.config/unity3d/Airborne\ Games/Airmen/ServerSettings.txt
{
    "name": "My Dedicated Server",
    "moderators": [],
    "ipBans": [
        "dummyIP1",
        "dummyIP2"
    ],
    "admins": [],
    "steamModerators": [],
    "steamBans": [],
    "steamAdmins": [],
    "gameMode": 0,
    "map": 0,
    "requireSteam": true,
    "steamAPIKey": "",
    "autoAI": false,
    "bonusTeamMass": 0,
    "bannedParts": [
        127,
        128,
        171,
        158
    ]
}
EOT

# Install 
steamcmd
> login anonymous
> force_install_dir /home/ubuntu/steam/airmen
> app_update 680870

cat <<EOT >> $HOME/steam/airmen/start.sh
export TERM=xterm
./airmenserv.x86_64 -batchmode -nographics
EOT

chmod +x $HOME/steam/airmen/start.sh

cd /root/steam/airmen

ln -s $HOME/.steam/steamcmd/linux64/steamclient.so $HOME/steam/airmen/
ln -s $HOME/.config/unity3d/Airborne\ Games/Airmen/ $HOME/steam/airmen/conf
```

## Install Mod Engine

View releases here: https://github.com/kingfredrickvi/AirmenModEngine/releases

Example script

```
wget https://github.com/kingfredrickvi/AirmenModEngine/releases/download/1.0/release1.0.tar.gz

$AIRMEN_PATH=/path/to/steam/airmen

mkdir -p $AIRMEN_PATH/plugins

tar -xzvf release1.0.tar.gz
mv AirmenMod.dll $AIRMEN_PATH/airmenserv_Data/Managed
mv Newtonsoft.Json.dll $AIRMEN_PATH/airmenserv_Data/Managed

# sudo apt-get install bsdiff

bspatch $AIRMEN_PATH/airmenserv_Data/Managed/Assembly-CSharp.dll $AIRMEN_PATH/airmenserv_Data/Managed/Assembly-CSharp.dll patch.dat
```

## Current Plugins List

* https://github.com/kingfredrickvi/AirmenWebAPI

## Creating a Plugin

Plugin development can happen on both Linux and Windows.

1. Download the latest release and extract the AirmenMod.dll
2. Create a new DLL project
3. Add AirmenMod.dll and `$AIRMEN_PATH/airmenserv_Data/Managed/Assembly-CSharp.dll` as a dependency

## Running a Plugin

1. Create a folder in `$AIRMEN_PATH/plugin/` for your mod
2. Add `mod.json` to your folder.
3. Build your dll from your plugin and add into the folder
4. Add any required dlls

Example `mod.json`

```
{
    "name": "My Plugin Name",
    "dlls": [
        "Newtonsoft.Json.dll"
    ],
    "priority": 1,
    "pluginfile": "AirmenWebAPI.dll",
    "pluginnamespace": "WebAPI",
    "pluginclass": "WebAPI"
}
```

All DLLs in every plugin's "dlls" is loaded first, then plugins are loaded and started lowest priority to highest priority.

## Current Issues

* OnPlugin___ functions producing errors when called
* Linq functions appear to have issues (maybe?)
* OnShipCreate and OnShipDestroy are not implemented
