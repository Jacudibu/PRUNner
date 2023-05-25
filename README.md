⚠️ PRUNner isn't really actively developed anymore for the time being. While the game data is still auto-updating and the application itself remains perfectly usable, you might want to check out https://prunplanner.org/ for a web-based solution with more features!

# PRUNner
PRUNner is a cross-platform standalone tool supposed to ease up base planning on an empire-wide scale by replacing spreadsheets with a custom-made application, yielding way higher response times than one could ever achieve in Google Sheets.

You can grab the latest version over at our [Releases](https://github.com/Jacudibu/PRUNner/releases): there's a .zip file for every major platform, just unpack and run the PRUNner file inside.

### Always up to date!
The App polls all relevant data from the wonderful [FIO](https://fio.fnar.net/), which means even if there hasn't been an update to the app for quite some time, you can easily update the data yourself! In case you notice any missing recipes or planets, just delete the FIOCache folder before launching the app in order to force an update. I might eventually add menu buttons to do that. 🙃

### Screenshots
First, let's find a planet to settle!
![PlanetFinder](https://user-images.githubusercontent.com/9059719/125678028-648e6575-e968-4440-9f01-e918028c9174.png)

Then, plan out your base!
![BasePlanner](https://user-images.githubusercontent.com/9059719/181455996-45bcc5d8-a5bd-4b90-88c7-00cb53afdeaa.png)

And finally, have an overview over your entire empire.
![EmpireOverview](https://user-images.githubusercontent.com/9059719/121958208-b78f2480-cd63-11eb-953c-c6537b079cd3.png)

### Contributing
I'm open to any kind of feedback or suggestions. This is my second Application using WPF / Avalonia, so there's probably a lot of stuff that could be done better.

Check out our [Issues](https://github.com/Jacudibu/PRUNner/issues) for a list of things that are planned (or have been suggested). If you want to help implementing any of them or add something else, it's probably best to join the [PrUn Community Tools Discord Server](https://discord.gg/2MDR5DYSfY) and/or creating an issue first.

### Building / Running PRUNner from Code
You'll need the .Net 6 SDK installed to build the application from source.
There's no special configuration required. If you want to see how to build the app via the command line for your operating system, take a look at [publish.sh](https://github.com/Jacudibu/PRUNner/blob/main/publish.sh).
