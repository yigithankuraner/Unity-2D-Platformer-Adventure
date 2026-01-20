# Unity-2D-Platformer-Adventure

\# 🏰 2D Action Platformer Adventure



A classic 2D side-scrolling platformer game developed using \*\*Unity Engine\*\* and \*\*C#\*\*. This project demonstrates advanced object-oriented programming (OOP) principles, physics-based character controllers, and state-machine based enemy AI.



\## 📄 Project Description

In this game, the player navigates through hand-crafted levels, avoids obstacles, and defeats enemies to reach the goal. The project focuses on creating a responsive gameplay feel ("game juice") through precise input handling and visual feedback.



\## 🛠️ Key Technical Features



\### 1. Advanced Player Controller (C#)

\* \*\*Physics-Based Movement:\*\* Custom character controller (`PlayerControllerX`) using `Rigidbody2D` for realistic momentum and jump physics.

\* \*\*Ground Detection:\*\* Implemented using `Raycast` or `OverlapCircle` to prevent infinite jumping and ensure precise landing logic.

\* \*\*Animation State Machine:\*\* Smooth transitions between Idle, Run, Jump, and Death animations controlled via code.



\### 2. Enemy AI System

\* \*\*State Machine Logic:\*\* Enemies have distinct behaviors (Patrolling, Chasing, Attacking) managed through scripts.

\* \*\*Waypoint System:\*\* Enemies patrol between defined points and react when the player enters their detection range.

\* \*\*Combat Logic:\*\* Health systems for both player and enemies, including invulnerability frames (i-frames) after taking damage.



\### 3. Core Systems \& UI

\* \*\*Game Manager (Singleton):\*\* Centralized management of game state (Score, Lives, Game Over, Win/Loss conditions).

\* \*\*UI Integration:\*\* Dynamic Health Bars, Main Menu, Pause Menu, and "Game Over" screens created with Unity UI (Canvas).

\* \*\*Scene Management:\*\* Seamless transitions between levels and menu scenes.

\* \*\*Audio System:\*\* Trigger-based sound effects (SFX) for jumping, collecting items, and damage events.



\## 🎮 Controls



| Action | Input Key |

| :--- | :--- |

| \*\*Move Left/Right\*\* | `A` / `D` or `Left Arrow` / `Right Arrow` |

| \*\*Jump\*\* | `Spacebar` or `W` |

| \*\*Interact/Attack\*\* | `E` or `Left Mouse Button` (If applicable) |

| \*\*Pause Game\*\* | `ESC` |



\## 🚀 Getting Started



1\.  Clone the repo:

&nbsp;   ```bash

&nbsp;   git clone \[https://github.com/KULLANICI\_ADIN/REPO\_ISMIN.git](https://github.com/KULLANICI\_ADIN/REPO\_ISMIN.git)

&nbsp;   ```

2\.  Open \*\*Unity Hub\*\*.

3\.  Click \*\*"Add"\*\* and select the cloned folder.

4\.  Open the project (Recommended Unity Version: 6.3 LTS or newer).

5\.  Open the `Scenes/MainMenu` or `Scenes/Level1` scene to start.


---

\*Developed by \[Yiğithan Kuraner] - Computer Engineering Student @ ESTÜ\*

