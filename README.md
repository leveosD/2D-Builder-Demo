# Builder

A Unity project focused on modular architecture and grid-based building placement. The project demonstrates separation of UI and gameplay logic, extensible interaction modes, and JSON-based building data serialization.

## Architecture

The scripts are organized into two Assembly Definition modules:

* **UIModule** — handles UI button interactions and renders building selection buttons based on the buildings available in the project.
* **TileModule** — contains the logic for placing and removing buildings on the game field.

A **Bootstrap** component loads the main scene containing the game field and user interface.

## Gameplay Systems

### Interaction Modes

The `IPlayMode` interface defines two methods: `Submit` and `Cancel`. `InputHandler` invokes them when the player presses the left and right mouse buttons, respectively.

This design allows interaction modes to be extended independently of the input system. The project currently implements:

* `PlaceMode` — places buildings on the grid.
* `DeleteMode` — removes existing buildings.

Additional modes, such as moving an already placed building, can be implemented by adding new `IPlayMode` implementations.

## Data Storage

Building definitions are stored as JSON files in the `Resources` directory. Each definition contains the building's name and dimensions.

Saved layouts are serialized into a separate JSON file containing the names of placed buildings and the coordinates of their top-left corners.

## Technologies

* **Unity Tilemaps**
* **Assembly Definition Files** — modular code organization
* **DOTween**
