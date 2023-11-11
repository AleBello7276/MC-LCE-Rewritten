# MC-LCE-Rewritten

## What Is This Project?
This is a **Rewrite** (or Decompilation) of Minecraft for legacy consoles, also known as **Legacy Console Edition (LCE) or just Legacy.**

My main goal is to create a **single version of Minecraft Legacy** that runs on PC (See [Why PC?][wpc]) and that is supported and updated to the latest versions of Minecraft by developers and modders.



This rewrite of the game will be in **C#** (yet to be confirmed) using **OpenGL** (yet to be confirmed) as the graphics API. It will aim to be as close to the original source code and game behaviors **as possible.**

_But in reality, I probably don't even know what I'm doing lol🦆_

## What Is Minecraft Legacy Console Edition?
Minecraft for legacy consoles was a version of Minecraft ported to consoles by [4J Studios][4j].

This version of the game was discontinued around 2019 and was replaced by the **Bedrock Edition.**

## Progress
<p align="center">
  <img src="https://github.com/AleBello7276/MC-LCE-Rewritten/blob/main/img/Bar_v1.png" width="50%" >
</p>

This is the progress of the **current Goal**.
(thanks MattKC for the idea ;})
## How Do I Help?
A lot of progress is being made! I will post the progress in this amazing Discord server :}

Link: [https://discord.gg/v3KCbd7K6x](https://discord.gg/v3KCbd7K6x)

If you want to help me, you are free to pop into the server! 🙂

## What Are My Plans/Intentions?
Basically, rewrite the Minecraft Console Edition using the Java Edition's code and reverse engineering of the binaries to help.

### To-Do List
- [ ] Convert **beta 1.6.6** from Java to C#

Since the first version of the legacy was based using the beta 1.6.6 code I will start with this version and slowly add the missing pieces like a **Puzzle**

---
- [ ] Implement all the features of the Legacy version

(Crafting, Controller Inputs etc)
As I said before "add the missing pieces"

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
