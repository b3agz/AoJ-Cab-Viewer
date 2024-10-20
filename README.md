# AoJ Cab Viewer - An AGE of Joy Arcade Cabinet Viewer Tool

This little application is designed to allow players and modders of the VR retro arcade exerience, Age of Joy, to view arcade cabinets on their PC. AoJCabViewer will load the files and data from the folder and construct the cab as it would be in the game.

Click "Load Cabinet" in the top left corner to load a folder. Folders must contain at least a "description.yaml" file and a *.glb file to for AoJCabViewer to load them.

Once loaded, use the right mouse button to rotate the camera, the left button to move around, and the scroll wheel to zoom in and out.

Please note that, while great pains have been taken to make the cabs in here look exactly as they would in the game, there will be minor differences due to this being a completely independent application using a different render pipeline to AoJ. That being said, it should be close enough in the vast majority of situations.

Hopefully it will get better with time and feedback :)

## AGE of Joy

For more information about the game that this tool is designed to compliment, head on over to their [Itch page](https://curifab.itch.io/age-of-joy). Or, if you're looking to dive into the code, you could check out the [Git repo](https://github.com/curif/AgeOfJoy-2022.1).

## License

This project is licensed under the [GNU General Public License v3.0 (GPL-3)](./gpl-3.0.md).

It also makes use of code and libraries that are made available under the [MIT License](./mit.md). These include:

- YAMLDotNet
- UnitySimpleFileBrowser
- UnityGLTF