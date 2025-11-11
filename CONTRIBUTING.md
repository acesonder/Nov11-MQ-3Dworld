# Contributing to MQ3D Virtual Hangout

Thank you for your interest in contributing to the Meta Quest 3 Virtual Hangout Room project! This document provides guidelines for contributing.

## How to Contribute

### Reporting Issues

If you find a bug or have a feature request:

1. Check if the issue already exists in the [Issues](https://github.com/acesonder/Nov11-MQ-3Dworld/issues) section
2. If not, create a new issue with:
   - Clear title
   - Detailed description
   - Steps to reproduce (for bugs)
   - Expected vs actual behavior
   - Screenshots/videos if applicable
   - Unity version and Quest 3 firmware version

### Submitting Changes

1. **Fork the Repository**
   ```bash
   # Fork via GitHub UI, then clone your fork
   git clone https://github.com/YOUR_USERNAME/Nov11-MQ-3Dworld.git
   cd Nov11-MQ-3Dworld
   ```

2. **Create a Feature Branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

3. **Make Your Changes**
   - Follow the coding standards (see below)
   - Add comments for complex logic
   - Update documentation if needed
   - Test thoroughly on Meta Quest 3

4. **Commit Your Changes**
   ```bash
   git add .
   git commit -m "Add feature: your feature description"
   ```

5. **Push to Your Fork**
   ```bash
   git push origin feature/your-feature-name
   ```

6. **Create a Pull Request**
   - Go to the original repository
   - Click "New Pull Request"
   - Select your branch
   - Fill in the PR template
   - Wait for review

## Coding Standards

### C# Code Style

Follow Unity C# coding conventions:

```csharp
// Use PascalCase for public members
public class MyClass
{
    // Use PascalCase for properties
    public string MyProperty { get; set; }
    
    // Use camelCase for private fields with underscore prefix
    private int _myPrivateField;
    
    // Use camelCase for parameters
    public void MyMethod(int myParameter)
    {
        // Use camelCase for local variables
        int localVariable = myParameter;
    }
}
```

### Best Practices

1. **Namespaces**: Always use appropriate namespaces
2. **Documentation**: Add XML comments for public APIs
   ```csharp
   /// <summary>
   /// Brief description of method
   /// </summary>
   /// <param name="paramName">Parameter description</param>
   /// <returns>Return value description</returns>
   ```
3. **Error Handling**: Use try-catch for risky operations
4. **Performance**: Avoid allocations in Update loops
5. **VR Optimization**: Keep frame rate at 72+ FPS

### Unity-Specific

- Use SerializeField for inspector-visible private fields
- Avoid FindObjectOfType in Update loops
- Use object pooling for frequently instantiated objects
- Cache component references
- Use Unity's lifecycle methods properly

## Testing Guidelines

### Before Submitting

- [ ] Code compiles without errors
- [ ] No Unity console errors
- [ ] Tested on Meta Quest 3 device
- [ ] Frame rate is acceptable (72+ FPS)
- [ ] Controllers work as expected
- [ ] No memory leaks
- [ ] Documentation updated

### Test Checklist

1. **Room Capture**
   - Small room (3x3m)
   - Medium space (10x10m)
   - Large area (stadium-sized)

2. **Drawing Tools**
   - All drawing modes work
   - Colors and sizes apply correctly
   - Undo/redo functions

3. **Model Import**
   - FBX, OBJ, GLTF formats
   - Scaling and rotation
   - Physics interactions

4. **World Sharing**
   - Save and load worlds
   - Search functionality
   - Rating system

## Documentation

When adding features:

1. Update README.md if it affects user-facing functionality
2. Update API.md with new public methods
3. Update ARCHITECTURE.md for structural changes
4. Add code comments for complex algorithms

## Community Guidelines

- Be respectful and inclusive
- Provide constructive feedback
- Help others when possible
- Follow the Code of Conduct

## Questions?

- Open a discussion in GitHub Discussions
- Check existing documentation
- Ask in pull request comments

## Recognition

Contributors will be acknowledged in:
- CONTRIBUTORS.md file
- Release notes
- Project README

Thank you for contributing! 🎉
