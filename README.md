# MC-LCE-Rewritten

## What Is This Project?
This is a **Rewrite** (or Decompilation) of Minecraft for legacy consoles, also known as **Legacy Console Edition (LCE) or just Legacy.**

My main goal is to create a **single version of Minecraft Legacy** that runs on PC (See [Why PC?][wpc]) and that is supported and updated to the latest versions of Minecraft by developers and modders.



This rewrite of the game will be in **C#** (yet to be confirmed) using **OpenGL** (yet to be confirmed) as the graphics API. It will aim to be as close to the original source code and game behaviors **as possible.**

_But in reality, I probably don't even know what I'm doing lol🦆_

## What Is Minecraft Legacy Console Edition?
Minecraft for legacy consoles was a version of Minecraft ported to consoles by [4J Studios][4j].

This version of the game was discontinued around 2019 and was replaced by the **Bedrock Edition.**

## How Do I Help?
A lot of progress is being made! I will post the progress in this amazing Discord server :}

Link: [https://discord.gg/v3KCbd7K6x](https://discord.gg/v3KCbd7K6x)

If you want to help me, you are free to pop into the server! 🙂

## What Are My Plans/Intentions?
Basically, rewrite the entire Xbox 360 Edition base game (TU0) using the Java Edition's code and some reverse engineering of the binaries to help.

### To-Do List
- [ ] Java stripped down version/Simplified version.

Basically, take away as many pieces of code as possible from beta 1.6.6 to simplify it as much as possible.

Because rewriting everything in one go is too difficult and there's a 99% chance of it not working right away. So, simplifying the Java code first, then converting it to C#, will give me a great starting point to put all the other pieces, like a **Puzzle.**

---
- [ ] Convert the Simplified Version to C#

Convert the Java Simplified version code to C# using OpenTK.

---

- [ ] Continue to add things to this To-Do List :}

## Useful Things to Know :)
I mentioned the Java version code because the TU0 is just a port of the Java Beta 1.6.6. This is confirmed by a string found in the binaries and for other reasons.

So, as I said, TU0 is based on beta 1.6.6. Using RetroMCP, I got the code for that version, so most of the rewrite will be based on that code.

Like I said in [Todo], it's like a BIG **Puzzle** where I have a reference (The game) and I have to put all the pieces (The code) together to make it work.

#### (not very useful lol)
4J most likely took the beta 1.6.6 code and modified it for the Xbox and to play with a controller, also leaving in the binaries useless pieces of code😅(Love you 4J😊❤️)


## Why PC?
This "Version" of Minecraft Legacy it will probably run only on PC for the reason that we cannot publish it on the Consoles Stores.

Obviously the controllers of the various consoles will be compatible, otherwise it wouldn't be Minecraft Legacy :)


## Tools I Used
To view Minecraft's Java beta 1.6.6 code, I used:
* [RetroMCP][rmcp]: RetroMCP is a modification of the Minecraft Coder Pack.

---

For reverse engineering parts of the game in the binaries of the console's executable, I used: 
* [Ghidra][ghi]: Ghidra is a software reverse engineering (SRE) framework created by the National Security Agency.

---

For loading the .xex along with Ghidra, I used: 
* [XEXLoaderWV][xel]: XEXLoaderWV is a .xex file loader for Ghidra.

---
For rendering, I used:
* [OpenTK][otk] The Open Toolkit is a low-level C# binding for OpenGL, OpenAL, etc., and it runs on all major platforms

[rmcp]: https://github.com/MCPHackers/RetroMCP-Java
[ghi]: https://github.com/NationalSecurityAgency/ghidra
[xel]: https://github.com/zeroKilo/XEXLoaderWV
[4j]: https://www.4jstudios.com
[Todo]: https://github.com/AleBello7276/MC-LCE-Rewritten#to-do-list
[otk]: https://opentk.net
[wpc]: https://github.com/AleBello7276/MC-LCE-Rewritten#why-pc
