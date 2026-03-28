# InspectionMinigame Scripts Overview

This folder contains all the **C# scripts used for the Vehicle Inspection Mini-Game** in the **initial interface** of the Driving Education Simulation. These scripts manage the car interactions, inspection mechanics, and vehicle selection.

---

## CarInspection.cs
- Handles **different angle views of the car** in the vehicle inspection section.  
- Features:  
  - Smooth rotation of the car left or right as buttons are pressed  
  - Allows players to inspect the vehicle from multiple angles  
  - Provides visual feedback while navigating around the car  

---

## MultiCarInspectionManager.cs
- Manages the **mini-game interactions during the car inspection**.  
- Features:  
  - Provides typing options for users to interact with the inspection task  
  - Checks if player answers are correct or incorrect  
  - Changes the arrows to the proper color based on right, wrong, or default answers  
  - Handles the **Check** and **Reset** interactions of the mini-game  

---

## VehicleSelector.cs
- Handles the **vehicle selection panel** in the mini-game.  
- Features:  
  - Allows the player to browse different vehicles  
  - Updates the vehicle image dynamically when a vehicle is chosen  
  - Manages button interactions for selecting the desired vehicle  

---

These scripts together make the **vehicle inspection mini-game fully interactive**, providing a smooth, responsive, and educational experience for the player.
