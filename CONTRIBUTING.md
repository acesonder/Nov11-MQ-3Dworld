# Contributing to Meta Quest 3D Virtual Hangout Room

Thank you for your interest in contributing! This document provides guidelines and information for contributors.

## Getting Started

1. Fork the repository
2. Clone your fork locally
3. Create a feature branch
4. Make your changes
5. Test thoroughly
6. Submit a pull request

## Development Setup

```bash
# Clone the repository
git clone https://github.com/acesonder/Nov11-MQ-3Dworld.git
cd Nov11-MQ-3Dworld

# Start development server
npm start

# Open browser
open http://localhost:8000
```

## Code Style Guidelines

### JavaScript
- Use ES6+ features (const, let, arrow functions, classes)
- Use meaningful variable names
- Add comments for complex logic
- Follow existing code structure
- Keep functions focused and small

### HTML
- Use semantic HTML5 elements
- Keep markup clean and readable
- Use accessibility attributes (ARIA)
- Maintain consistent indentation (2 spaces)

### CSS
- Use meaningful class names
- Follow BEM naming convention where appropriate
- Keep styles organized by component
- Use CSS custom properties for theming

## Feature Development

### Adding New 3D Objects

```javascript
// Example: Adding a new building type
createCustomBuilding(width, height, depth, position) {
    const geometry = new THREE.BoxGeometry(width, height, depth);
    const material = new THREE.MeshStandardMaterial({ 
        color: 0xcccccc,
        roughness: 0.7,
        metalness: 0.3
    });
    const building = new THREE.Mesh(geometry, material);
    building.position.copy(position);
    building.castShadow = true;
    building.receiveShadow = true;
    this.scene.add(building);
    this.worldObjects.push(building);
    return building;
}
```

### Adding New UI Controls

```html
<!-- In index.html -->
<button id="new-feature-btn">New Feature</button>
```

```javascript
// In main.js setupEventListeners()
document.getElementById('new-feature-btn')
    .addEventListener('click', () => this.newFeature());
```

### Adding New VR Interactions

```javascript
// Handle new controller event
this.controller1.addEventListener('squeezestart', () => {
    // Your interaction code here
});
```

## Testing

### Manual Testing Checklist

- [ ] Desktop mode works (WASD + mouse)
- [ ] All buttons are functional
- [ ] Draw mode creates visible marks
- [ ] Models import correctly
- [ ] Worlds save and load properly
- [ ] No console errors
- [ ] Performance is acceptable (60 FPS)

### VR Testing Checklist

- [ ] VR mode enters successfully
- [ ] Controllers are visible and tracked
- [ ] Controller interactions work
- [ ] No motion sickness issues
- [ ] Performance is smooth (72+ FPS)
- [ ] UI is readable in VR

### Cross-Browser Testing

Test on:
- Chrome/Edge (Windows, Mac, Android)
- Firefox (Windows, Mac)
- Safari (Mac, iOS) - limited WebXR support
- Meta Quest Browser

## Pull Request Process

1. **Before submitting:**
   - Test your changes thoroughly
   - Update documentation if needed
   - Ensure no console errors
   - Check code style

2. **PR Description should include:**
   - What changes were made
   - Why the changes were necessary
   - How to test the changes
   - Screenshots/videos if UI changed
   - Any breaking changes

3. **PR Title format:**
   ```
   [Type] Brief description
   
   Types: Feature, Fix, Docs, Style, Refactor, Test, Chore
   
   Examples:
   [Feature] Add multiplayer support
   [Fix] Resolve drawing color picker bug
   [Docs] Update installation instructions
   ```

## Feature Requests

We welcome feature requests! Please:

1. Check if the feature already exists or is planned
2. Open an issue with the "enhancement" label
3. Describe the feature and use case clearly
4. Include mockups or examples if applicable

### Roadmap Items

Current priorities:
- Multiplayer networking
- Advanced physics
- Better mobile support
- Cloud world storage
- Avatar customization
- Voice chat integration

## Bug Reports

When reporting bugs, include:

1. **Description:** What happened vs. what should happen
2. **Steps to reproduce:** Detailed steps to recreate the bug
3. **Environment:** Browser, OS, device (VR headset model)
4. **Console logs:** Any error messages
5. **Screenshots/videos:** Visual evidence if applicable

### Bug Report Template

```markdown
**Bug Description:**
[Clear description of the issue]

**Steps to Reproduce:**
1. Go to '...'
2. Click on '...'
3. See error

**Expected Behavior:**
[What should happen]

**Actual Behavior:**
[What actually happens]

**Environment:**
- Browser: Chrome 120
- OS: Windows 11
- Device: Meta Quest 3
- Application version: 1.0.0

**Console Errors:**
```
[Paste console output]
```

**Screenshots:**
[Attach screenshots]
```

## Code Review Process

All contributions go through code review:

1. Automated checks must pass
2. Code must follow style guidelines
3. Changes must be tested
4. At least one approval required
5. No merge conflicts

## Performance Guidelines

### Keep the app performant:

1. **Optimize 3D Models:**
   - Keep polygon count reasonable
   - Use LOD when possible
   - Compress textures

2. **Efficient Rendering:**
   - Don't create objects in render loop
   - Dispose of unused geometries/materials
   - Use object pooling for frequent creations

3. **Memory Management:**
   ```javascript
   // Always dispose when removing objects
   removeObject(obj) {
       this.scene.remove(obj);
       if (obj.geometry) obj.geometry.dispose();
       if (obj.material) {
           if (Array.isArray(obj.material)) {
               obj.material.forEach(m => m.dispose());
           } else {
               obj.material.dispose();
           }
       }
   }
   ```

## Accessibility

Ensure features are accessible:

- Add ARIA labels to UI elements
- Support keyboard navigation
- Provide text alternatives for visual content
- Test with screen readers when applicable
- Consider users with motion sensitivity

## Documentation

Update documentation when:

- Adding new features
- Changing existing behavior
- Adding new dependencies
- Modifying setup process
- Fixing bugs that affect documentation

### Documentation Locations

- `README.md` - Overview and quick start
- `DEPLOYMENT.md` - Deployment instructions
- `CONTRIBUTING.md` - This file
- `assets/README.md` - Asset guidelines
- Code comments - Complex logic

## Community

### Getting Help

- Open an issue for bugs or questions
- Check existing issues and discussions
- Read the documentation thoroughly
- Test on a clean install first

### Code of Conduct

Be respectful and constructive:
- Be welcoming to newcomers
- Accept constructive criticism
- Focus on what's best for the project
- Show empathy towards others

## Recognition

Contributors will be:
- Listed in project credits
- Mentioned in release notes
- Acknowledged in the README

## License

By contributing, you agree that your contributions will be licensed under the same MIT License that covers the project.

## Questions?

Feel free to open an issue with the "question" label or reach out to the maintainers.

---

Thank you for contributing to the Meta Quest 3D Virtual Hangout Room! 🥽✨
