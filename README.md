# Meta Quest 3D Virtual Hangout Room

A web-based VR application for Meta Quest that allows users to create, capture, and share virtual spaces as large as professional stadiums. Features include VR room capture, 3D drawing tools, model importing, and world sharing.

## Features

### 🥽 VR Support
- Full Meta Quest headset integration via WebXR
- Desktop fallback mode with keyboard/mouse controls
- VR controller support for navigation and drawing

### 🏟️ Space Capture
- Capture virtual environments as large as professional stadiums
- Bounding box visualization for captured spaces
- Save and track multiple space captures
- Stadium-scale environment (200x200 units)

### 🎨 3D Drawing Tools
- Real-time 3D drawing in VR and desktop modes
- Color picker for customizing drawing colors
- Draw mode toggle for precision control
- Line connections between drawing points
- Clear drawings functionality

### 📦 3D Model Import
- Support for GLTF/GLB 3D model formats
- Drag-and-drop model import
- Automatic shadow casting and receiving
- Model placement in the scene

### 🌍 World Sharing
- Save complete world states to browser storage
- Load previously saved worlds
- World list with timestamps
- Share environments between sessions

## Getting Started

### Prerequisites
- Modern web browser with WebXR support (Chrome, Edge, Firefox)
- Meta Quest headset (optional, desktop mode available)
- HTTPS connection (required for WebXR)

### Installation

1. Clone this repository:
```bash
git clone https://github.com/acesonder/Nov11-MQ-3Dworld.git
cd Nov11-MQ-3Dworld
```

2. Serve the application using a local web server with HTTPS:

**Option 1: Using Python**
```bash
python -m http.server 8000
```

**Option 2: Using Node.js http-server**
```bash
npx http-server -p 8000
```

**Option 3: Using VS Code Live Server**
- Install the Live Server extension
- Right-click on `index.html` and select "Open with Live Server"

3. Access the application:
- Desktop: Open `http://localhost:8000` in your browser
- Meta Quest: Navigate to your computer's local IP address in the Quest browser

### HTTPS Setup (Required for VR)

WebXR requires HTTPS. For local development:

1. **Using ngrok** (Recommended for Meta Quest testing):
```bash
ngrok http 8000
```
Then use the provided HTTPS URL on your Quest browser.

2. **Using mkcert** (Local SSL certificates):
```bash
# Install mkcert
brew install mkcert  # macOS
# or
choco install mkcert  # Windows

# Create certificates
mkcert -install
mkcert localhost 127.0.0.1 ::1

# Serve with HTTPS
# Use any HTTPS-capable server with the generated certificates
```

## Usage

### Desktop Controls
- **WASD**: Move around the environment
- **Mouse**: Look around
- **Space**: Move up
- **Shift**: Move down
- **Click**: Draw (when draw mode is enabled)

### VR Controls
- **Enter VR Mode**: Click the VR button to enter immersive mode
- **Controllers**: Use Quest controllers to navigate
- **Trigger**: Draw in 3D space (when draw mode is enabled)

### Features Guide

#### Capturing Spaces
1. Navigate to the area you want to capture
2. Click "Capture Space" button
3. A green wireframe box appears showing the captured volume
4. Capture data is stored and can be saved with your world

#### 3D Drawing
1. Click "Draw Mode" button to enable drawing
2. In desktop mode: Click on surfaces to place drawing points
3. In VR mode: Use controller trigger to draw in 3D space
4. Use color picker to change drawing color
5. Click "Erase Mode" to clear all drawings

#### Importing 3D Models
1. Click "Import 3D Model" button
2. Select a GLTF (.gltf) or GLB (.glb) file
3. Model appears in front of you in the scene
4. Models are automatically added to your world

#### World Management
1. **Save World**: Saves current state including objects, drawings, and captures
2. **Load World**: Loads the most recent saved world
3. **World List**: Click on any saved world to load it
4. **Clear World**: Removes all user-created content

## Technical Details

### Architecture
- **Three.js**: 3D rendering engine
- **WebXR**: VR/AR API for Meta Quest integration
- **GLTFLoader**: 3D model loading
- **LocalStorage**: Persistent world data storage

### Scene Components
- Stadium-sized ground plane (200x200 units)
- Grid helper for scale reference
- Sample buildings and stadium seating
- Central platform
- Dynamic lighting with shadows

### Browser Compatibility
- Chrome 90+ (Recommended)
- Edge 90+
- Firefox 98+
- Meta Quest Browser

### Performance
- Real-time rendering at 60+ FPS on desktop
- 72Hz or 90Hz on Meta Quest (depending on device)
- Optimized shadow mapping
- Efficient geometry instancing

## File Structure
```
Nov11-MQ-3Dworld/
├── index.html          # Main HTML file with UI
├── js/
│   └── main.js        # Core VR application logic
└── README.md          # This file
```

## Development

### Adding Custom Features

#### Adding New 3D Objects
```javascript
const geometry = new THREE.BoxGeometry(5, 5, 5);
const material = new THREE.MeshStandardMaterial({ color: 0xff0000 });
const mesh = new THREE.Mesh(geometry, material);
mesh.position.set(0, 2.5, 0);
mesh.castShadow = true;
mesh.receiveShadow = true;
this.scene.add(mesh);
this.worldObjects.push(mesh);
```

#### Customizing Environment
Modify the `setupEnvironment()` and `createSampleStructures()` methods in `main.js` to change the initial scene.

### Testing

#### Desktop Testing
1. Open the application in a browser
2. Use keyboard/mouse controls to navigate
3. Test all features in desktop mode

#### VR Testing
1. Ensure HTTPS is enabled
2. Open on Meta Quest browser
3. Click "Enter VR" button
4. Test with Quest controllers

## Troubleshooting

### VR Button Not Working
- Ensure you're using HTTPS
- Check browser WebXR support
- Verify Meta Quest is connected and updated

### Models Not Loading
- Ensure file is in GLTF/GLB format
- Check file size (keep under 50MB for best performance)
- Verify file is not corrupted

### Performance Issues
- Reduce number of objects in scene
- Lower shadow map resolution
- Disable fog if needed
- Clear old drawings regularly

## Known Limitations

- LocalStorage has size limits (~5-10MB)
- Complex models may impact performance
- World sharing requires manual export/import
- VR controllers simplified (no detailed hand tracking)

## Future Enhancements

- [ ] Multiplayer support with WebRTC
- [ ] Cloud-based world storage
- [ ] Advanced hand tracking
- [ ] Physics engine integration
- [ ] Voice chat capabilities
- [ ] Texture painting tools
- [ ] Procedural terrain generation
- [ ] Cross-platform world sharing

## Contributing

Contributions are welcome! Please feel free to submit issues or pull requests.

## License

This project is open source and available under the MIT License.

## Credits

- Built with [Three.js](https://threejs.org/)
- VR support via [WebXR](https://immersiveweb.dev/)
- Developed for Meta Quest platform

## Support

For issues or questions:
- Open an issue on GitHub
- Check the troubleshooting section above
- Refer to [Three.js documentation](https://threejs.org/docs/)
- Refer to [WebXR documentation](https://immersiveweb.dev/)

---

**Note**: This is a demonstration application showing the core concepts of VR space capture and sharing. For production use, consider implementing proper backend storage, user authentication, and multiplayer synchronization.