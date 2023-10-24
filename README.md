# MC-LCE-Rewritten

### What is this?
This is a **Rewrite** (or Decompilation) of the version for Consoles of Minecraft or also know as **Legacy Console edition (LCE) or just Legacy**

this will be a rewrite in **C#** (yet to be confirmed) using **OpenGL** (yet to be confirmed) as the graphics API, it will be as close to the original source code and game behaviors **As Possible**


(But in reality I probably don't even know what I'm doing lol🦆)


### What is Minecraft Legacy?
Minecraft for console or Legacy was A version of Minecraft ported to consoles by [4J Studios][4j]

This version was discontinued around 2019 and got replaced by the **Bedrock Edition**

## How to help?
A lot of the progress I will post in this amazing discord server :}

https://discord.gg/v3KCbd7K6x

so if you want to help me, you are free to pop into the server🙂

## What i want to do
Basically Rewrite the entire Xbox360 Base Game (TU0) using the java code and some reverse engineering for helping


### Todo
- [ ] Java stripped down version/Simplified version

Basically take away as many pieces of code as possible from beta 1.6.6 to simplify it as much as possible

Because rewriting everything in one go is too difficult 99% won't work right away, so simplifying the java code first then converting it to c# will give me a great starting point to put all the other pieces, like a **Puzzle**

---
- [ ] Convert the Simplified version to C#

Convert the java Simplified version code to c# using OpenTk

---

- [ ] Continue add thing to this Todo list :}

## Usefull things to know :)
I mentioned the Java version code beacuse the TU0 is just a port of the Java Beta 1.6.6, this is confirmed by a string found in the binaries and also for other reasons.

So as i said TU0 is based on beta 1.6.6, using RetroMCP i got the code for that version so most of the rewrite will be based on that code

Like i said in [Todo] it's like a BIG **Puzzle** where i have a reference (The game) and I have to put all the pieces (The code) together to make it work

#### (not very usefull lol)
4J most likely took the beta 1.6.6 code and modified it for the xbox and to play with a controller, also leaving in the binaries useless pieces of code😅(Love you 4J😊❤️)



## Tools I used
To view the Minecraft's Java beta 1.6.6 code i used:
* [RetroMCP][rmcp]: RetroMCP is a modification of the Minecraft Coder Pack.

---

For reverse engineering parts of the game in the binaries of the consoles executable i used: 
* [Ghidra][ghi]: Ghidra is a software reverse engineering (SRE) framework created by the National Security Agency.

---

For loading the .xex along with ghidra i used: 
* [XEXLoaderWV][xel]: XEXLoaderWV is a .xex file loader for Ghidra.

---
For rendering i used:
* [OpenTK][otk] The Open Toolkit is a low-level C# bindings for OpenGL, OpenAL etc and it runs on all major platforms



[rmcp]: https://github.com/MCPHackers/RetroMCP-Java
[ghi]: https://github.com/NationalSecurityAgency/ghidra
[xel]: https://github.com/zeroKilo/XEXLoaderWV
[4j]: https://www.4jstudios.com
[Todo]: https://github.com/AleBello7276/MC-LCE-Rewritten/edit/main/README.md#todo
[otk]: https://opentk.net
