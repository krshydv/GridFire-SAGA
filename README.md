# GridFire Saga

GridFire Saga is a fast-paced **2D top-down wave shooter** built with **Unity 6.3 LTS (6000.3.x)**.  
Fight increasingly difficult enemy waves, aim freely with the mouse, dash to evade danger, and survive as long as possible while chasing a high score.

---

## Features

- **Top-down movement** (WASD)
- **Mouse aiming** + shooting
- **Dash ability** (short invincibility window)
- **Wave-based enemy spawns** with difficulty scaling
- **Basic enemy types** (Grunt + Dasher)
- **Event-driven UI** (wave, score, enemies remaining, ammo, health)
- **Object pooling** for bullets (performance-friendly)

---

## Controls

| Input | Action |
|------:|--------|
| W / A / S / D | Move |
| Mouse | Aim |
| Left Click | Shoot |
| R | Reload |
| Space | Dash |
| Esc | Pause |

---

## Requirements

- **Unity Hub**
- **Unity 6.3 LTS** (6000.3.x)
- (Optional) **VS Code** + C# extensions for editing scripts

---

## Getting Started

### 1) Clone the repo
```bash
git clone https://github.com/krshydv/GridFire-SAGA.git
cd GridFire-SAGA
```

### 2) Open in Unity Hub
1. Open **Unity Hub**
2. Go to **Projects**
3. Click **Add** (or **Open**) and select the repository folder
4. Launch the project using **Unity 6.3 LTS**

> Unity will regenerate the `Library/` folder automatically (it should not be committed).

---

## Project Structure

Key directories:

- `Assets/Scripts/` — gameplay scripts (player, enemies, weapons, UI, managers)
- `Assets/Prefabs/` — prefabs (player, bullets, enemies)
- `Assets/Scenes/` — game scenes
- `ScriptableObjects/` — stats/assets for player, gun, and enemies (created inside Unity)

---

## How to Play (Editor)

1. Open the main scene (example): `Assets/Scenes/GameScene.unity`
2. Press **Play** in the Unity Editor

---

## Notes for Contributors

- Please keep code formatted and readable.
- Unity `.meta` files are required and should be committed.
- Avoid committing generated folders like `Library/`, `Temp/`, `Obj/`, `Logs/`.

---

## Roadmap Ideas

- Additional enemy types (ranged, tank, boss)
- Weapon attachments / upgrades
- Better VFX/SFX and animations
- Save system for high scores
- Settings menu (volume, sensitivity, resolution)

---
