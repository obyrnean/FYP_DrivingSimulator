# Level1 Scene Scripts Overview

This folder contains all the **C# scripts used for interactions and gameplay logic in Level 1** of the Driving Education Simulation.  

Each script corresponds to a particular section or scene in Level 1 and manages the **interactive elements, instructor feedback, and user decisions**.

---

## Structure and Flow
- The scripts follow the **structured order of gameplay**, starting from the **InspectionMinigame.cs**, which is the first interactive mini-game at the beginning of Level 1.  
- Scripts are numbered according to the sequence of scenes (e.g., Scene2, Scene3 … Scene42).  
- Most scripts share **similar coding patterns**, handling:  
  - Instructor messages and feedback  
  - User interaction buttons (multiple choice, clik on sign, click in order, right/wrong)  
  - Decision points with branching outcomes  
  - Repetition options for incorrect or repeatable tasks  

---

## Branching Scenes
- Scripts with names like `SceneX_2` represent **branching points**:  
  - Depending on the player’s choice in the previous scene, the game can lead to either **SceneX** or **SceneX_2**  
  - This allows for dynamic decision-based progression throughout Level 1  

---

## Personalization
- Most user interactions are **customized via the Unity Inspector**, allowing for:  
  - Scene-specific feedback messages  
  - Tailored UI buttons and interactive elements  
  - Flexible branching and repeated interactions without changing the script code  

---

- These scripts collectively create a **seamless, interactive learning experience** in Level 1.  
- They ensure that players receive proper guidance, feedback, and decision-based gameplay while progressing through the lesson from start to finish.
