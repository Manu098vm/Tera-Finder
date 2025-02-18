# Tera Finder
![immagine](https://github.com/Manu098vm/Tera-Finder/assets/52102823/15f9d8c3-423c-4bfc-89c4-fb1495445c9e)

This is a PKHeX.Core based program that allows users to view, edit, and calculate Raids and Mass Outbreaks for Pokémon Scarlet & Violet. It is the spiritual successor to [SVXoroCalc](https://github.com/Manu098vm/SVResearches). 
Both a standalone program and a PKHeX plugin are available.

## Features:
* [Connect to a Remote Device](https://github.com/Manu098vm/Tera-Finder/wiki/Connect-To-Remote-Device)
* [View & Edit Mass Outbreaks](https://github.com/Manu098vm/Tera-Finder/wiki/Mass-Outbreak-Viewer-&-Editor)
* [View & Edit Raids](https://github.com/Manu098vm/Tera-Finder/wiki/Raid-Viewer-&-Editor)
* [Import Poké Portal News (Raid and Mass Outbreak Events) in Zip/Folder formats](https://github.com/Manu098vm/Tera-Finder/wiki/Raid-and-Mass-Outbreak-Events-%E2%80%90-Pok%C3%A9-Portal-News-Importer)
* [Reverse a Pokémon into its origin Seed to check legality](https://github.com/Manu098vm/Tera-Finder/wiki/Seed-Checker)
* [Calculate RNG seeds that result in a Pokémon with specific details](https://github.com/Manu098vm/Tera-Finder/wiki/Raid-Calculator)
* [Calculate RNG seeds that result in a Raid with desired rewards](https://github.com/Manu098vm/Tera-Finder/wiki/Reward-Calculator)
* [Generate Legal PK9 files from Raid searches](https://github.com/Manu098vm/Tera-Finder/wiki/How-to-generate-Legal-PK9-Pok%C3%A9mon-from-Tera-Raids)
* [Edit Tera Raid related Game Progress/Caught Flags](https://github.com/Manu098vm/Tera-Finder/wiki/Game-Flags-Editor)
* [Supports localized languages](https://github.com/Manu098vm/Tera-Finder/wiki/General-Guide#about-the-localizations)

Powered by [PKHeX](https://github.com/kwsch/PKHeX), [pkNX](https://github.com/kwsch/pkNX), and [SysBot.NET](https://github.com/kwsch/SysBot.NET).

## How To
### Download and Usage
* Refer to the [Initial Setup Page](https://github.com/Manu098vm/Tera-Finder/wiki/General-Guide) to get started.
* Check out the [Wiki](https://github.com/Manu098vm/Tera-Finder/wiki) for guides and details.

### Building
You can use any C# 13 & .NET 9.0 compatible IDE, such as Visual Studio, to compile this program.

Select either Debug or Release mode and click **Build** -> **Build Solution**.

* The Standalone Launcher will be placed in `TeraFinder.Launcher/bin/`.
* The Plugin files will be placed in `TeraFinder.Plugins/bin/`.

### Support/Troubleshooting
For common troubleshooting, please check the [appropriate section in the wiki](https://github.com/Manu098vm/Tera-Finder/wiki#troubleshooting).
If you find a bug or need support, please leave a comment in the [Project Pokémon thread](https://projectpokemon.org/home/forums/topic/62964-scvi-tera-finder-saveram-tera-raid-viewer-editor-calculator-and-more/).
Alternatively, feel free to contact me on my [Discord Server](https://discord.gg/yWveAjKbKt):

[<img src="https://canary.discordapp.com/api/guilds/693083823197519873/widget.png?style=banner2">](https://discord.gg/yWveAjKbKt)

## Credits and Thanks
* [kwsch](https://github.com/kwsch) for [PKHeX](https://github.com/kwsch/PKHeX), [pkNX](https://github.com/kwsch/pkNX), and [SysBot.NET](https://github.com/kwsch/SysBot.NET), which this program relies on
* [Archit Date](https://github.com/architdate), [LegoFigure11](https://github.com/LegoFigure11), [SteveCookTU](https://github.com/SteveCookTU) for their research on item reward structures and event group ID handling implemented in [RaidCrawler](https://github.com/LegoFigure11/RaidCrawler/blob/main/Structures/RaidRewards.cs) and [SV Raid Reader](https://github.com/SteveCookTU/sv_raid_reader/blob/master/src/item_list.rs)
* [Lincoln-LM](https://github.com/Lincoln-LM) for the raid map coordinates and save RAM logic in [sv-live-map](https://github.com/Lincoln-LM/sv-live-map)
* [Archit Date](https://github.com/architdate) for his C# port of Lincoln's logic implemented in [RaidCrawler](https://github.com/LegoFigure11/RaidCrawler), on which my code is based
* [Lusamine](https://github.com/Lusamine) for the Key Block pointer taken from the disassembled game code for v1.2.0
* [Leanny](https://github.com/Leanny) for the Sword/Shield [PKHeX Raid Plugin](https://github.com/Leanny/PKHeX_Raid_Plugin), which served as inspiration for my plugin
* [santacrab2](https://github.com/santacrab2) for contributions improving search speed and adding DLC Location Names
* [Archit Date](https://github.com/architdate), [Lusamine](https://github.com/Lusamine), [santacrab2](https://github.com/santacrab2) for their SearchSaveKey implementation in [RaidCrawler](https://github.com/LegoFigure11/RaidCrawler/blob/f8e996aac4b134e6eb6231d539c345748fead490/RaidCrawler.Core/Connection/ConnectionWrapper.cs#L126)
* [Zyro670](https://github.com/zyro670) and [santacrab2](https://github.com/santacrab2) for their help with RAM block reading/writing, specifically related to outbreaks
* [olliz0r](https://github.com/olliz0r) and [berichan](https://github.com/berichan) for [sys-botbase](https://github.com/olliz0r/sys-botbase)
* [fishguy6564](https://github.com/fishguy6564) and [Koi-3088](https://github.com/Koi-3088) for [usb-botbase](https://github.com/Koi-3088/USB-Botbase)
* [kwsch](https://github.com/kwsch), [Archit Date](https://github.com/architdate), [LegoFigure11](https://github.com/LegoFigure11), [Lusamine](https://github.com/Lusamine), and [Lincoln-LM](https://github.com/Lincoln-LM) for the Coordinates dumps and map display [formulas](https://github.com/LegoFigure11/RaidCrawler/blob/d36475046c638fbc37fbeb0aaa001f3663273b9b/RaidCrawler.WinForms/MainWindow.cs#L1589)
* All contributors to the mentioned projects for their valuable research

### Some Related Tools
* [RaidCalc](https://github.com/MewTracker/sv-research) by [MewTracker](https://github.com/MewTracker)
* [RaidCrawler](https://github.com/LegoFigure11/RaidCrawler) by [LegoFigure11](https://github.com/LegoFigure11) and [Archit Date](https://github.com/architdate)
* [sv-live-map](https://github.com/Lincoln-LM/sv-live-map) by [Lincoln-LM](https://github.com/Lincoln-LM)
* [sv_raid_lookup](https://stevecooktu.github.io/sv_raid_lookup/) ([repo](https://github.com/SteveCookTU/sv_raid_lookup)) by [SteveCookTU](https://github.com/SteveCookTU)

## License
![gplv3-with-text-136x68](https://user-images.githubusercontent.com/52102823/199572700-4e02ed70-74ef-4d67-991e-3168d93aac0d.png)

Copyright © 2025 Manu098vm

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
