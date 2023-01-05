# Tera Finder
![immagine](https://user-images.githubusercontent.com/52102823/210862408-8ea8f49f-cb73-4d9e-9ca5-c86a5dce8cb4.png)

This is a PKHeX.Core based program that allows to view, edit and calculate seeds for SV Raids. This is the spiritual sequel of my [SVXoroCalc](https://github.com/Manu098vm/SVResearches).
Both a standalone program and a PKHeX Plugin are available for use.

### Features:
* Import Poké Portal News in Zip/Folder formats
* Check & Edit Raids from a Save File
* Reverse a Pokémon into its origin Seed to check legality
* Calculate an RNG seeds that results in a Pokémon with given details
* Calculate an RNG seeds that results in wanted rewards
* Edit Tera Raid related Game Progress/Caught Flags
* Supports localized languages

Powered by [PKHeX](https://github.com/kwsch/PKHeX) and [pkNX](https://github.com/kwsch/pkNX).

## How To
### Download
Check the [Release page](https://github.com/Manu098vm/Tera-Finder/releases) for the latest stable build, or [Git Actions](https://github.com/Manu098vm/Tera-Finder/actions) for the latest dev build. 

### Building
You can use any C# 10 & .NET 6.0 compatible IDE such as Visual Studio to compile this program.

Select either Debug or Release and click Build -> Build Solution.

The Standalone Launcher will be placed in `TeraFinder.Launcher/bin/`.

The Plugin files will be placed in `bin/`.

### Standalone Launcher Usage
Open the `TeraFinder.Launcher.exe` program to open its GUI.

In order to unlock all the functionalities, load a valid SV Save File by drag & drop or click the `Load Save File` button.

The usage should be fairly intuitive.

### PKHeX Plugin Usage
Currently the plugin only works with a .NET 6 build of PKHeX. You'll need to compile PKHeX from its [source code](https://github.com/kwsch/PKHeX) in order to obtain that.

Copy-paste the plugin directory to the `PKHeX/plugins` folder.

Create the `PKHeX/plugins` folder if not already existing.

Open PKHeX and you'll find the tools this Plugin offers under `Tools -> Tera Finder Plugin`.

### Support
If you have any issues, feel free to ask for support in my [Discord server](https://discord.gg/F9nMfvw9sS).


## Credits and Thanks
* [kwsch](https://github.com/kwsch) for [PKHeX](https://github.com/kwsch/PKHeX) and [pkNX](https://github.com/kwsch/pkNX), on which this program relies on
* [Archit Date](https://github.com/architdate), [LegoFigure11](https://github.com/LegoFigure11), [SteveCookTU](https://github.com/SteveCookTU) for their great researches on the item rewards structures/rng handling implemented in [RaidCrawler](https://github.com/LegoFigure11/RaidCrawler/blob/main/Structures/RaidRewards.cs) and [sv raid reader](https://github.com/SteveCookTU/sv_raid_reader/blob/master/src/item_list.rs)
* [Lincoln-LM](https://github.com/Lincoln-LM) for the raid map coordinates in [sv-live-map](https://github.com/Lincoln-LM/sv-live-map)
* [Leanny](https://github.com/Leanny) for his Sword/Shield [PKHeX Raid Plugin](https://github.com/Leanny/PKHeX_Raid_Plugin) which was of inspiration for my plugin
* [santacrab2](https://github.com/santacrab2) for his contributions to make the searches lot faster
* [kwsch](https://github.com/kwsch), [Lusamine](https://github.com/Lusamine), [LegoFigure11](https://github.com/LegoFigure11), [Archit Date](https://github.com/architdate), [SteveCookTU](https://github.com/SteveCookTU), [Lincoln-LM](https://github.com/Lincoln-LM), and all the contributors to the mentioned programs for their awesome researches
* GitHub API implementation provided by [Octokit](https://github.com/octokit/octokit.net), licensed under the MIT license

### Some related cool tools
[sv-live-map](https://github.com/Lincoln-LM/sv-live-map) by [Lincoln-LM](https://github.com/Lincoln-LM)

[RaidCrawler](https://github.com/LegoFigure11/RaidCrawler) by [LegoFigure11](https://github.com/LegoFigure11) and [Archit Date](https://github.com/architdate)

[sv_raid_lookup](https://stevecooktu.github.io/sv_raid_lookup/) ([repo](https://github.com/SteveCookTU/sv_raid_lookup)) by [SteveCookTU](https://github.com/SteveCookTU)


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
