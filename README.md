# KPS Allocator

## What is it?

A Simple Weapon Allocator For <https://github.com/b3none/cs2-retakes>.

I made this for our own community server as at that time there was no weapn allocation mod that fit our needs.

## Installation

1. Make sure CS2Retakes is installed (<https://github.com/yonilerner/cs2-retakes-allocator/releases>)
2. Download the latest KPS Allocator release from <https://github.com/nokkvireyr/kps-allocator/releases>
3. extract the zip file to your CSSharp plugins folder
4. Start your server and enjoy!

## Usage

### Commands

This plugins consist of only (2) commands

- `!guns/!gun` brings up a gun selection menu for the team you are currently on.
- `!guns/!gun <CT/T>` brings up a gun selection menu for the team that is selected in the command.

## Configuration

The configuration file is located in `/addons/counterstrikesharp/plugins/KPSAllocator`

### Database Configuration

#### database.json

The plugins supports both MySQL and SQLite. You can choose which one you want to use by changing the `engine` value in the `database.json` file. Default is SQLite.

```JSON
{
  "engine": "sql",        //-> Change this to "mysql" if you want to use MySQL
  "Host": "localhost",    //-> Only need to change this if you are using MySQL
  "Port": 3306,           //-> Only need to change this if you are using MySQL
  "User": "username",     //-> Only need to change this if you are using MySQL
  "Pass": "password",     //-> Only need to change this if you are using MySQL
  "Database": "database", //-> Only need to change this if you are using MySQL
  "Version": "1.0.0"      //-> Don't change this
}
```

### Game Configuration

#### game.json

This file is used to configure the weapon allocator settings. Ex Util settings, Round Types, Random Rounds and so on.

```JSON
{
  //If true, the rounds will be roundomly played, not linearly.
  "RandomRound": false, 
  "RoundPercentage": {
    //If true, the round will be played as a fixed rounds so ex FullBuy: 20, SmallBuy: 5, Pistol: 5 will play 20 FullBuy rounds, 5 SmallBuy rounds and 5 Pistol rounds. And if there are still more rounds left it will fallback to play the remaining rounds as FullBuy rounds.
    "IsFixed": false,
    // These are the round percentages. So default is 83% FullBuy, 7% SmallBuy and 10% Pistol (Make sure it adds up to 100%). If Fixed round are used then this needs to be changed to round numbers and not percentages.
    "FullBuy": 83,
    "SmallBuy": 7,
    "Pistol": 10
  },
  // RoundUtilValue is used to determmin what Util value each round has
  "RoundUtilValue": [
    // Default for pistol round is 0 - 100, Which does not provide any util. This can be changed to 101 - 102 to provide a flashbang.
    {
      "Min": 0,
      "Max": 100,
      "RoundType": "Pistol"
    },
    // Default for Small Buy Rounds is 300 - 500 which mean the plugin gives a player a random "value" and distributes utils based on that value.
    {
      "Min": 300,
      "Max": 500,
      "RoundType": "SmallBuy"
    },
    // Default for Small Buy Rounds is 500 - 800 which mean the plugin gives a player a random "value" and distributes utils based on that value.
    {
      "Min": 500,
      "Max": 800,
      "RoundType": "FullBuy"
    }
  ],
  // This is to set the value of the utils. So if you want to change the value of a flashbang to 200 you would change the value to 200. You can also change the amount of util that the use can be given for each type (Only viable for flashbangs, other wise it will drop the util on the ground). The plugin will then randomly distribute the utils based on the value the player was given (configured above).
  "UtilValues": [
    {
      "Item": "Flashbang",
      "Value": 200,
      "Amount": 1
    },
    {
      "Item": "Smoke",
      "Value": 300,
      "Amount": 1
    },
    {
      "Item": "Molotov",
      "Value": 500,
      "Amount": 1
    },
    {
      "Item": "HighExplosive",
      "Value": 200,
      "Amount": 1
    }
  ],
  "Version": "1.0.0"
}
```
