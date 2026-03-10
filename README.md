> **Last updated:** Jan 24, 2026  
> ⚠️ Everything is under development and subject to change.

Project Hidden Threads by Midnight Umbrella
CMPUT 250 - University of Alberta
# Hidden Threads
**Team:** Midnight Umbrella
**Platform:** Unity (WebGL) • Short-session narrative mystery

## 🏗️ Project Structure
> Solve your friend’s murder by investigating physical spaces and uncovering the truth hidden inside their social media—before the evidence disappears.

- `Project/` — Unity project root  
  - `Assets/` — scripts, scenes, prefabs, art, audio  
  - `Packages/` — package manifest  
  - `ProjectSettings/` — Unity settings  
- `Docs/` — pitch materials, milestone PDFs, references, screenshots  
- `Builds/` — exported builds (e.g., WebGL) *(optional; often excluded from git)*

---

## 🚀 Getting Started
### Prerequisites
- **Unity Hub**
- Unity version: **2019.4.18f1** or **2022.3.12f1** (course-supported)
- Git (optional, for contributors)

### Run in Unity
1. Unity Hub → **Open** → select `Project/`
2. Open the main scene (e.g., `Assets/Scenes/Main.unity`)
3. Press **Play**

### Build (WebGL / itch.io)
1. `File → Build Settings`
2. Select **WebGL** → **Switch Platform**
3. Click **Build** and upload the generated folder to itch.io as an HTML project

---

## Assumptions and Tradeoffs
- **Scope-first:** We prioritize a playable vertical slice (core loop + 1–2 small levels) over large content volume.
- **Readable “digital UI” over realism:** Social media/phone screens may be simplified for clarity and fast iteration.
- **Limited branching:** Choices may affect available leads, but we avoid heavy branching narratives to reduce writing/implementation load.
- **Content-light, systems-heavy:** We’ll reuse templates for posts/DMs and focus more on investigation mechanics + evidence linking.
- **Asset constraints:** T.B.D.
- **WebGL constraints:** We avoid platform-specific features and keep performance reasonable for browser builds.


## How to Run (Unity)
Recommended Unity versions: **2019.4.18f1** or **2022.3.12f1**

1. Open Unity Hub → **Open** → select the Unity project folder  
2. Load the main scene (e.g., `Assets/Scenes/Main.unity`)  
3. Press **Play**

### WebGL Build (for itch.io)
1. `File → Build Settings` → Platform: **WebGL** → **Switch Platform**  
2. **Build** → upload the build folder to itch.io (HTML)

---

## Credits 
- **Alan**: Producer, Programming, Documentation / QA Testing
- **Divine**: UI/UX Lead, Programming
- **Jenny**: Visual Design Lead, Digital Media
- **Mashhood**: Story / Writing Lead
- **Nate**: Level Design Lead, Programming, Sound Design Assistance
- **Thomas**: Sound Design Lead, Programming Assistance



