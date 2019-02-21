# RedMachineTest
class GameConfig
  It contains settings fields for game
  
class Team
  This class includes two field: name and Color of team;
  
class Circle
  This class decribes circle in game
  
class SavedState 
  It used to store data about saved state of simulation.

Controllers:
CameraController attached to MainCamera object and make right Ortographic Size for current area size

HUD controller used to display info on UI

CircleController is responsible for circle behavior, like collision, destroying, init oject of Circle and store it in field.

Game Manager:
  It manages the entire game process: load config, spawn circle, save and load data.
