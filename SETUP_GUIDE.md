# MQ3D Virtual Hangout - Setup Guide

This guide will help you set up the Meta Quest 3 Virtual Hangout Room application from scratch.

## Prerequisites

### Required Software
1. **Unity Hub** (Latest version)
   - Download from: https://unity.com/download
   
2. **Unity Editor 2022.3.10f1**
   - Install via Unity Hub
   - Ensure Android Build Support is included
   - Include Android SDK & NDK Tools
   
3. **Meta Quest Developer Hub** (Optional but recommended)
   - Download from: https://developer.oculus.com/downloads/

### Meta Quest 3 Setup

1. **Enable Developer Mode:**
   - Create a Meta developer account at https://developer.oculus.com
   - In the Meta Quest mobile app:
     - Go to Menu > Devices
     - Select your Quest 3
     - Tap "Developer Mode"
     - Toggle it ON

2. **USB Debugging:**
   - Connect Quest 3 to your computer via USB-C
   - Put on the headset
   - Allow USB debugging when prompted
   - Select "Always allow from this computer"

## Project Setup

### Step 1: Clone and Open Project

```bash
# Clone the repository
git clone https://github.com/acesonder/Nov11-MQ-3Dworld.git
cd Nov11-MQ-3Dworld

# Open with Unity Hub
# Add project from Unity Hub and open with Unity 2022.3.10f1
```

### Step 2: Configure Unity for Quest 3

1. **Switch to Android Platform:**
   ```
   File > Build Settings
   - Select "Android"
   - Click "Switch Platform"
   ```

2. **Configure Player Settings:**
   ```
   Edit > Project Settings > Player
   
   Company Name: AceSonder (or your name)
   Product Name: MQ3D Virtual Hangout
   
   Other Settings:
   - Package Name: com.acesonder.mq3dvirtualhangout
   - Minimum API Level: Android 10.0 (API 29)
   - Target API Level: Android 12.0 (API 32)
   - Graphics APIs: OpenGLES3, Vulkan
   - Color Space: Linear
   ```

3. **Enable XR:**
   ```
   Edit > Project Settings > XR Plug-in Management
   
   - Check "Oculus" under Android tab
   - Install XR Plugin Management if prompted
   ```

4. **Configure XR Settings:**
   ```
   Edit > Project Settings > XR Plug-in Management > Oculus
   
   - Target Devices: Quest 3
   - Stereo Rendering Mode: Multiview
   - V2 Signing (Quest 3): Enabled
   ```

### Step 3: Import Required Packages

The packages should auto-import from `Packages/manifest.json`. Verify these are installed:

- com.unity.xr.meta-openxr: 1.0.1
- com.unity.xr.oculus: 4.1.2
- com.unity.xr.interaction.toolkit: 2.5.2
- com.unity.inputsystem: 1.7.0
- com.unity.netcode.gameobjects: 1.7.1

If any are missing, add them via:
```
Window > Package Manager > Add package by name
```

### Step 4: Create Main Scene

1. **Create a new scene:**
   ```
   File > New Scene
   Select "Basic (Built-in)" or "3D Core"
   Save as: Assets/Scenes/MainScene.unity
   ```

2. **Set up XR Origin:**
   ```
   Right-click in Hierarchy
   XR > XR Origin (Action-based)
   ```

3. **Add Main Manager:**
   ```
   Create Empty GameObject: "ApplicationManager"
   Add Component: VirtualHangoutManager
   Add Component: SpatialCaptureManager
   Add Component: WorldSharingManager
   Add Component: ModelImportManager
   Add Component: VRDrawingTool
   Add Component: VRControllerManager
   ```

4. **Configure Scene:**
   - Add lighting (Directional Light)
   - Add ground plane for testing
   - Set up UI Canvas for VR

### Step 5: Build Settings

1. **Configure Build:**
   ```
   File > Build Settings
   
   Add Open Scenes:
   - Assets/Scenes/MainScene.unity
   
   Platform: Android
   Run Device: Quest 3 (when connected)
   
   Build Settings:
   - Compression Method: LZ4
   - Development Build: ON (for testing)
   ```

2. **Quality Settings:**
   ```
   Edit > Project Settings > Quality
   
   For Android:
   - Set to "Medium" or "High"
   - Adjust based on performance needs
   ```

## Building and Deployment

### Method 1: Direct Build to Device

1. **Connect Quest 3:**
   ```
   - Connect via USB-C cable
   - Ensure device is recognized (check "adb devices" in terminal)
   ```

2. **Build and Run:**
   ```
   File > Build Settings
   - Click "Build and Run"
   - Choose output location
   - Wait for build to complete
   ```

3. **Launch on Quest:**
   - App will automatically launch after build
   - Find in Library > Unknown Sources if needed

### Method 2: Build APK File

1. **Build APK:**
   ```
   File > Build Settings
   - Click "Build"
   - Save as: MQ3DVirtualHangout.apk
   ```

2. **Install via SideQuest or ADB:**
   ```bash
   adb install MQ3DVirtualHangout.apk
   ```

## Testing the Application

### Initial Testing Checklist

- [ ] Application launches without errors
- [ ] Controllers are detected and tracked
- [ ] Main menu appears and is interactive
- [ ] Room capture can be initiated
- [ ] Drawing tools respond to trigger input
- [ ] World browser displays available worlds
- [ ] Model import menu is accessible

### Performance Testing

1. **Check Frame Rate:**
   - Enable "Show Device Stats" in Unity
   - Target: 72 FPS minimum on Quest 3
   - Ideal: 90 FPS or 120 FPS

2. **Test Room Capture:**
   - Capture a small room (3m x 3m)
   - Capture a medium space (10m x 10m)
   - Test large area capture (stadium-sized)

3. **Test Drawing Performance:**
   - Draw simple lines
   - Draw complex patterns
   - Create multiple 3D shapes
   - Test undo/redo functionality

## Common Issues and Solutions

### Issue: Controllers Not Working

**Solution:**
```
1. Check XR Interaction Toolkit is properly installed
2. Verify XR Origin is in the scene
3. Ensure Input System package is installed
4. Check controller bindings in Input Actions asset
```

### Issue: App Won't Build

**Solution:**
```
1. Clear Library folder and reimport
2. Check Android SDK path in Preferences
3. Verify all packages are compatible
4. Update to latest Unity 2022.3 LTS version
```

### Issue: Poor Performance

**Solution:**
```
1. Reduce quality settings
2. Enable GPU instancing on materials
3. Use occlusion culling
4. Optimize mesh complexity
5. Reduce draw calls (check Stats window)
```

### Issue: Spatial Capture Not Working

**Solution:**
```
1. Ensure XR Spatial Awareness is enabled
2. Check camera permissions
3. Verify Scene Understanding is supported
4. Test in a well-lit environment
```

## Development Tips

### Best Practices

1. **Always test on device:**
   - Unity Editor play mode differs from device
   - Performance characteristics are different
   - Controller behavior may vary

2. **Use version control:**
   - Commit frequently
   - Use .gitignore for Unity projects
   - Don't commit Library or Temp folders

3. **Optimize early:**
   - Profile regularly with Unity Profiler
   - Monitor frame times
   - Test with realistic content amounts

4. **Iterate quickly:**
   - Use Development Build for faster iteration
   - Enable auto-build when possible
   - Test incremental changes

### Debugging on Quest 3

1. **Enable Logging:**
   ```csharp
   // In your scripts
   Debug.Log("Message");  // Shows in logcat
   ```

2. **View Logs:**
   ```bash
   adb logcat -s Unity
   ```

3. **Remote Debugging:**
   - Use Unity Profiler over WiFi
   - Enable "Autoconnect Profiler" in Build Settings

## Next Steps

After successful setup:

1. **Explore the codebase:**
   - Review scripts in Assets/Scripts/
   - Understand system architecture
   - Read inline documentation

2. **Customize features:**
   - Modify brush colors and sizes
   - Adjust capture resolution
   - Add custom 3D models

3. **Test thoroughly:**
   - Try all features
   - Test edge cases
   - Gather user feedback

4. **Optimize performance:**
   - Profile the application
   - Optimize hot paths
   - Reduce memory usage

## Resources

- **Unity XR Documentation:** https://docs.unity3d.com/Manual/XR.html
- **Meta Quest Developer Portal:** https://developer.oculus.com
- **Unity Forums:** https://forum.unity.com
- **XR Interaction Toolkit Guide:** https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@latest

## Support

If you encounter issues not covered in this guide:

1. Check the main README.md
2. Review Unity console for errors
3. Open an issue on GitHub
4. Consult Meta Quest developer forums

---

**Happy Building! 🚀**
