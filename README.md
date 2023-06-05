# Space Invaders
3D Space Invaders game prototype.

### Goals:
The main goal of this project is to recreate Space Invaders with gameplay variation and multiple technical objectives. Each enemy has a specific color. When an enemy of a particular color is destroyed, all connected enemies of the same color are also destroyed.

![Game Image](https://user-images.githubusercontent.com/62396712/125202080-f7b1cc00-e269-11eb-948e-28728ffef289.png)

### Leaderboards:
Leaderboards display the top 10 users, sorted by score. The game supports local leaderboard storage but can be easily adapted for server-based storage. All leaderboard operations are asynchronous and responsive. Users must obtain a valid Auth Token from the Auth service before accessing leaderboards.

### Services Contracts:
API contract details.

### Game Flow:
The player begins at the main menu, then plays a session. The player can submit their score to the leaderboard if it exceeds any existing scores. The game returns to the main menu after the end of each session.

### Discussion Points:
- **Architecture**: The game uses a custom architecture designed to minimize reliance on `MonoBehaviour`.
- **Game states**: The game uses a state machine for transitions.
- **Code flow**: A single `MonoBehaviour` manages game flow.
- **Game Systems**: These create and manage game entities.
- **Game Views**: These represent in-game actors.
- **Game Settings & Parameters**: All settings and system configurations are stored in `scriptableObject` assets.
- **Services Contract**: A local leaderboard system with RestClient utility for handling database requests.
- **Game Systems**: A variety of helper systems to manage game mechanics.

