# Unity Scripts
### For Unity 2018.4.24f (Might not work with newer versions)
---

## Scripts

### Dice Game

A small minigame where you roll 3 dices for 3 consecutive rolls. If
the sum of all faces is 7 in the 3 rolls, you win.

This minigame uses dices that roll with physics instead of precalculating
a random result and determines the value of the face of the dice based on
the rotation of the dice.

### FPS Character Controller

A custom made FPS controller based on quake-like games (Mostly HL1). You can
run/walk, jump, and crouch. Still work in progress.

### Keep Object Upright

It does exactly what the name suggests, it keeps the object containing this
script as a component upright permanently.

### Permanent Rotation

Keeps the object rotating.

### UI Raycasts

Script for in-world UI buttons. It detects all the canvas objects in the scene
and gives you the ability to interact with their buttons while in the game.
Requires to set the content of the scene inside a 'root' object that you need
to specify in the script parameters. Non interactable canvases can be placed
outside of the root object.