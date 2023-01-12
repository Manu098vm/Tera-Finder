# Tera Finder
![immagine](https://user-images.githubusercontent.com/52102823/210862408-8ea8f49f-cb73-4d9e-9ca5-c86a5dce8cb4.png)

This is a PKHeX.Core based program that allows to view, edit and calculate Raids for Scarlet & Violet. This is the spiritual sequel of my [SVXoroCalc](https://github.com/Manu098vm/SVResearches).
Both a standalone program and a PKHeX Plugin are available for use.

### Features:
* [Import Poké Portal News in Zip/Folder formats](https://github.com/Manu098vm/Tera-Finder/wiki/Raid-Event---Pok%C3%A9-Portal-News-Importer)
* [View & Edit Raids from a Save File](https://github.com/Manu098vm/Tera-Finder/wiki/Raid-Viewer-&-Editor)
* [Reverse a Pokémon into its origin Seed to check legality](https://github.com/Manu098vm/Tera-Finder/wiki/Seed-Checker)
* [Calculate RNG seeds that results in a Pokémon with given details](https://github.com/Manu098vm/Tera-Finder/wiki/Raid-Calculator)
* [Calculate RNG seeds that results in a Raid with wanted rewards](https://github.com/Manu098vm/Tera-Finder/wiki/Reward-Calculator)
* [Generate Legal PK9 files from Raid searches](https://github.com/Manu098vm/Tera-Finder/wiki/How-to-generate-Legal-PK9-Pok%C3%A9mon-from-Tera-Raids)
* [Edit Tera Raid related Game Progress/Caught Flags](https://github.com/Manu098vm/Tera-Finder/wiki/Game-Flags-Editor)
* [Supports localized languages](https://github.com/Manu098vm/Tera-Finder/wiki/General-Guide#about-the-localizations)
* [Supports Connections to Remote Devices (WiFi or USB)](https://github.com/Manu098vm/Tera-Finder/wiki/Connect-To-Remote-Device)

Powered by [PKHeX](https://github.com/kwsch/PKHeX), [pkNX](https://github.com/kwsch/pkNX) and [SysBot.NET](https://github.com/kwsch/SysBot.NET).

## How To
### Download And Usage
* See the [Initial Setup Page](https://github.com/Manu098vm/Tera-Finder/wiki/General-Guide) to begin with the program. 
* Check out the [Wiki](https://github.com/Manu098vm/Tera-Finder/wiki) for guides and details.

### Building
You can use any C# 10 & .NET 6.0 compatible IDE such as Visual Studio to compile this program.

Select either Debug or Release and click Build -> Build Solution.

The Standalone Launcher will be placed in `TeraFinder.Launcher/bin/`.

The Plugin files will be placed in `bin/`.

### Support
If you find any bug or you need support, please write a thread in the [Project Pokémon forums](https://projectpokemon.org/home/forums/).
Alternatively, feel free to contact me on my [Discord Server](https://discord.gg/F9nMfvw9sS).


## Credits and Thanks
* [kwsch](https://github.com/kwsch) for [PKHeX](https://github.com/kwsch/PKHeX), [pkNX](https://github.com/kwsch/pkNX) and [SysBot.NET](https://github.com/kwsch/SysBot.NET), on which this program relies on
* [Archit Date](https://github.com/architdate), [LegoFigure11](https://github.com/LegoFigure11), [SteveCookTU](https://github.com/SteveCookTU) for their great researches on the item rewards structures/rng handling implemented in [RaidCrawler](https://github.com/LegoFigure11/RaidCrawler/blob/main/Structures/RaidRewards.cs) and [sv raid reader](https://github.com/SteveCookTU/sv_raid_reader/blob/master/src/item_list.rs)
* [Lincoln-LM](https://github.com/Lincoln-LM) for the raid map coordinates and the save RAM pointer/logic in [sv-live-map](https://github.com/Lincoln-LM/sv-live-map)
* [Leanny](https://github.com/Leanny) for his Sword/Shield [PKHeX Raid Plugin](https://github.com/Leanny/PKHeX_Raid_Plugin) which was of inspiration for my plugin
* [santacrab2](https://github.com/santacrab2) for his contributions to make the searches lot faster
* [olliz0r](https://github.com/olliz0r) and [berichan](https://github.com/berichan) for [sys-botbase](https://github.com/olliz0r/sys-botbase)
* [fishguy6564](https://github.com/fishguy6564) and [Koi-3088](https://github.com/Koi-3088) for [usb-botbase](https://github.com/Koi-3088/USB-Botbase)
* [kwsch](https://github.com/kwsch), [Lusamine](https://github.com/Lusamine), [LegoFigure11](https://github.com/LegoFigure11), [Archit Date](https://github.com/architdate), [SteveCookTU](https://github.com/SteveCookTU), [Lincoln-LM](https://github.com/Lincoln-LM), and all the contributors to the mentioned programs for their awesome researches
* GitHub API implementation provided by [Octokit](https://github.com/octokit/octokit.net), licensed under the MIT license

### Some related cool tools
[sv-live-map](https://github.com/Lincoln-LM/sv-live-map) by [Lincoln-LM](https://github.com/Lincoln-LM)

[RaidCrawler](https://github.com/LegoFigure11/RaidCrawler) by [LegoFigure11](https://github.com/LegoFigure11) and [Archit Date](https://github.com/architdate)

[sv_raid_lookup](https://stevecooktu.github.io/sv_raid_lookup/) ([repo](https://github.com/SteveCookTU/sv_raid_lookup)) by [SteveCookTU](https://github.com/SteveCookTU)

[Raid Calc](https://github.com/MewTracker/sv-research) by [MewTracker](https://github.com/MewTracker)


## License
![gplv3-with-text-136x68](https://user-images.githubusercontent.com/52102823/199572700-4e02ed70-74ef-4d67-991e-3168d93aac0d.png)

Copyright © 2022 Manu098vm

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
