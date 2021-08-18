=====================
Grid Placer for Unity
=====================
1. SETUP
2. USE INSTRUCTIONS
3. CREDITS

1. SETUP
Place the included scripts inside the following directory:
"Assets/Editor/GridPlacer/"
If you need these scripts to be placed elsewhere, edit lines 40 & 42 of
GridPlacerUXML.cs, to reflect the new directory of these files.

2. USE INSTRUCTIONS
In the Editor Toolbar, under Tools, click Grid Placer, to open the Grid Placer window.

  a. Base Object
This is the object that will be cloned to create the grid.
If you have selected an object in then scene when you open the Grid Placer, it will appear in this field.
All defined properties of this object will be cloned as they are. If you want to customize the behavior
of this script, add your code to the functions GenerateSG and GenerateHG.

  b. Lengths
Integer field.
These represent the length of the grid in each of the cardinal directions, X, Y, and Z.
Negative values are supported, and will create the grid going in the negative direction.

  c. Grid Offset
Floating Point field.
This is the amount of space between the center of each object within the grid.

  d. Axis toggles
Toggles that set which axes to operate over. When false, the script acts as though that given axis
is set to 1.

  e. Parent toggle
When true, this creates an invisible object that the entire grid will be set as a child of.
This can be useful to adjust the position and rotation of the entire grid at once.

  f. Generate Square Grid
Generates a Square Grid. Objects will be placed evenly along each axis.

  g. Axis Offsets, and the Hex Grid.
In order to generate a Hex Grid, we need to determine how the offsets are set.
The first selected axis controls which axis the offsets are based on.
The second selected axis is the direction in which the offset is applied.
For instance, by default X and then Z are selected, so we say:
"For every other X row, offset everything in the Z direction."
In this case, cells are closer in the X direction. This offset is the chosen Grid Offset, multiplied by the
Sine of 60 degrees. For every other X row, all cells are Half of the Grid Offset further in the Z direction.

  h. Generate Hex Grid
Generates a Hex Grid, as described above.

  i. Undo
The creation of the grid is grouped as one large action, for the purpose of undo-ing.

3. CREDITS
This software written by Cactus Fantastico
https://www.patreon.com/FantasticoSF