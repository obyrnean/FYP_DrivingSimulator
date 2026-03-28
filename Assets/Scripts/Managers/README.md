# Managers Scripts Overview

This folder contains all the **manager scripts** used in the Driving Education Simulation. These scripts handle **main panel interactions, audio, music, scene loading, and UI behaviors** for both the **initial interface** and **Level 1 gameplay**.

---

## AudioManager.cs
- Manages **all audio features** in the simulation.  
- Features:  
  - Controls audio toggles in the main menu  
  - Ensures audio persists across scene transitions (e.g., going from Main Menu to Level 1)  
  - Prevents audio from breaking when switching scenes  

---

## FeedbackManager.cs
- Simulates **user feedback interaction**.  
- Features:  
  - Accepts typed feedback from users in the feedback panel  
  - Displays feedback via printing or UI output  
  - Demonstrates how feedback could be integrated in serious games  

---

## Level1Music.cs
- Controls the **background music for Level 1 gameplay**.  
- Handles starting, stopping, and looping music specific to Level 1 scenes.  

---

## LevelLoader.cs
- Handles **scene transitions** from Main Menu to Level 1.  
- Features:  
  - Loads Level 1 when the player clicks the corresponding button  
  - Integrates with managers to preserve audio and UI state  

---

## MainMenuManager.cs
- Manages **all interactions in the Main Menu**.  
- Features:  
  - Button clicks and panel toggles  
  - Scene selection and navigation  
  - Ensures smooth transitions between menu sections  

---

## MusicController.cs
- Works alongside **AudioManager** to control **background music**.  
- Ensures music persists and behaves consistently across scenes, including Level 1.  

---

## PanelManager.cs
- Manages **panel interactions** within Level 1 gameplay.  
- Features:  
  - Switching between panels  
  - Controlling UI flow for mini-games and decision points  

---

## SoundManager.cs
- Handles **sound effects** in the simulation.  
- Plays sounds triggered by specific interactions (clicks, notifications, feedback events)  
- Works alongside **MusicController** to separate audio channels  

---

## UIButtonHover.cs
- Controls **button animation and interactivity**.  
- Features:  
  - Enlarges buttons when hovering  
  - Changes button color when clicked  
  - Enhances user immersion through responsive UI  

---

These manager scripts ensure the simulation runs smoothly, with **persistent audio, interactive panels, responsive UI, and proper scene management**, creating a seamless user experience from the main menu to Level 1 gameplay.
