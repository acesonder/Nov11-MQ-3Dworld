# Assets Directory

This directory contains 3D models, textures, and other assets for the Virtual Hangout Room.

## Directory Structure

```
assets/
├── models/          # 3D models in GLTF/GLB format
├── textures/        # Texture images (PNG, JPG)
└── README.md        # This file
```

## Adding 3D Models

### Supported Formats
- GLTF (.gltf) - Text-based format with separate texture files
- GLB (.glb) - Binary format with embedded textures (recommended)

### Model Guidelines
- Keep file sizes under 50MB for optimal performance
- Use PBR materials for realistic rendering
- Include proper UV mapping
- Optimize polygon count (aim for < 100K triangles)
- Ensure proper scale (1 unit = 1 meter)

### Recommended Sources for Free 3D Models
- [Sketchfab](https://sketchfab.com/) - Look for "Downloadable" models with CC licenses
- [Google Poly Archive](https://poly.pizza/) - Free 3D models
- [Kenney.nl](https://kenney.nl/assets) - Free game assets
- [Quaternius](http://quaternius.com/) - Free low-poly models

### Using Models in the Application
1. Download a GLTF/GLB model
2. Place it in the `assets/models/` directory (optional)
3. In the application, click "Import 3D Model"
4. Select your model file
5. The model will appear in the scene

## Adding Textures

### Supported Formats
- PNG (recommended for transparency)
- JPG (for photos/realistic textures)

### Texture Guidelines
- Keep resolution reasonable (1024x1024 or 2048x2048)
- Use power-of-two dimensions when possible
- Compress images to reduce file size
- Use proper naming conventions (e.g., `brick_diffuse.png`, `brick_normal.png`)

## Examples

You can add sample models to test the application:

1. **Simple Cube** - For testing basic import
2. **Character Model** - For avatar representation
3. **Furniture** - For room decoration
4. **Props** - For interactive elements

## Performance Tips

- Use LOD (Level of Detail) models when available
- Compress textures using tools like [Squoosh](https://squoosh.app/)
- Combine meshes when possible
- Use instancing for repeated objects
- Consider using Draco compression for GLTF files

## License Considerations

When using 3D models:
- Always check the license terms
- Attribute creators when required
- Respect commercial use restrictions
- Don't redistribute without permission

---

For more information, see the main README.md file in the project root.
